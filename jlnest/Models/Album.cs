using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Album
{
    public Guid Id { get; set; }

    public Guid? CommunityId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Community? Community { get; set; }

    public virtual ICollection<Medium> Media { get; set; } = new List<Medium>();
}
