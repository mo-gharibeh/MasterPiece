using System;
using System.Collections.Generic;

namespace Motostation.Models;

public partial class Notification
{
    public int NotificationId { get; set; }

    public int? UserId { get; set; }

    public string NotificationText { get; set; } = null!;

    public bool? IsRead { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual User? User { get; set; }
}
