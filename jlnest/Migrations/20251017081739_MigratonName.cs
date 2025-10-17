using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MigratonName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "store_items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    price = table.Column<int>(type: "int", nullable: false),
                    thumbnail = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    category = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    username = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "varchar(255)", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    avatar_url = table.Column<string>(type: "text", nullable: true),
                    coins = table.Column<int>(type: "int", nullable: true, defaultValueSql: "'0'"),
                    status = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    last_online = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    theme_preference = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true, defaultValueSql: "'light'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "communities",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    avatar_url = table.Column<string>(type: "text", nullable: true),
                    banner_url = table.Column<string>(type: "text", nullable: true),
                    creator_id = table.Column<Guid>(type: "char(36)", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    tags = table.Column<string>(type: "json", nullable: true),
                    member_count = table.Column<int>(type: "int", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "communities_ibfk_1",
                        column: x => x.creator_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    user_id = table.Column<Guid>(type: "char(36)", nullable: true),
                    type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    source_id = table.Column<Guid>(type: "char(36)", nullable: true),
                    is_read = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValueSql: "'0'"),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "notifications_ibfk_1",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    user_id = table.Column<Guid>(type: "char(36)", nullable: true),
                    amount = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    item_id = table.Column<Guid>(type: "char(36)", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "transactions_ibfk_1",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "transactions_ibfk_2",
                        column: x => x.item_id,
                        principalTable: "store_items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_inventory",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    item_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    purchased_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    is_equipped = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.user_id, x.item_id });
                    table.ForeignKey(
                        name: "user_inventory_ibfk_1",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "user_inventory_ibfk_2",
                        column: x => x.item_id,
                        principalTable: "store_items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "albums",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    community_id = table.Column<Guid>(type: "char(36)", nullable: true),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "albums_ibfk_1",
                        column: x => x.community_id,
                        principalTable: "communities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "chats",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    community_id = table.Column<Guid>(type: "char(36)", nullable: true),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true, defaultValueSql: "'private'"),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "chats_ibfk_1",
                        column: x => x.community_id,
                        principalTable: "communities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "posts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    author_id = table.Column<Guid>(type: "char(36)", nullable: true),
                    community_id = table.Column<Guid>(type: "char(36)", nullable: true),
                    content = table.Column<string>(type: "text", nullable: false),
                    images = table.Column<string>(type: "json", nullable: true),
                    likes_count = table.Column<int>(type: "int", nullable: false, defaultValueSql: "'0'"),
                    comments_count = table.Column<int>(type: "int", nullable: false, defaultValueSql: "'0'"),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    visibility = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true, defaultValueSql: "'public'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "posts_ibfk_1",
                        column: x => x.author_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "posts_ibfk_2",
                        column: x => x.community_id,
                        principalTable: "communities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "subscriptions",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    community_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    role = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true, defaultValueSql: "'member'"),
                    joined_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.user_id, x.community_id });
                    table.ForeignKey(
                        name: "subscriptions_ibfk_1",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "subscriptions_ibfk_2",
                        column: x => x.community_id,
                        principalTable: "communities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "media",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    album_id = table.Column<Guid>(type: "char(36)", nullable: true),
                    url = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                    uploaded_by = table.Column<Guid>(type: "char(36)", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "media_ibfk_1",
                        column: x => x.album_id,
                        principalTable: "albums",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "media_ibfk_2",
                        column: x => x.uploaded_by,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "chat_participants",
                columns: table => new
                {
                    chat_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    user_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    joined_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    last_read = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.chat_id, x.user_id });
                    table.ForeignKey(
                        name: "chat_participants_ibfk_1",
                        column: x => x.chat_id,
                        principalTable: "chats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "chat_participants_ibfk_2",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "messages",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    chat_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    sender_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    read_status = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValueSql: "'0'"),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "messages_ibfk_1",
                        column: x => x.chat_id,
                        principalTable: "chats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "messages_ibfk_2",
                        column: x => x.sender_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "comments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(UUID())"),
                    post_id = table.Column<Guid>(type: "char(36)", nullable: true),
                    author_id = table.Column<Guid>(type: "char(36)", nullable: true),
                    text = table.Column<string>(type: "text", nullable: false),
                    likes_count = table.Column<int>(type: "int", nullable: true, defaultValueSql: "'0'"),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "comments_ibfk_1",
                        column: x => x.post_id,
                        principalTable: "posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "comments_ibfk_2",
                        column: x => x.author_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "community_id",
                table: "albums",
                column: "community_id");

            migrationBuilder.CreateIndex(
                name: "user_id",
                table: "chat_participants",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "community_id1",
                table: "chats",
                column: "community_id");

            migrationBuilder.CreateIndex(
                name: "author_id",
                table: "comments",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "idx_comments_post",
                table: "comments",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "creator_id",
                table: "communities",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "album_id",
                table: "media",
                column: "album_id");

            migrationBuilder.CreateIndex(
                name: "uploaded_by",
                table: "media",
                column: "uploaded_by");

            migrationBuilder.CreateIndex(
                name: "idx_messages_chat",
                table: "messages",
                columns: new[] { "chat_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "sender_id",
                table: "messages",
                column: "sender_id");

            migrationBuilder.CreateIndex(
                name: "idx_notifications_user",
                table: "notifications",
                columns: new[] { "user_id", "is_read", "created_at" });

            migrationBuilder.CreateIndex(
                name: "author_id1",
                table: "posts",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "idx_posts_community",
                table: "posts",
                columns: new[] { "community_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "community_id2",
                table: "subscriptions",
                column: "community_id");

            migrationBuilder.CreateIndex(
                name: "idx_subscriptions_user",
                table: "subscriptions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "item_id",
                table: "transactions",
                column: "item_id");

            migrationBuilder.CreateIndex(
                name: "user_id1",
                table: "transactions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_user_inventory",
                table: "user_inventory",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "item_id1",
                table: "user_inventory",
                column: "item_id");

            migrationBuilder.CreateIndex(
                name: "email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "username",
                table: "users",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chat_participants");

            migrationBuilder.DropTable(
                name: "comments");

            migrationBuilder.DropTable(
                name: "media");

            migrationBuilder.DropTable(
                name: "messages");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "subscriptions");

            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.DropTable(
                name: "user_inventory");

            migrationBuilder.DropTable(
                name: "posts");

            migrationBuilder.DropTable(
                name: "albums");

            migrationBuilder.DropTable(
                name: "chats");

            migrationBuilder.DropTable(
                name: "store_items");

            migrationBuilder.DropTable(
                name: "communities");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
