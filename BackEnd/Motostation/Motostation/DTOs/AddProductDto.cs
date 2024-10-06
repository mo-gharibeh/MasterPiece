namespace Motostation.DTOs
{
    public class AddProductDto
    {

        public int CategoryId { get; set; }

        public string ProductName { get; set; } = null!;

        public string? Description { get; set; }

        public int SellerId { get; set; }

        public string ProductType { get; set; } = null!; // sale // rent

        public decimal? Price { get; set; }

        //public decimal? RentalPrice { get; set; }

        public int? RentalDuration { get; set; }

        public bool? IsCurrentlyRented { get; set; }

        public int StockQuantity { get; set; }

        public IFormFile? ImageUrl { get; set; }

        public string? Brand { get; set; }

        public string ProductCondition { get; set; } = null!;  //New //Used




    }
}
