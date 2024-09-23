using System;
using System.Collections.Generic;

namespace Motostation.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public byte[]? PasswordHash { get; set; }

    public string? FullName { get; set; }

    public string? ProfileImageUrl { get; set; }

    public string? CoverImageUrl { get; set; }

    public string Role { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public DateTime? LastLoginDate { get; set; }

    public byte[]? PasswordSalt { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual ICollection<Follower> FollowerFollowedUsers { get; set; } = new List<Follower>();

    public virtual ICollection<Follower> FollowerUsers { get; set; } = new List<Follower>();

    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

    public virtual ICollection<Motorcycle> Motorcycles { get; set; } = new List<Motorcycle>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<StoreRating> StoreRatings { get; set; } = new List<StoreRating>();

    public virtual ICollection<Store> Stores { get; set; } = new List<Store>();

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();

    public virtual ICollection<Transaction> TransactionBuyers { get; set; } = new List<Transaction>();

    public virtual ICollection<Transaction> TransactionSellers { get; set; } = new List<Transaction>();
}
