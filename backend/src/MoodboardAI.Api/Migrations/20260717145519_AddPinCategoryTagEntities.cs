using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoodboardAI.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddPinCategoryTagEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pins_Categories_CategoryId",
                table: "Pins");

            migrationBuilder.AddForeignKey(
                name: "FK_Pins_Categories_CategoryId",
                table: "Pins",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pins_Categories_CategoryId",
                table: "Pins");

            migrationBuilder.AddForeignKey(
                name: "FK_Pins_Categories_CategoryId",
                table: "Pins",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
