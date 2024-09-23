using System;
using System.Collections.Generic;

namespace Motostation.Models;

public partial class Route
{
    public int RouteId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? StartLocation { get; set; }

    public string? EndLocation { get; set; }

    public string? RestStops { get; set; }

    public DateTime? CreatedDate { get; set; }
}
