using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class StoreItem
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Type { get; set; }

    public int Price { get; set; }

    public string? Thumbnail { get; set; }

    public string? Description { get; set; }

    public string? Category { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual ICollection<UserInventory> UserInventories { get; set; } = new List<UserInventory>();
}
