using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Motostation.Models;

namespace Motostation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactMessagesController : ControllerBase
    {
        private readonly MyDbContext _context;

        public ContactMessagesController(MyDbContext context)
        {
            _context = context;
        }
        // GET: api/ContactMessages
        [HttpGet]
        public IActionResult GetContactMessages()
        {
            var messages = _context.ContactMessages.ToList();
            return Ok(messages);
        }

        // GET: api/ContactMessages/5
        [HttpGet("{id}")]
        public IActionResult GetContactMessage(int id)
        {
            var message = _context.ContactMessages.Find(id);
            if (message == null)
            {
                return NotFound();
            }
            return Ok(message);
        }

        // PUT api/ContactMessages/approve/{id}
        [HttpPut("approve/{id}")]
        public IActionResult ApproveMessage(int id)
        {
            var message = _context.ContactMessages.Find(id);
            if (message == null)
            {
                return NotFound();
            }

            // Update the IsApproved status
            if (message.IsApproved == false)
            {
                message.IsApproved = true;
            }
            else
            {
                message.IsApproved = false;
            }

            _context.SaveChanges(); // Save changes synchronously
            return NoContent(); // 204 No Content
            
        }

        // DELETE: api/ContactMessages/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteContactMessage(int id)
        {
            var message = _context.ContactMessages.Find(id);
            if (message == null)
            {
                return NotFound();
            }

            _context.ContactMessages.Remove(message);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
