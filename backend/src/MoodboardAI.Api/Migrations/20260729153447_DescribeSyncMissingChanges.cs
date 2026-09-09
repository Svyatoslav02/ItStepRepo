using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoodboardAI.Api.Migrations
{
    /// <inheritdoc />
    public partial class DescribeSyncMissingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotificationPreferences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    PushLikes = table.Column<bool>(type: "boolean", nullable: false),
                    PushComments = table.Column<bool>(type: "boolean", nullable: false),
                    PushTags = table.Column<bool>(type: "boolean", nullable: false),
                    PushFriendRequests = table.Column<bool>(type: "boolean", nullable: false),
                    PushUpdates = table.Column<bool>(type: "boolean", nullable: false),
                    PushRecommendations = table.Column<bool>(type: "boolean", nullable: false),
                    PushMentions = table.Column<bool>(type: "boolean", nullable: false),
                    EmailLikes = table.Column<bool>(type: "boolean", nullable: false),
                    EmailComments = table.Column<bool>(type: "boolean", nullable: false),
                    EmailTags = table.Column<bool>(type: "boolean", nullable: false),
                    EmailFriendRequests = table.Column<bool>(type: "boolean", nullable: false),
                    EmailUpdates = table.Column<bool>(type: "boolean", nullable: false),
                    EmailRecommendations = table.Column<bool>(type: "boolean", nullable: false),
                    EmailMentions = table.Column<bool>(type: "boolean", nullable: false),
                    QuietMode = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationPreferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationPreferences_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationPreferences_UserId",
                table: "NotificationPreferences",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationPreferences");
        }
    }
}
