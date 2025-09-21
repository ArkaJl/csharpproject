using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class UserActivity
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public long PostsCount { get; set; }

    public long CommentsCount { get; set; }

    public long CommunitiesCount { get; set; }

    public DateTime? LastOnline { get; set; }
}
