using Microsoft.EntityFrameworkCore;
using MoodboardAI.Api.Models;

namespace MoodboardAI.Api.Data;

public static class TegsSeedData
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        // Categories
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Interior Design", Icon = "interior.png", CreatedAt = new DateTime(2026, 07, 07) },
            new Category { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Art & Illustration", Icon = "art.png", CreatedAt = new DateTime(2026, 07, 07) },
            new Category { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "Technology", Icon = "tech.png", CreatedAt = new DateTime(2026, 07, 07) },
            new Category { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), Name = "Food & Drink", Icon = "food.png", CreatedAt = new DateTime(2026, 07, 07) },
            new Category { Id = Guid.Parse("55555555-5555-5555-5555-555555555555"), Name = "Travel", Icon = "travel.png", CreatedAt = new DateTime(2026, 07, 07) },
            new Category { Id = Guid.Parse("66666666-6666-6666-6666-666666666666"), Name = "Nature", Icon = "nature.png", CreatedAt = new DateTime(2026, 07, 07) },
            new Category { Id = Guid.Parse("77777777-7777-7777-7777-777777777777"), Name = "Photography", Icon = "photo.png", CreatedAt = new DateTime(2026, 07, 07) },
            new Category { Id = Guid.Parse("88888888-8888-8888-8888-888888888888"), Name = "Architecture", Icon = "arch.png", CreatedAt = new DateTime(2026, 07, 07) }
        );

        // Tags
        modelBuilder.Entity<Tag>().HasData(
            new Tag { Id = Guid.Parse("aaaaaaa1-aaaa-aaaa-aaaa-aaaaaaaaaaa1"), Name = "minimal", CreatedAt = new DateTime(2026, 07, 07) },
            new Tag { Id = Guid.Parse("aaaaaaa2-aaaa-aaaa-aaaa-aaaaaaaaaaa2"), Name = "modern", CreatedAt = new DateTime(2026, 07, 07) },
            new Tag { Id = Guid.Parse("aaaaaaa3-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), Name = "abstract", CreatedAt = new DateTime(2026, 07, 07) },
            new Tag { Id = Guid.Parse("aaaaaaa4-aaaa-aaaa-aaaa-aaaaaaaaaaa4"), Name = "botanical", CreatedAt = new DateTime(2026, 07, 07) },
            new Tag { Id = Guid.Parse("aaaaaaa5-aaaa-aaaa-aaaa-aaaaaaaaaaa5"), Name = "creative", CreatedAt = new DateTime(2026, 07, 07) },
            new Tag { Id = Guid.Parse("aaaaaaa6-aaaa-aaaa-aaaa-aaaaaaaaaaa6"), Name = "galaxy", CreatedAt = new DateTime(2026, 07, 07) },
            new Tag { Id = Guid.Parse("aaaaaaa7-aaaa-aaaa-aaaa-aaaaaaaaaaa7"), Name = "moon", CreatedAt = new DateTime(2026, 07, 07) },
            new Tag { Id = Guid.Parse("aaaaaaa8-aaaa-aaaa-aaaa-aaaaaaaaaaa8"), Name = "night-drive", CreatedAt = new DateTime(2026, 07, 07) },
            new Tag { Id = Guid.Parse("aaaaaaa9-aaaa-aaaa-aaaa-aaaaaaaaaaa9"), Name = "above-clouds", CreatedAt = new DateTime(2026, 07, 07) }
        );

        // Pins
        modelBuilder.Entity<Pin>().HasData(
            new Pin { Id = Guid.Parse("99999999-9999-9999-9999-999999999999"), Title = "Modern Living Room", ImageUrl = "https://i.pinimg.com/736x/1f/7e/53/1f7e53a190519f8ccbe427e431351e42.jpg", CategoryId = Guid.Parse("11111111-1111-1111-1111-111111111111"), AuthorId = Guid.Parse("bbbbbbb1-bbbb-bbbb-bbbb-bbbbbbbbbbb1"), CreatedAt = new DateTime(2026, 07, 07), UpdatedAt = new DateTime(2026, 07, 08) },
            new Pin { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Title = "Galaxy Art", ImageUrl = "https://i.pinimg.com/1200x/7c/f1/b3/7cf1b3f266e793502d1820b16f2df3b4.jpg", CategoryId = Guid.Parse("22222222-2222-2222-2222-222222222222"), AuthorId = Guid.Parse("bbbbbbb2-bbbb-bbbb-bbbb-bbbbbbbbbbb2"), CreatedAt = new DateTime(2026, 07, 07), UpdatedAt = new DateTime(2026, 07, 08) }
        );

        // PinTags
        modelBuilder.Entity<PinTag>().HasData(
            new PinTag { Id = Guid.Parse("ccccccc1-cccc-cccc-cccc-ccccccccccc1"), PinId = Guid.Parse("99999999-9999-9999-9999-999999999999"), TagId = Guid.Parse("aaaaaaa2-aaaa-aaaa-aaaa-aaaaaaaaaaa2"), CreatedAt = new DateTime(2026, 07, 07) }, // modern
            new PinTag { Id = Guid.Parse("ccccccc2-cccc-cccc-cccc-ccccccccccc2"), PinId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), TagId = Guid.Parse("aaaaaaa6-aaaa-aaaa-aaaa-aaaaaaaaaaa6"), CreatedAt = new DateTime(2026, 07, 07) }  // galaxy
        );
    }
}
