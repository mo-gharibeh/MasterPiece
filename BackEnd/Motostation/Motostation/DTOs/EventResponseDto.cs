namespace Motostation.DTOs
{
    public class EventResponseDto
    {
        public int EventId { get; set; }

        public int? OrganizerId { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Location { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Capacity { get; set; }

        public decimal RegistrationFee { get; set; }

        public bool IsPaid { get; set; }

        public string? EventType { get; set; }

        public string? Status { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastUpdated { get; set; }

        public IFormFile? CoverImageURL { get; set; }

        public string? StartLocation { get; set; }

        public string? EndLocation { get; set; }

        public string? FreeActivities { get; set; } // Store JSON string for free activities

        public string? RestStops { get; set; } // Store JSON string for rest stops
    }
}
