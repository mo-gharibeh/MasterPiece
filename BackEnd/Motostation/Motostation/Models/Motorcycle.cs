using System;
using System.Collections.Generic;

namespace Motostation.Models;

public partial class Motorcycle
{
    public int MotorcycleId { get; set; }

    public int? UserId { get; set; }

    public string MotorcycleType { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    public bool IsForSale { get; set; }

    public bool IsForRent { get; set; }

    public string Location { get; set; } = null!;

    public string? MotorcycleImageUrl { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual User? User { get; set; }
}
