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
    public class ProductsController : ControllerBase
    {
        private readonly MyDbContext _context;

        public ProductsController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet("allProducts")]
        public IActionResult GetProducts()
        {
            var products = _context.Products
                .Select(c => new
                {
                    c.ProductId,
                    c.ProductName,
                    c.ImageUrl,
                    c.ProductType,
                    c.Price,
                    userName = c.Seller.UserName,
                    categoryName = c.Category.CategoryName,
                })
                .ToList();
            return Ok(products);


        }

        // GET: api/Products/Category/5
        [HttpGet("category/{id}")]
        public IActionResult GetProductByCategoryId(int id) 
        {
            var products = _context.Products.Where(c => c.CategoryId == id)
                .Select(c => new
                {
                    c.ProductId,
                    c.ProductName,
                    c.ImageUrl,
                    c.ProductType,
                    c.Price,
                    categoryName = c.Category.CategoryName,
                })
                .ToList();
            return Ok(products);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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


        // POST: api/Products/
        [HttpPost]
        public IActionResult PostProduct([FromForm] AddProductDto productDto) 
        {
            if (productDto == null)
            {
                return BadRequest();
            }
            if (productDto.ImageUrl != null)
            {
                var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }
                var filePath = Path.Combine(uploadsFolderPath, productDto.ImageUrl.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    productDto.ImageUrl.CopyToAsync(stream);
                }
            }
            var product = new Product
            {
            CategoryId = productDto.CategoryId,
            ProductName = productDto.ProductName,
            Description = productDto.Description,
            SellerId = productDto.SellerId,
            ProductType = productDto.ProductType,
            //RentalPrice = productDto.RentalPrice,
            RentalDuration = productDto.RentalDuration,
            IsCurrentlyRented = productDto.IsCurrentlyRented,
            Price = productDto.Price,
            StockQuantity = productDto.StockQuantity,
            ImageUrl = productDto.ImageUrl.FileName,
            Brand = productDto.Brand,
            ProductCondition = productDto.ProductCondition,
            };
            _context.Products.Add(product);
            _context.SaveChanges();
            return Ok(product);
        }


        [HttpGet("filter")]
        public IActionResult FilterProducts(int? categoryId, string? productName)
        {
            var query = _context.Products.AsQueryable();

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            if (!string.IsNullOrEmpty(productName))
            {
                query = query.Where(p => p.ProductName.ToLower().Contains(productName.ToLower()));
            }

            var products = query
                .Select(p => new
                {
                    p.ProductId,
                    p.ProductName,
                    p.ImageUrl,
                    p.ProductType,
                    p.Price,
                    CategoryName = p.Category.CategoryName
                })
                .ToList();

            return Ok(products);
        }








        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Product>> PostProduct(Product product)
        //{
        //    _context.Products.Add(product);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        //}

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
