
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Models;

public partial class JlnestContext : DbContext
{
    public JlnestContext()
    {
    }

    public JlnestContext(DbContextOptions<JlnestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Album> Albums { get; set; }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<ChatParticipant> ChatParticipants { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Community> Communities { get; set; }

    public virtual DbSet<CommunityOverview> CommunityOverviews { get; set; }

    public virtual DbSet<Medium> Media { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<StoreItem> StoreItems { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserActivity> UserActivities { get; set; }

    public virtual DbSet<UserInventory> UserInventories { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("albums");

            entity.HasIndex(e => e.CommunityId, "community_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(UUID())")
                .HasColumnName("id");
            entity.Property(e => e.CommunityId).HasColumnName("community_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");

            entity.HasOne(d => d.Community).WithMany(p => p.Albums)
                .HasForeignKey(d => d.CommunityId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("albums_ibfk_1");
        });

        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("chats");

            entity.HasIndex(e => e.CommunityId, "community_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(UUID())")
                .HasColumnName("id");
            entity.Property(e => e.CommunityId).HasColumnName("community_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .HasDefaultValueSql("'private'")
                .HasColumnName("type");

            entity.HasOne(d => d.Community).WithMany(p => p.Chats)
                .HasForeignKey(d => d.CommunityId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("chats_ibfk_1");
        });

        modelBuilder.Entity<ChatParticipant>(entity =>
        {
            entity.HasKey(e => new { e.ChatId, e.UserId }).HasName("PRIMARY");

            entity.ToTable("chat_participants");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.ChatId).HasColumnName("chat_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.JoinedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("joined_at");
            entity.Property(e => e.LastRead)
                .HasColumnType("timestamp")
                .HasColumnName("last_read");

            entity.HasOne(d => d.Chat).WithMany(p => p.ChatParticipants)
                .HasForeignKey(d => d.ChatId)
                .HasConstraintName("chat_participants_ibfk_1");

            entity.HasOne(d => d.User).WithMany(p => p.ChatParticipants)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("chat_participants_ibfk_2");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("comments");

            entity.HasIndex(e => e.AuthorId, "author_id");

            entity.HasIndex(e => e.PostId, "idx_comments_post");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(UUID())")
                .HasColumnName("id");
            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.LikesCount)
                .HasDefaultValueSql("'0'")
                .HasColumnName("likes_count");
            entity.Property(e => e.PostId).HasColumnName("post_id");
            entity.Property(e => e.Text)
                .HasColumnType("text")
                .HasColumnName("text");

            entity.HasOne(d => d.Author).WithMany(p => p.Comments)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("comments_ibfk_2");

            entity.HasOne(d => d.Post).WithMany(p => p.Comments)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("comments_ibfk_1");
        });

        modelBuilder.Entity<Community>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("communities");

            entity.HasIndex(e => e.CreatorId, "creator_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(UUID())")
                .HasColumnName("id");
            entity.Property(e => e.AvatarUrl)
                .HasColumnType("text")
                .HasColumnName("avatar_url");
            entity.Property(e => e.BannerUrl)
                .HasColumnType("text")
                .HasColumnName("banner_url");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatorId).HasColumnName("creator_id");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.MemberCount)
                .HasDefaultValueSql("'0'")
                .HasColumnName("member_count");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Tags)
                .HasColumnType("json")
                .HasColumnName("tags");

