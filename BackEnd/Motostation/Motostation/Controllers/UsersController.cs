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
        private readonly IEmailService _emailService; // Add email service
        private readonly IMemoryCache _cache;


        public UsersController(MyDbContext db, IEmailService emailService, IMemoryCache cache)
        {
            _db = db;
            _emailService = emailService; // Inject email service
            _cache = cache;
        }

        [HttpGet("id")]
        public IActionResult GetUserById(int id)
        {
            var user = _db.Users.Find(id);
            return Ok(user);
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


    }
}
