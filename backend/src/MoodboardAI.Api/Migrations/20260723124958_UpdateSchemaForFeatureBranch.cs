using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MoodboardAI.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchemaForFeatureBranch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Icon", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "interior.png", "Interior Design" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "art.png", "Art & Illustration" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "tech.png", "Technology" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "food.png", "Food & Drink" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "travel.png", "Travel" },
                    { new Guid("66666666-6666-6666-6666-666666666666"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "nature.png", "Nature" },
                    { new Guid("77777777-7777-7777-7777-777777777777"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "photo.png", "Photography" },
                    { new Guid("88888888-8888-8888-8888-888888888888"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "arch.png", "Architecture" }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "CreatedAt", "Name" },
                values: new object[,]
                {
                    { new Guid("aaaaaaa1-aaaa-aaaa-aaaa-aaaaaaaaaaa1"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "minimal" },
                    { new Guid("aaaaaaa2-aaaa-aaaa-aaaa-aaaaaaaaaaa2"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "modern" },
                    { new Guid("aaaaaaa3-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "abstract" },
                    { new Guid("aaaaaaa4-aaaa-aaaa-aaaa-aaaaaaaaaaa4"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "botanical" },
                    { new Guid("aaaaaaa5-aaaa-aaaa-aaaa-aaaaaaaaaaa5"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "creative" },
                    { new Guid("aaaaaaa6-aaaa-aaaa-aaaa-aaaaaaaaaaa6"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "galaxy" },
                    { new Guid("aaaaaaa7-aaaa-aaaa-aaaa-aaaaaaaaaaa7"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "moon" },
                    { new Guid("aaaaaaa8-aaaa-aaaa-aaaa-aaaaaaaaaaa8"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "night-drive" },
                    { new Guid("aaaaaaa9-aaaa-aaaa-aaaa-aaaaaaaaaaa9"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "above-clouds" }
                });

            migrationBuilder.InsertData(
                table: "Pins",
                columns: new[] { "Id", "AuthorId", "CategoryId", "CreatedAt", "Description", "ImageUrl", "SourceUrl", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("99999999-9999-9999-9999-999999999999"), new Guid("bbbbbbb1-bbbb-bbbb-bbbb-bbbbbbbbbbb1"), new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "https://i.pinimg.com/736x/1f/7e/53/1f7e53a190519f8ccbe427e431351e42.jpg", null, "Modern Living Room", new DateTime(2026, 7, 8, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("bbbbbbb2-bbbb-bbbb-bbbb-bbbbbbbbbbb2"), new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "https://i.pinimg.com/1200x/7c/f1/b3/7cf1b3f266e793502d1820b16f2df3b4.jpg", null, "Galaxy Art", new DateTime(2026, 7, 8, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "PinTags",
                columns: new[] { "Id", "CreatedAt", "PinId", "TagId" },
                values: new object[,]
                {
                    { new Guid("ccccccc1-cccc-cccc-cccc-ccccccccccc1"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("99999999-9999-9999-9999-999999999999"), new Guid("aaaaaaa2-aaaa-aaaa-aaaa-aaaaaaaaaaa2") },
                    { new Guid("ccccccc2-cccc-cccc-cccc-ccccccccccc2"), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("aaaaaaa6-aaaa-aaaa-aaaa-aaaaaaaaaaa6") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"));

            migrationBuilder.DeleteData(
                table: "PinTags",
                keyColumn: "Id",
                keyValue: new Guid("ccccccc1-cccc-cccc-cccc-ccccccccccc1"));

            migrationBuilder.DeleteData(
                table: "PinTags",
                keyColumn: "Id",
                keyValue: new Guid("ccccccc2-cccc-cccc-cccc-ccccccccccc2"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa1-aaaa-aaaa-aaaa-aaaaaaaaaaa1"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa3-aaaa-aaaa-aaaa-aaaaaaaaaaa3"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa4-aaaa-aaaa-aaaa-aaaaaaaaaaa4"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa5-aaaa-aaaa-aaaa-aaaaaaaaaaa5"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa7-aaaa-aaaa-aaaa-aaaaaaaaaaa7"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa8-aaaa-aaaa-aaaa-aaaaaaaaaaa8"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa9-aaaa-aaaa-aaaa-aaaaaaaaaaa9"));

            migrationBuilder.DeleteData(
                table: "Pins",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"));

            migrationBuilder.DeleteData(
                table: "Pins",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa2-aaaa-aaaa-aaaa-aaaaaaaaaaa2"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaa6-aaaa-aaaa-aaaa-aaaaaaaaaaa6"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));
        }
    }
}
