using System;
using System.Collections.Generic;

namespace Motostation.Models;

public partial class Event
{
    public int EventId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? Location { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int? OrganizerId { get; set; }

    public string? EventType { get; set; }

    public int? Capacity { get; set; }

    public decimal? RegistrationFee { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? LastUpdated { get; set; }

    public string? CoverImageUrl { get; set; }

    public string? Tags { get; set; }

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual User? Organizer { get; set; }
}
