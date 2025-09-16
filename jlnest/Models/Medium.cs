using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Medium
{
    public Guid Id { get; set; }

    public Guid? AlbumId { get; set; }

    public string Url { get; set; } = null!;

    public string? Type { get; set; }

    public Guid? UploadedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Album? Album { get; set; }

    public virtual User? UploadedByNavigation { get; set; }
}
