using System;
using System.Collections.Generic;

namespace Motostation.Models;

public partial class StoreRating
{
    public int RatingId { get; set; }

    public int? StoreId { get; set; }

    public int? UserId { get; set; }

    public int RatingValue { get; set; }

    public string? ReviewText { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Store? Store { get; set; }

    public virtual User? User { get; set; }
}
