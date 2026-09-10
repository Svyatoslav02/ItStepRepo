using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoodboardAI.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationPreferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "QuietModeEnd",
                table: "NotificationPreferences",
                type: "interval",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "QuietModeStart",
                table: "NotificationPreferences",
                type: "interval",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuietModeEnd",
                table: "NotificationPreferences");

            migrationBuilder.DropColumn(
                name: "QuietModeStart",
                table: "NotificationPreferences");
        }
    }
}
