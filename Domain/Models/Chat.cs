using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class Chat
{
    public Guid Id { get; set; }

    public Guid? CommunityId { get; set; }

    public string? Name { get; set; }

    public string? Type { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<ChatParticipant> ChatParticipants { get; set; } = new List<ChatParticipant>();

    public virtual Community? Community { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
