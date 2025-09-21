using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class Community
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? AvatarUrl { get; set; }

    public string? BannerUrl { get; set; }

    public Guid? CreatorId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Tags { get; set; }

    public int? MemberCount { get; set; }

    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    public virtual ICollection<Chat> Chats { get; set; } = new List<Chat>();

    public virtual User? Creator { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
