using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Motostation.Models;

namespace Motostation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactMessage : ControllerBase
    {
        private readonly MyDbContext _db;

        public ContactMessage(MyDbContext db)
        { 
            _db = db; 
        }

        // GET: api/Contact
        [HttpGet]
        public IActionResult GetContact()
        {
            var contact = _db.ContactMessages.Where(c => c.IsApproved == true).ToList();
            return Ok(contact);
        }

    }
}
