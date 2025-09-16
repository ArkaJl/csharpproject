using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class ChatParticipant
{
    public Guid ChatId { get; set; }

    public Guid UserId { get; set; }

    public DateTime? JoinedAt { get; set; }

    public DateTime? LastRead { get; set; }

    public virtual Chat Chat { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
