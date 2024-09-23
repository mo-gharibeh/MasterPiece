using System;
using System.Collections.Generic;

namespace Motostation.Models;

public partial class Subscription
{
    public int SubscriptionId { get; set; }

    public int? UserId { get; set; }

    public string SubscriptionType { get; set; } = null!;

    public decimal Price { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsActive { get; set; }

    public virtual User? User { get; set; }
}
