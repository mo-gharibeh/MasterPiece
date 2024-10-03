namespace Motostation.DTOs
{
    public class AddPaymentDto
    {
        public int? UserId { get; set; }

        public decimal Amount { get; set; }

        public string PaymentMethod { get; set; } = null!;

        public string PaymentStatus { get; set; } = null!;
    }
}
