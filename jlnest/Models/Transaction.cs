using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Transaction
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    public int Amount { get; set; }

    public string? Type { get; set; }

    public Guid? ItemId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual StoreItem? Item { get; set; }

    public virtual User? User { get; set; }
}
