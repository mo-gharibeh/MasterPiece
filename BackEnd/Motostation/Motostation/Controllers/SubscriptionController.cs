using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Motostation.DTOs;
using Motostation.Models;

namespace Motostation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly MyDbContext _db;
        public SubscriptionController(MyDbContext db)
        {
            _db = db;
        }

        // GET: api/Payments
        [HttpGet("allPayment")]
        public IActionResult GetPayments()
        {
            var payments = _db.Payments
                .Select(t => new
                {
                    t.PaymentId,
                    UserName = t.User.UserName,
                    t.PaymentDate,
                    t.Amount,
                    t.PaymentStatus,
                    t.PaymentMethod,
                })
                .ToList();
            return Ok(payments);
        }

        // GET: api/Subscriptions
        [HttpGet("allSubscription")]
        public IActionResult GetSubscriptions()
        {
            var subscriptions = _db.Subscriptions
                .Select(s => new
                {
                    s.SubscriptionId,
                    UserName = s.User.UserName,
                    s.SubscriptionType,
                    s.Price,
                    s.StartDate,
                    s.EndDate,
                    s.IsActive,

                })
                .ToList();
            return Ok(subscriptions);
        }

        [HttpPost("addSubscription")]
        public IActionResult AddSubscription([FromBody] AddSubscriptionDto subscriptionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var subscription = new Subscription
            {
                UserId = subscriptionDto.UserId,
                SubscriptionType = subscriptionDto.SubscriptionType,
                Price = subscriptionDto.Price,
            };
            var type = subscription.SubscriptionType;
            var startDate = DateTime.Now;
            DateTime endDate = startDate;

            switch (type)
            {
                case "7 days":
                    endDate = startDate.AddDays(7);
                    break;
                case "30 days":
                    endDate = startDate.AddDays(30);
                    break;
                default:
                    return BadRequest("Invalid subscription type.");
            }

            subscription.StartDate = startDate;
            subscription.EndDate = endDate;
            subscription.IsActive = true;

            _db.Subscriptions.Add(subscription);
            _db.SaveChanges();
            return Ok(subscription);


        }

        // add Payment to payment list
        [HttpPost("addPayment")]
        public IActionResult AddPayment([FromBody] AddPaymentDto paymentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            var payment = new Payment
            {
                UserId = paymentDto.UserId,
                Amount = paymentDto.Amount,
                PaymentMethod = paymentDto.PaymentMethod,
                PaymentStatus = paymentDto.PaymentStatus,
                PaymentDate = DateTime.Now,
            };
            _db.Payments.Add(payment);
            _db.SaveChanges();
            return Ok(payment);

        }


        // API لحساب المدة المتبقية وتنبيه المستخدم
        [HttpGet("checkSubscriptionStatus/{userId}")]
        public IActionResult CheckSubscriptionStatus(int userId)
        {
            // البحث عن الاشتراك النشط للمستخدم
            var subscription = _db.Subscriptions
                                  .FirstOrDefault(s => s.UserId == userId && s.IsActive);

            if (subscription == null)
            {
                return NotFound("No active subscription found for this user.");
            }

            // حساب المدة المتبقية
            var remainingDays = (subscription.EndDate - DateTime.UtcNow).TotalDays;

            if (remainingDays <= 0)
            {
                // انتهاء الاشتراك
                subscription.IsActive = false;

                // تحديث دور المستخدم إلى "User"
                var user = _db.Users.Find(userId);
                if (user != null)
                {
                    user.Role = "User";
                }

                _db.SaveChanges();

                return Ok(new { message = "Subscription has ended ." });
            }
            else if (remainingDays <= 5)
            {
                // تنبيه قرب انتهاء الاشتراك
                return Ok(new { message = $"Your subscription will end in {Math.Ceiling(remainingDays)} days. Please renew soon." });
            }

            // الاشتراك ما زال نشطًا
            return Ok(new { message = $"Your subscription is active. {Math.Ceiling(remainingDays)} days remaining." });
        }

    }
}
