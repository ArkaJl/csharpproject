using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string? AvatarUrl { get; set; }
    public int? Coins { get; set; }
    public string? Status { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? LastOnline { get; set; }
    public string? ThemePreference { get; set; }

    // Добавленные поля для аутентификации
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public string? EmailConfirmationToken { get; set; }
    public bool IsEmailConfirmed { get; set; } = false;
    public string? ResetPasswordToken { get; set; }
    public DateTime? ResetPasswordTokenExpiry { get; set; }

    // Навигационные свойства
    public virtual ICollection<ChatParticipant> ChatParticipants { get; set; } = new List<ChatParticipant>();
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual ICollection<Community> Communities { get; set; } = new List<Community>();
    public virtual ICollection<Medium> Media { get; set; } = new List<Medium>();
    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public virtual ICollection<UserInventory> UserInventories { get; set; } = new List<UserInventory>();
}