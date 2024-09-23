using System;
using System.Collections.Generic;

namespace Motostation.Models;

public partial class Favorite
{
    public int FavoriteId { get; set; }

    public int? UserId { get; set; }

    public int? ProductId { get; set; }

    public int? StoreId { get; set; }

    public int? EventId { get; set; }

    public virtual Event? Event { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Store? Store { get; set; }

    public virtual User? User { get; set; }
}
