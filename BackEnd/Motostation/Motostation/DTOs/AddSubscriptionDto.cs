namespace Motostation.DTOs
{
    public class AddSubscriptionDto
    {
        public int UserId { get; set; }

        public string SubscriptionType { get; set; } = null!;

        public decimal Price { get; set; }

    }
}
