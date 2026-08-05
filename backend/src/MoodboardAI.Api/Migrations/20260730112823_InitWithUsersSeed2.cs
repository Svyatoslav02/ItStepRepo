using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MoodboardAI.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitWithUsersSeed2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Icon = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Interests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Icon = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    Bio = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    AvatarUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsOnboardingCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlockedUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BlockerId = table.Column<Guid>(type: "uuid", nullable: false),
                    BlockedUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlockedUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlockedUsers_Users_BlockedUserId",
                        column: x => x.BlockedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BlockedUsers_Users_BlockerId",
                        column: x => x.BlockerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "Pins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ImageUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    SourceUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pins_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pins_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecentSearches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Query = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecentSearches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecentSearches_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserInterests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    InterestId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInterests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserInterests_Interests_InterestId",
                        column: x => x.InterestId,
                        principalTable: "Interests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserInterests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPrivacySettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    PrivateAccount = table.Column<bool>(type: "boolean", nullable: false),
                    SearchVisibility = table.Column<bool>(type: "boolean", nullable: false),
                    ContentVisibility = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPrivacySettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPrivacySettings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PinId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Likes_Pins_PinId",
                        column: x => x.PinId,
                        principalTable: "Pins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Likes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PinTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PinId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PinTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PinTags_Pins_PinId",
                        column: x => x.PinId,
                        principalTable: "Pins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PinTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Saves",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PinId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Saves", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Saves_Pins_PinId",
                        column: x => x.PinId,
                        principalTable: "Pins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Saves_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Icon", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), "interior.png", "Interior Design" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), "art.png", "Art & Illustration" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), "tech.png", "Technology" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), "food.png", "Food & Drink" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), "travel.png", "Travel" },
                    { new Guid("66666666-6666-6666-6666-666666666666"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), "nature.png", "Nature" },
                    { new Guid("77777777-7777-7777-7777-777777777777"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), "photo.png", "Photography" },
                    { new Guid("88888888-8888-8888-8888-888888888888"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), "arch.png", "Architecture" }
                });

            migrationBuilder.InsertData(
                table: "Interests",
                columns: new[] { "Id", "CreatedAt", "Icon", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111101"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "minimal", "Minimal" },
                    { new Guid("11111111-1111-1111-1111-111111111102"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "3d-art", "3D Art" },
                    { new Guid("11111111-1111-1111-1111-111111111103"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "app-mobile", "App Mobile" },
                    { new Guid("11111111-1111-1111-1111-111111111104"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "retro", "Retro" },
                    { new Guid("11111111-1111-1111-1111-111111111105"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "photography", "Photography" },
                    { new Guid("11111111-1111-1111-1111-111111111106"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "architecture", "Architecture" },
                    { new Guid("11111111-1111-1111-1111-111111111107"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "modern", "Modern" },
                    { new Guid("11111111-1111-1111-1111-111111111108"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "art", "Art" },
                    { new Guid("11111111-1111-1111-1111-111111111109"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "eco", "Eco" },
                    { new Guid("11111111-1111-1111-1111-111111111110"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "prints", "Prints" }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "CreatedAt", "Name" },
                values: new object[,]
                {
                    { new Guid("aaaaaaa1-aaaa-aaaa-aaaa-aaaaaaaaaaa1"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), "minimal" },
                    { new Guid("aaaaaaa2-aaaa-aaaa-aaaa-aaaaaaaaaaa2"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), "modern" },
                    { new Guid("aaaaaaa3-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), "abstract" },
                    { new Guid("aaaaaaa4-aaaa-aaaa-aaaa-aaaaaaaaaaa4"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), "botanical" },
                    { new Guid("aaaaaaa5-aaaa-aaaa-aaaa-aaaaaaaaaaa5"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), "creative" },
                    { new Guid("aaaaaaa6-aaaa-aaaa-aaaa-aaaaaaaaaaa6"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), "galaxy" },
                    { new Guid("aaaaaaa7-aaaa-aaaa-aaaa-aaaaaaaaaaa7"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), "moon" },
                    { new Guid("aaaaaaa8-aaaa-aaaa-aaaa-aaaaaaaaaaa8"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), "night-drive" },
                    { new Guid("aaaaaaa9-aaaa-aaaa-aaaa-aaaaaaaaaaa9"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), "above-clouds" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AvatarUrl", "Bio", "CreatedAt", "DateOfBirth", "DisplayName", "Email", "FullName", "IsOnboardingCompleted", "PasswordHash", "UpdatedAt", "Username" },
                values: new object[,]
                {
                    { new Guid("bbbbbbb1-bbbb-bbbb-bbbb-bbbbbbbbbbb1"), null, null, new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), null, "Author One", "author1@example.com", "Author One", false, "HASHED_PASSWORD_1", new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), "authorone" },
                    { new Guid("bbbbbbb2-bbbb-bbbb-bbbb-bbbbbbbbbbb2"), null, null, new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), null, "Author Two", "author2@example.com", "Author Two", false, "HASHED_PASSWORD_2", new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), "authortwo" }
                });

            migrationBuilder.InsertData(
                table: "Pins",
                columns: new[] { "Id", "AuthorId", "CategoryId", "CreatedAt", "Description", "ImageUrl", "SourceUrl", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("99999999-9999-9999-9999-999999999999"), new Guid("bbbbbbb1-bbbb-bbbb-bbbb-bbbbbbbbbbb1"), new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), null, "https://i.pinimg.com/736x/1f/7e/53/1f7e53a190519f8ccbe427e431351e42.jpg", null, "Modern Living Room", new DateTime(2026, 7, 8, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("bbbbbbb2-bbbb-bbbb-bbbb-bbbbbbbbbbb2"), new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), null, "https://i.pinimg.com/1200x/7c/f1/b3/7cf1b3f266e793502d1820b16f2df3b4.jpg", null, "Galaxy Art", new DateTime(2026, 7, 8, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "PinTags",
                columns: new[] { "Id", "CreatedAt", "PinId", "TagId" },
                values: new object[,]
                {
                    { new Guid("ccccccc1-cccc-cccc-cccc-ccccccccccc1"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999999"), new Guid("aaaaaaa2-aaaa-aaaa-aaaa-aaaaaaaaaaa2") },
                    { new Guid("ccccccc2-cccc-cccc-cccc-ccccccccccc2"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("aaaaaaa6-aaaa-aaaa-aaaa-aaaaaaaaaaa6") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlockedUsers_BlockedUserId",
                table: "BlockedUsers",
                column: "BlockedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BlockedUsers_BlockerId_BlockedUserId",
                table: "BlockedUsers",
                columns: new[] { "BlockerId", "BlockedUserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Likes_PinId_UserId",
                table: "Likes",
                columns: new[] { "PinId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Likes_UserId",
                table: "Likes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationPreferences_UserId",
                table: "NotificationPreferences",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Pins_AuthorId",
                table: "Pins",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Pins_CategoryId",
                table: "Pins",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Pins_Title",
                table: "Pins",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_PinTags_PinId_TagId",
                table: "PinTags",
                columns: new[] { "PinId", "TagId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PinTags_TagId",
                table: "PinTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_RecentSearches_UserId_Query",
                table: "RecentSearches",
                columns: new[] { "UserId", "Query" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Saves_PinId_UserId",
                table: "Saves",
                columns: new[] { "PinId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Saves_UserId",
                table: "Saves",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name",
                table: "Tags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserInterests_InterestId",
                table: "UserInterests",
                column: "InterestId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInterests_UserId_InterestId",
                table: "UserInterests",
                columns: new[] { "UserId", "InterestId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPrivacySettings_UserId",
                table: "UserPrivacySettings",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlockedUsers");

            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.DropTable(
                name: "NotificationPreferences");

            migrationBuilder.DropTable(
                name: "PinTags");

            migrationBuilder.DropTable(
                name: "RecentSearches");

            migrationBuilder.DropTable(
                name: "Saves");

            migrationBuilder.DropTable(
                name: "UserInterests");

            migrationBuilder.DropTable(
                name: "UserPrivacySettings");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Pins");

            migrationBuilder.DropTable(
                name: "Interests");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
