using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Motostation.DTOs;
using Motostation.Models;
using SendGrid.Helpers.Mail;

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

        [HttpGet("free")]
        public IActionResult GetAllFreeEvents()
        {
            // Retrieve all events where IsPaid is false
            var freeEvents = _db.Events
                .Where(e => e.IsPaid == false)
                .ToList();

            return Ok(freeEvents);
        }

        [HttpGet("paid")]
        public IActionResult GetAllPaidEvents()
        {
            // Retrieve all events where IsPaid is true
            var paidEvents = _db.Events
                .Where(e => e.IsPaid == true)
                .ToList();

            return Ok(paidEvents);
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

            string? coverImagePath = null;

            // Handle file upload if provided
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
                    eventDto.CoverImageURL.CopyTo(stream);
                }
                coverImagePath = eventDto.CoverImageURL.FileName;
            }

            var newEvent = new Event
            {
                OrganizerId = eventDto.OrganizerId,
                Title = eventDto.Title,
                Description = eventDto.Description,
                Location = eventDto.Location,
                StartDate = eventDto.StartDate,
                EndDate = eventDto.EndDate,
                Capacity = eventDto.Capacity,
                RegistrationFee = eventDto.RegistrationFee,
                IsPaid = eventDto.RegistrationFee > 0, // Set IsPaid based on RegistrationFee
                EventType = eventDto.EventType,
                CoverImageUrl = coverImagePath,
                Status = eventDto.Status ?? "Upcoming",
                CreatedDate = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow,
                StartLocation = eventDto.StartLocation,
                EndLocation = eventDto.EndLocation,
                FreeActivities = eventDto.FreeActivities,
                RestStops = eventDto.RestStops
            };

            _db.Events.Add(newEvent);
            _db.SaveChanges();

            return Ok(new { message = "Event added successfully" });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateEvent(int id, [FromForm] EventResponseDto eventDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var eventToUpdate = _db.Events.Find(id);
            if (eventToUpdate == null)
            {
                return NotFound(new { message = "Event not found" });
            }

            // Handle file upload if provided
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
                    eventDto.CoverImageURL.CopyTo(stream);
                }
                eventToUpdate.CoverImageUrl = eventDto.CoverImageURL.FileName;
            }

            // Update properties
            eventToUpdate.Title = eventDto.Title;
            eventToUpdate.Description = eventDto.Description;
            eventToUpdate.Location = eventDto.Location;
            eventToUpdate.StartDate = eventDto.StartDate;
            eventToUpdate.EndDate = eventDto.EndDate;
            eventToUpdate.Capacity = eventDto.Capacity;
            eventToUpdate.RegistrationFee = eventDto.RegistrationFee;
            eventToUpdate.IsPaid = eventDto.RegistrationFee > 0;
            eventToUpdate.EventType = eventDto.EventType;
            eventToUpdate.LastUpdated = DateTime.UtcNow;
            eventToUpdate.StartLocation = eventDto.StartLocation;
            eventToUpdate.EndLocation = eventDto.EndLocation;
            eventToUpdate.FreeActivities = eventDto.FreeActivities;
            eventToUpdate.RestStops = eventDto.RestStops;

            _db.Events.Update(eventToUpdate);
            _db.SaveChanges();

            return Ok(new { message = "Event updated successfully" });
        }


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

        // GET: api/Events/upcoming
        [HttpGet("upcoming")]
        public async Task<IActionResult> GetUpcomingEvents()
        {
            var upcomingEvents = await _db.Events
                .Where(e => e.StartDate >= DateTime.Now) 
                .OrderBy(e => e.StartDate)  
                .Take(2)  
                .Select(e => new
                {
                    e.EventId,
                    e.Title,
                    e.Description,
                    e.StartDate,
                    e.Location,
                    e.CoverImageUrl  
                })
                .ToListAsync();

            return Ok(upcomingEvents);
        }

    }
}

