using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Comment
{
    public Guid Id { get; set; }

    public Guid? PostId { get; set; }

    public Guid? AuthorId { get; set; }

    public string Text { get; set; } = null!;

    public int? LikesCount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User? Author { get; set; }

    public virtual Post? Post { get; set; }
}
