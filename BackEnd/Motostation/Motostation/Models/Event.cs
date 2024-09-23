using System;
using System.Collections.Generic;

namespace Motostation.Models;

public partial class Event
{
    public int EventId { get; set; }

    public int? UserId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Location { get; set; } = null!;

    public DateTime EventDate { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual User? User { get; set; }
}
