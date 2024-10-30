namespace Motostation.DTOs
{
    public class EventResponseDto
    {
        public int? OrganizerId { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Location { get; set; } = null!;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Capacity { get; set; }
        public decimal RegistrationFee { get; set; }
        public string? EventType { get; set; }
        public IFormFile? CoverImageURL { get; set; }
        public string? Tags { get; set; }


    }
}
