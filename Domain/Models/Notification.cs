﻿using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class Notification
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    public string? Type { get; set; }

    public Guid? SourceId { get; set; }

    public bool IsRead { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User? User { get; set; }
}
