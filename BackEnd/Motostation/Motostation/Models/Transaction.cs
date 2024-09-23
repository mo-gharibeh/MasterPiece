using System;
using System.Collections.Generic;

namespace Motostation.Models;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int SellerId { get; set; }

    public int BuyerId { get; set; }

    public int? ProductId { get; set; }

    public string TransactionType { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime? TransactionDate { get; set; }

    public virtual User Buyer { get; set; } = null!;

    public virtual Product? Product { get; set; }

    public virtual User Seller { get; set; } = null!;
}
