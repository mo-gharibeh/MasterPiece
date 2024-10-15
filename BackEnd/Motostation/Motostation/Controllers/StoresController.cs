using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Motostation.DTOs;
using Motostation.Models;

namespace Motostation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly MyDbContext _context;

        public StoresController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Stores
        [HttpGet]
        public IActionResult GetStores()
        {
            var stors = _context.Stores.ToList();
            return Ok(stors);
        }

        // GET: api/Stores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Store>> GetStore(int id)
        {
            var store = await _context.Stores.FindAsync(id);

            if (store == null)
            {
                return NotFound();
            }

            return store;
        }

        // PUT: api/Stores/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStore(int id, Store store)
        {
            if (id != store.StoreId)
            {
                return BadRequest();
            }

            _context.Entry(store).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Stores
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // Add
        [HttpPost]
        public IActionResult PostStore([FromForm] StoreResponseDto storeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (storeDto.StoreImageUrl != null)
            {
                var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }
                var filePath = Path.Combine(uploadsFolderPath, storeDto.StoreImageUrl.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    storeDto.StoreImageUrl.CopyToAsync(stream);
                }
                var x = new Store
                {
                    StoreName = storeDto.StoreName,
                    Address = storeDto.Address,
                    Phone = storeDto.Phone,
                    Email   = storeDto.Email,
                    WorkingHours = storeDto.WorkingHours,
                    StoreImageUrl = storeDto.StoreImageUrl.FileName,
                    Location = storeDto.Location,
                    ManagerId = storeDto.ManagerId,
                    
                };
                _context.Add(x);
                _context.SaveChanges();                
                return Ok();

            }
            return Ok();

        

         }

        // DELETE: api/Stores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStore(int id)
        {
            var store = await _context.Stores.FindAsync(id);
            if (store == null)
            {
                return NotFound();
            }

            _context.Stores.Remove(store);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StoreExists(int id)
        {
            return _context.Stores.Any(e => e.StoreId == id);
        }
    }
}
