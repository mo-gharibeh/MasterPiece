using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    }
}
