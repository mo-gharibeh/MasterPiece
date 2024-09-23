using System;
using System.Collections.Generic;

namespace Motostation.Models;

public partial class Follower
{
    public int FollowerId { get; set; }

    public int? UserId { get; set; }

    public int? FollowedUserId { get; set; }

    public virtual User? FollowedUser { get; set; }

    public virtual User? User { get; set; }
}
