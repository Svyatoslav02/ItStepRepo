using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MoodboardAI.Api.Migrations
{
    /// <inheritdoc />
    public partial class FinalSeedDataMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pins_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PinTags",
                columns: table => new
                {
                    PinId = table.Column<int>(type: "integer", nullable: false),
                    TagId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PinTags", x => new { x.PinId, x.TagId });
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

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Interior Design" },
                    { 2, "Art & Illustration" },
                    { 3, "Technology" },
                    { 4, "Food & Drink" },
                    { 5, "Travel" },
                    { 6, "Nature" },
                    { 7, "Photography" },
                    { 8, "Architecture" }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "minimal" },
                    { 2, "modern" },
                    { 3, "abstract" },
                    { 4, "botanical" },
                    { 5, "creative" },
                    { 6, "galaxy" },
                    { 7, "moon" },
                    { 8, "night-drive" },
                    { 9, "above-clouds" }
                });

            migrationBuilder.InsertData(
                table: "Pins",
                columns: new[] { "Id", "CategoryId", "ImageUrl", "Title" },
                values: new object[,]
                {
                    { 1, 1, "https://www.realhomes.com/design/modern-living-room-ideas", "Modern Living Room" },
                    { 2, 2, "https://www.etsy.com/uk/listing/1064354283/starry-night-sky-galaxy-watercolor-art", "Galaxy Art" }
                });

            migrationBuilder.InsertData(
                table: "PinTags",
                columns: new[] { "PinId", "TagId" },
                values: new object[,]
                {
                    { 1, 2 },
                    { 2, 6 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pins_CategoryId",
                table: "Pins",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PinTags_TagId",
                table: "PinTags",
                column: "TagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PinTags");

            migrationBuilder.DropTable(
                name: "Pins");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
