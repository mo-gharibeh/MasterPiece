namespace Motostation.DTOs
{
    public class EventResponseDto
    {
        public int? UserId { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Location { get; set; } = null!;

        public DateTime EventDate { get; set; }


    }
}
