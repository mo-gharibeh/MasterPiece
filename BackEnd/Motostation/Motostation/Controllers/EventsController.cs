﻿using Microsoft.AspNetCore.Http;
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
        public IActionResult AddEvent([FromBody] EventResponseDto eventDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                // Step 2: Create an event entity from the DTO
                var newEvent = new Event
                {
                    UserId = eventDto.UserId,
                    Title = eventDto.Title,
                    Description = eventDto.Description,
                    Location = eventDto.Location,
                    EventDate = eventDto.EventDate,
                };
                _db.Events.Add(newEvent);
                _db.SaveChanges();

                return Ok(new { message = "Event added successfully" });
            }
            catch (Exception ex)
            {
                // Log the exception (you can implement a logger)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
