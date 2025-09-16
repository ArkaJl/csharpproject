using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Post
{
    public Guid Id { get; set; }

    public Guid? AuthorId { get; set; }

    public Guid? CommunityId { get; set; }

    public string Content { get; set; } = null!;

    public string? Images { get; set; }

    public int? LikesCount { get; set; }

    public int? CommentsCount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Visibility { get; set; }

    public virtual User? Author { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Community? Community { get; set; }
}
