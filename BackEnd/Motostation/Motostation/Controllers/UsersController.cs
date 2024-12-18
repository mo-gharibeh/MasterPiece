﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Motostation.DataService;
using Motostation.DTOs;
using Motostation.Models;
using System.Net.Mail;
using System.Net;

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

        // GET: api/Users
        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _db.Users.ToList();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _db.Users.Find(id);
            return Ok(user);
        }

        [HttpGet("AllPost")]
        public IActionResult GetAllPosts()
        {
            var posts = _db.Posts.ToList();
            return Ok(posts);
        }

        [HttpDelete("DeletePost")]
        public IActionResult DeletePost(int id)
        {
            var postToRemove = _db.Posts.Find(id);
            if (postToRemove == null)
            {
                return NotFound(new { message = "Post not found" });
            }
            _db.Posts.Remove(postToRemove);
            _db.SaveChanges();
            return Ok(new { message = "Post deleted successfully" });   

        }

        [HttpGet("getPost/{postId}")]
        public IActionResult GetPost(int postId)
        {
            var post = _db.Posts.Find(postId);
            return Ok(post);
        }

            [HttpGet("posts/userId/{id}")]
        public IActionResult grtPost(int id)
        {
            var userPosts = _db.Posts.Where(s => s.UserId == id ).ToList();
            return Ok(userPosts);
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



        // Edit Post 
        [HttpPut("editPost/{id}")]
        public IActionResult PutPost(int id, [FromForm] EditPostDto editDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var post = _db.Posts.FirstOrDefault(p => p.PostId == id);
            if (post == null)
            {
                return NotFound(new { message = "Post not found" });
            }

            // Define the uploads folder path
            var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }

            // Handle file upload only if a new image was uploaded
            if (editDto.ImageUrl != null)
            {
                var filePath = Path.Combine(uploadsFolderPath, editDto.ImageUrl.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    editDto.ImageUrl.CopyTo(stream); // Synchronous file copy
                }

                // Update the post's image URL to the new file name
                post.ImageUrl = editDto.ImageUrl.FileName;
            }

            // Update the post's content
            post.Content = editDto.Content ?? post.Content;

            _db.Posts.Update(post);
            _db.SaveChanges();

            return Ok(new
            {
                message = "Post edited successfully"
            });
        }




        [HttpPut("editProfile/{id}")]
        public IActionResult PutUser(int id, [FromForm] EditProfileDto editDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Define the uploads folder path for storing the images
            var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }

            // Handle Profile Image upload
            if (editDto.ProfileImageUrl != null)
            {
                var profileImagePath = Path.Combine(uploadsFolderPath, editDto.ProfileImageUrl.FileName);
                using (var stream = new FileStream(profileImagePath, FileMode.Create))
                {
                    editDto.ProfileImageUrl.CopyTo(stream); // Synchronous file copy
                }
            }

            // Handle Cover Image upload
            if (editDto.CoverImageUrl != null)
            {
                var coverImagePath = Path.Combine(uploadsFolderPath, editDto.CoverImageUrl.FileName);
                using (var stream = new FileStream(coverImagePath, FileMode.Create))
                {
                    editDto.CoverImageUrl.CopyTo(stream); // Synchronous file copy
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

            // Update the cover image URL if a new image was uploaded
            if (editDto.CoverImageUrl != null)
            {
                user.CoverImageUrl = editDto.CoverImageUrl.FileName; // Save only the file name in the database
            }

            // Save changes to the database
            _db.Users.Update(user);
            _db.SaveChanges();

            return Ok(new { message = "Profile updated successfully" });
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

            return Ok(new { success = false, message = "User role updated to Manager successfully!" });
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterDto model) // Using FromForm to map form data
        {
            if (ModelState.IsValid)
            {
                // Check if the email or userName already exists 
                var existingUser = _db.Users.FirstOrDefault(u => u.Email == model.Email || u.UserName == model.UserName);
                if (existingUser != null)
                {
                    return BadRequest(new { success = false, message = "Email or UserName already exists" });
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

                // _emailService.SendOtpEmail(user.Email, otp); // Synchronous OTP email sending

                // Send OTP email asynchronously
                //await SendOtpEmailAsync(model.Email, otp);


                // Save the user in the database
                _db.Users.Add(user);
                _db.SaveChanges(); // Synchronous save

                return Ok(new { success = true, message = "User registered, OTP sent" });
            }

            return BadRequest(new { success = false, message = "Invalid data" });
        }

        //private async Task SendOtpEmailAsync(string email, string otp)
        //{
        //    var smtpClient = new SmtpClient("smtp.gmail.com") // ضبط مزود SMTP
        //    {
        //        Port = 465,
        //        Credentials = new NetworkCredential("motostation7@gmail.com", "nxiiebaqmnvfdtrq"),
        //        EnableSsl = true,
        //    };

        //    var mailMessage = new MailMessage
        //    {
        //        From = new MailAddress("motostation7@gmail.com"),
        //        Subject = "OTP for Registration",
        //        Body = $"Your OTP is: {otp}",
        //        IsBodyHtml = false,
        //    };
        //    mailMessage.To.Add(email);

        //    await smtpClient.SendMailAsync(mailMessage);
        //}


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

        [HttpPut("changePassword/{id}")]
        public IActionResult ChangePassword(int id, [FromBody] ChangePasswordDto passwordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Find user by id
            var user = _db.Users.Find(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Verify current password
            if (!PasswordHasherNew.VerifyPasswordHash(passwordDto.CurrentPassword, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest(new { message = "Current password is incorrect." });
            }

            // Verify that new password and confirm password match
            if (passwordDto.NewPassword != passwordDto.ConfirmPassword)
            {
                return BadRequest(new { message = "New password and confirmation password do not match." });
            }

            // Generate new password hash and salt
            byte[] passwordHash, passwordSalt;
            PasswordHasherNew.createPasswordHash(passwordDto.NewPassword, out passwordHash, out passwordSalt);

            // Update the user's password hash and salt
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            // Save changes to the database
            _db.Users.Update(user);
            _db.SaveChanges();

            return Ok(new { message = "Password changed successfully." });
        }

    }


    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    } 
}