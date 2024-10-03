using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Motostation.DataService;
using Motostation.DTOs;
using Motostation.Models;

namespace Motostation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MyDbContext _db;
        private readonly TokenGenerator _tokenGenerator;
        private readonly IEmailService _emailService; // Add email service
        private readonly IMemoryCache _cache;


        public UsersController(MyDbContext db, IEmailService emailService, IMemoryCache cache)
        {
            _db = db;
            _emailService = emailService; // Inject email service
            _cache = cache;
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _db.Users.Find(id);
            return Ok(user);
        }

        [HttpGet("posts/userId/{id}")]
        public IActionResult grtPost(int id)
        {
            var userPosts = _db.Posts.Where(s => s.UserId == id ).ToList();
            return Ok(userPosts);
        }

        [HttpPut("editProfile/{id}")]
        public IActionResult PutUser(int id, [FromForm] EditProfileDto editDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Handle file upload if a new profile image is provided
            if (editDto.ProfileImageUrl != null)
            {
                var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }

                var filePath = Path.Combine(uploadsFolderPath, editDto.ProfileImageUrl.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    editDto.ProfileImageUrl.CopyTo(stream); // Synchronous file copy
                }
            }

            // Retrieve the user from the database
            var user = _db.Users.Find(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Update the user's details
            user.FullName = editDto.FullName ?? user.FullName;
            user.PhoneNumber = editDto.PhoneNumber ?? user.PhoneNumber;
            user.Location = editDto.Location ?? user.Location;

            // Update the profile image URL if a new image was uploaded
            if (editDto.ProfileImageUrl != null)
            {
                user.ProfileImageUrl = editDto.ProfileImageUrl.FileName; // Save only the file name in the database
            }

            // Save changes to the database
            _db.Users.Update(user);
            _db.SaveChanges();

            return Ok(new { message = "Profile updated successfully" });
        }


        // API for adding Posts to the database from body 
        [HttpPost("addPost")]
        public IActionResult AddPost([FromForm] AddPostDto postDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Handle file upload if a new profile image is provided
            if (postDto.ImageUrl != null)
            {
                var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }

                var filePath = Path.Combine(uploadsFolderPath, postDto.ImageUrl.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    postDto.ImageUrl.CopyTo(stream); // Synchronous file copy
                }

                var post = new Post
                {
                    UserId = postDto.UserId,
                    Content = postDto.Content,
                    ImageUrl = postDto.ImageUrl.FileName, // Save only the file name in the database
                };
                // Save the post to the database
                _db.Posts.Add(post);
                _db.SaveChanges();

            }
            return Ok(new { message = "Post added successfully" });

        }

        [HttpPut("updateRole/{id}")]
        public IActionResult UpdateUserRole(int id)
        {
            // البحث عن المستخدم باستخدام UserID
            var user = _db.Users.Find(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // تحديث الدور إلى 'Manager'
            user.Role = "Manager";

            // حفظ التغييرات في قاعدة البيانات
            _db.SaveChanges();

            return Ok(new { message = "User role updated to Manager successfully!" });
        }


        [HttpPost("register")]
        public IActionResult Register([FromForm] RegisterDto model) // Using FromForm to map form data
        {
            if (ModelState.IsValid)
            {
                // Check if the email already exists
                var existingUser = _db.Users.FirstOrDefault(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    return BadRequest(new { success = false, message = "Email already exists" });
                }

                // Generate password hash and salt
                byte[] passwordHash, passwordSalt;
                PasswordHasherNew.createPasswordHash(model.Password, out passwordHash, out passwordSalt);

                // Create user with hashed password and salt
                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Password = model.Password,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    CreatedDate = DateTime.UtcNow
                };

                // Generate OTP
                var otp = GenerateOtp();

                // Store OTP in cache for 5 minutes
                _cache.Set(model.Email, otp, TimeSpan.FromMinutes(5));

                // Send OTP email synchronously
                _emailService.SendOtpEmail(user.Email, otp); // Synchronous OTP email sending

                // Save the user in the database
                _db.Users.Add(user);
                _db.SaveChanges(); // Synchronous save

                return Ok(new { success = true, message = "User registered, OTP sent" });
            }

            return BadRequest(new { success = false, message = "Invalid data" });
        }

        [HttpPost("confirm-otp")]
        public IActionResult ConfirmOtp([FromBody] OtpConfirmationDto model)
        {
            if (_cache.TryGetValue(model.Email, out string cachedOtp))
            {
                if (cachedOtp == model.Otp)
                {
                    return Ok(new { success = true, message = "OTP confirmed" });
                }
                return BadRequest(new { success = false, message = "Invalid OTP" });
            }

            return BadRequest(new { success = false, message = "OTP expired or not found" });
        }

        // Method to generate a random OTP
        private string GenerateOtp()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString(); // Generates a 6-digit OTP
        }



        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest login)
        {
            // التحقق من بيانات الاعتماد (اسم المستخدم وكلمة المرور)
            var user = _db.Users
                .FirstOrDefault(u => u.UserName == login.UserName && u.Password == login.Password);

            if (user == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            // إعادة الدور
            var userRole = user.Role;  // يمكن أن يكون User أو Manager

            return Ok(new { Role = userRole, UserId = user.UserId });
        }
    }


    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    } 
}