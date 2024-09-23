using System;
using System.Collections.Generic;

namespace Motostation.Models;

public partial class ContactMessage
{
    public int MessageId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Subject { get; set; } = null!;

    public string Content { get; set; } = null!;

    public bool? IsApproved { get; set; }

    public DateTime? CreatedDate { get; set; }
}
