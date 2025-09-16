using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Subscription
{
    public Guid UserId { get; set; }

    public Guid CommunityId { get; set; }

    public string? Role { get; set; }

    public DateTime? JoinedAt { get; set; }

    public virtual Community Community { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
