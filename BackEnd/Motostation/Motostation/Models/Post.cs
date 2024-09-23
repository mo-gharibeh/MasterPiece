using System;
using System.Collections.Generic;

namespace Motostation.Models;

public partial class Post
{
    public int PostId { get; set; }

    public int? UserId { get; set; }

    public string Content { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? LikesCount { get; set; }

    public int? CommentsCount { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

    public virtual User? User { get; set; }
}
