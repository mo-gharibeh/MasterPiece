using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Motostation.DTOs;
using Motostation.Models;

namespace Motostation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly MyDbContext _db;

        public EventsController(MyDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetAllEvents()
        {
            var events = _db.Events.ToList();
            return Ok(events);
        }

        [HttpGet("{id}")]
        public IActionResult getEventById(int id)
        {
            var events = _db.Events.Find(id);
            return Ok(events);
        }

        [HttpPost]
        public IActionResult AddEvent([FromForm] EventResponseDto eventDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (eventDto.CoverImageURL != null)
            {
                var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }
                var filePath = Path.Combine(uploadsFolderPath, eventDto.CoverImageURL.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    eventDto.CoverImageURL.CopyToAsync(stream);
                }
            }

            try
            {
                // Step 2: Create an event entity from the DTO
                var newEvent = new Event
                {
                    OrganizerId = eventDto.OrganizerId, // Assuming this should be set
                    Title = eventDto.Title,
                    Description = eventDto.Description,
                    Location = eventDto.Location,
                    StartDate = eventDto.StartDate,
                    EndDate = eventDto.EndDate,
                    Capacity = eventDto.Capacity,
                    RegistrationFee = eventDto.RegistrationFee,
                    EventType = eventDto.EventType,
                    CoverImageUrl = eventDto.CoverImageURL.FileName,
                    Tags = eventDto.Tags,
                    CreatedDate = DateTime.UtcNow,
                    LastUpdated = DateTime.UtcNow,
                    Status = "Upcoming" // Default status
                };

                // Add the new event to the database
                _db.Events.Add(newEvent);
                _db.SaveChanges(); // Save changes synchronously

                return Ok(new { message = "Event added successfully" });
            }
            catch (Exception ex)
            {
                // Log the exception (you can implement a logger)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        //[HttpPut]
        //public IActionResult UpdateEvent(int id, [FromForm] EventResponseDto eventDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    var eventToUpdate = _db.Events.Find(id);
        //    if (eventToUpdate == null)
        //    {
        //        return NotFound(new { message = "Event not found" });
        //    }
        //    //eventToUpdate.UserId = eventDto.UserId;
        //    eventToUpdate.Title = eventDto.Title;
        //    eventToUpdate.Description = eventDto.Description;
        //    eventToUpdate.Location = eventDto.Location;
        //    eventToUpdate.EventDate = eventDto.EventDate;


        //    _db.Events.Update(eventToUpdate);
        //    _db.SaveChanges();
        //    return Ok(new { message = "Event updated successfully" });
        //}

        [HttpDelete]
        public IActionResult DeleteEvent(int id)
        {
            var eventToRemove = _db.Events.Find(id);
            if (eventToRemove == null)
            {
                return NotFound(new { message = "Event not found" });
            }
            _db.Events.Remove(eventToRemove);

            _db.SaveChanges();
            return Ok(new { message = "Event deleted successfully" });

        }
    }
}

