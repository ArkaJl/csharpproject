using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class Message
{
    public Guid Id { get; set; }

    public Guid ChatId { get; set; }

    public Guid SenderId { get; set; }

    public string Content { get; set; } = null!;

    public bool ReadStatus { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Chat? Chat { get; set; }

    public virtual User? Sender { get; set; }
}
