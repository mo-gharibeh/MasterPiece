namespace Motostation.DTOs
{
    public class CategoryDto
    {
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        public IFormFile? ImageUrl { get; set; } // To handle image file uploads
        public bool IsActive { get; set; } = true;
        

    }
}
    