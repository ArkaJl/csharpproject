using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class CommunityOverview
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public int? MemberCount { get; set; }

    public long PostsCount { get; set; }

    public DateTime? LastActivity { get; set; }
}
