using System;
using System.Collections.Generic;

namespace Motostation.Models;

public partial class Like
{
    public long Id { get; set; }

    public int? PostId { get; set; }

    public int? UserId { get; set; }

    public bool? Flag { get; set; }

    public virtual Post? Post { get; set; }

    public virtual User? User { get; set; }
}
