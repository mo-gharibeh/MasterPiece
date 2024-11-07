using System;
using System.Collections.Generic;

namespace Motostation.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public int CategoryId { get; set; }

    public string ProductName { get; set; } = null!;

    public string? Description { get; set; }

    public int SellerId { get; set; }

    public string ProductType { get; set; } = null!;

    public decimal? Price { get; set; }

    public decimal? RentalPrice { get; set; }

    public int? RentalDuration { get; set; }

    public bool? IsCurrentlyRented { get; set; }

    public int StockQuantity { get; set; }

    public string? ImageUrl { get; set; }

    public string? Brand { get; set; }

    public string ProductCondition { get; set; } = null!;

    public bool? IsSold { get; set; }

    public bool? IsAvailable { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual User Seller { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
