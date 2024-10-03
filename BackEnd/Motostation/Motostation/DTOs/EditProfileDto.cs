namespace Motostation.DTOs
{
    public class EditProfileDto
    {
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Location { get; set; }
        public IFormFile? ProfileImageUrl { get; set; }

    }
}
