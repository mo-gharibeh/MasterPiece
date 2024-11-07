using System;
using System.Collections.Generic;

namespace Motostation.Models;

public partial class Store
{
    public int StoreId { get; set; }

    public string StoreName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? Email { get; set; }

    public string? WorkingHours { get; set; }

    public string? StoreImageUrl { get; set; }

    public string? Location { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int ManagerId { get; set; }

    public virtual User Manager { get; set; } = null!;

    public virtual ICollection<StoreRating> StoreRatings { get; set; } = new List<StoreRating>();
}
