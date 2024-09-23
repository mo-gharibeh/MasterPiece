using System;
using System.Collections.Generic;

namespace Motostation.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public int? PostId { get; set; }

    public int? UserId { get; set; }

    public string CommentText { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public virtual Post? Post { get; set; }

    public virtual User? User { get; set; }
}
