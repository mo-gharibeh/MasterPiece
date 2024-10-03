namespace Motostation.DTOs
{
    public class AddPostDto
    {
        public int UserId { get; set; }

        public string Content { get; set; } = null!;

        public IFormFile ImageUrl { get; set; }
    }
}