            entity.HasOne(d => d.Creator).WithMany(p => p.Communities)
                .HasForeignKey(d => d.CreatorId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("communities_ibfk_1");
        });

        modelBuilder.Entity<CommunityOverview>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("community_overview");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(UUID())")
                .HasColumnName("id");
            entity.Property(e => e.LastActivity)
                .HasColumnType("timestamp")
                .HasColumnName("last_activity");
            entity.Property(e => e.MemberCount)
                .HasDefaultValueSql("'0'")
                .HasColumnName("member_count");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.PostsCount).HasColumnName("posts_count");
        });

        modelBuilder.Entity<Medium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("media");

            entity.HasIndex(e => e.AlbumId, "album_id");

            entity.HasIndex(e => e.UploadedBy, "uploaded_by");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(UUID())")
                .HasColumnName("id");
            entity.Property(e => e.AlbumId).HasColumnName("album_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.Type)
                .HasMaxLength(10)
                .HasColumnName("type");
            entity.Property(e => e.UploadedBy).HasColumnName("uploaded_by");
            entity.Property(e => e.Url)
                .HasColumnType("text")
                .HasColumnName("url");

            entity.HasOne(d => d.Album).WithMany(p => p.Media)
                .HasForeignKey(d => d.AlbumId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("media_ibfk_1");

            entity.HasOne(d => d.UploadedByNavigation).WithMany(p => p.Media)
                .HasForeignKey(d => d.UploadedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("media_ibfk_2");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("messages");

            entity.HasIndex(e => new { e.ChatId, e.CreatedAt }, "idx_messages_chat");

            entity.HasIndex(e => e.SenderId, "sender_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(UUID())")
                .HasColumnName("id");
            entity.Property(e => e.ChatId).HasColumnName("chat_id");
            entity.Property(e => e.Content)
                .HasColumnType("text")
                .HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.ReadStatus)
                .HasDefaultValueSql("'0'")
                .HasColumnName("read_status");
            entity.Property(e => e.SenderId).HasColumnName("sender_id");

            entity.HasOne(d => d.Chat).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ChatId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("messages_ibfk_1");

            entity.HasOne(d => d.Sender).WithMany(p => p.Messages)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("messages_ibfk_2");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("notifications");

            entity.HasIndex(e => new { e.UserId, e.IsRead, e.CreatedAt }, "idx_notifications_user");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(UUID())")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.IsRead)
                .HasDefaultValueSql("'0'")
                .HasColumnName("is_read");
            entity.Property(e => e.SourceId).HasColumnName("source_id");
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .HasColumnName("type");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("notifications_ibfk_1");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("posts");

            entity.HasIndex(e => e.AuthorId, "author_id");

            entity.HasIndex(e => new { e.CommunityId, e.CreatedAt }, "idx_posts_community");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(UUID())")
                .HasColumnName("id");
            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.CommentsCount)
                .HasDefaultValueSql("'0'")
                .HasColumnName("comments_count");
            entity.Property(e => e.CommunityId).HasColumnName("community_id");
            entity.Property(e => e.Content)
                .HasColumnType("text")
                .HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.Images)
                .HasColumnType("json")
                .HasColumnName("images");
            entity.Property(e => e.LikesCount)
                .HasDefaultValueSql("'0'")
                .HasColumnName("likes_count");
            entity.Property(e => e.Visibility)
                .HasMaxLength(20)
                .HasDefaultValueSql("'public'")
                .HasColumnName("visibility");

            entity.HasOne(d => d.Author).WithMany(p => p.Posts)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("posts_ibfk_1");

            entity.HasOne(d => d.Community).WithMany(p => p.Posts)
                .HasForeignKey(d => d.CommunityId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("posts_ibfk_2");
        });

        modelBuilder.Entity<StoreItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("store_items");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(UUID())")
                .HasColumnName("id");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .HasColumnName("category");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Thumbnail)
                .HasColumnType("text")
                .HasColumnName("thumbnail");
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .HasColumnName("type");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.CommunityId }).HasName("PRIMARY");

            entity.ToTable("subscriptions");

            entity.HasIndex(e => e.CommunityId, "community_id");

            entity.HasIndex(e => e.UserId, "idx_subscriptions_user");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.CommunityId).HasColumnName("community_id");
            entity.Property(e => e.JoinedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("joined_at");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasDefaultValueSql("'member'")
                .HasColumnName("role");

            entity.HasOne(d => d.Community).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.CommunityId)
                .HasConstraintName("subscriptions_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("subscriptions_ibfk_1");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("transactions");

            entity.HasIndex(e => e.ItemId, "item_id");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(UUID())")
                .HasColumnName("id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .HasColumnName("type");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Item).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("transactions_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("transactions_ibfk_1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "email").IsUnique();

            entity.HasIndex(e => e.Username, "username").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(UUID())")
                .HasColumnName("id");
            entity.Property(e => e.AvatarUrl)
                .HasColumnType("text")
                .HasColumnName("avatar_url");
            entity.Property(e => e.Coins)
                .HasDefaultValueSql("'0'")
                .HasColumnName("coins");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.LastOnline)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("last_online");
            entity.Property(e => e.PasswordHash)
                .HasColumnType("text")
                .HasColumnName("password_hash");
            entity.Property(e => e.Status)
                .HasColumnType("text")
                .HasColumnName("status");
            entity.Property(e => e.ThemePreference)
                .HasMaxLength(20)
                .HasDefaultValueSql("'light'")
                .HasColumnName("theme_preference");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        modelBuilder.Entity<UserActivity>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("user_activity");

            entity.Property(e => e.CommentsCount).HasColumnName("comments_count");
            entity.Property(e => e.CommunitiesCount).HasColumnName("communities_count");
            entity.Property(e => e.Id)
                .HasDefaultValueSql("(UUID())")
                .HasColumnName("id");
            entity.Property(e => e.LastOnline)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("last_online");
            entity.Property(e => e.PostsCount).HasColumnName("posts_count");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        modelBuilder.Entity<UserInventory>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.ItemId }).HasName("PRIMARY");

            entity.ToTable("user_inventory");

            entity.HasIndex(e => e.UserId, "idx_user_inventory");

            entity.HasIndex(e => e.ItemId, "item_id");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.IsEquipped)
                .HasDefaultValueSql("'0'")
                .HasColumnName("is_equipped");
            entity.Property(e => e.PurchasedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("purchased_at");

            entity.HasOne(d => d.Item).WithMany(p => p.UserInventories)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("user_inventory_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.UserInventories)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("user_inventory_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
