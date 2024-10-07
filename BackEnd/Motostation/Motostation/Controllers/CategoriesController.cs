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
    public class CategoriesController : ControllerBase
    {
        private readonly MyDbContext _context;

        public CategoriesController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public  IActionResult GetCategories()
        {
            var categories = _context.Categories.ToList();
            return Ok(categories);
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public IActionResult GetCategory(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound(new { message = "Category not found" });
            }
            return Ok(category);
        }

        [HttpPost("add")]
        public IActionResult CreateCategory([FromForm] CategoryDto categoryDto)
        {
            if (string.IsNullOrEmpty(categoryDto.CategoryName))
            {
                return BadRequest(new { message = "Category name is required" });
            }

            if (categoryDto.ImageUrl != null)
            {
                var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }
                string filePath = Path.Combine(uploadsFolderPath, categoryDto.ImageUrl.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    categoryDto.ImageUrl.CopyToAsync(stream);
                }
            }

            var category = new Category
            {
                CategoryName = categoryDto.CategoryName,
                Description = categoryDto.Description,
                ImageUrl =  categoryDto.ImageUrl.FileName,
                IsActive = categoryDto.IsActive
            };

            _context.Categories.Add(category);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryId }, category);
        }



        [HttpPut("edit/{id}")]
        public IActionResult UpdateCategory(int id, [FromForm] CategoryDto categoryDto)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound(new { message = "Category not found" });
            }

            if (string.IsNullOrEmpty(categoryDto.CategoryName))
            {
                return BadRequest(new { message = "Category name is required" });
            }

            if (categoryDto.ImageUrl != null)
            {
                var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }
                string filePath = Path.Combine(uploadsFolderPath, categoryDto.ImageUrl.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    categoryDto.ImageUrl.CopyToAsync(stream);
                }
            }

            // Update other fields
            category.CategoryName = categoryDto.CategoryName;
            category.Description = categoryDto.Description;
            category.IsActive = categoryDto.IsActive;
            category.ImageUrl = categoryDto.ImageUrl.FileName;


            _context.Categories.Update(category);
            _context.SaveChanges();

            return Ok(category);
        }


        


        [HttpDelete("delete/{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound(new { message = "Category not found" });
            }

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return NoContent();
        }




        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }
    }
}
