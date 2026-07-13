using Microsoft.EntityFrameworkCore;
using MoodboardAI.Api.Models.Entities;
namespace MoodboardAI.Api.Data;

public static class TegsSeedData
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        // Categories
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Interior Design" },
            new Category { Id = 2, Name = "Art & Illustration" },
            new Category { Id = 3, Name = "Technology" },
            new Category { Id = 4, Name = "Food & Drink" },
            new Category { Id = 5, Name = "Travel" },
            new Category { Id = 6, Name = "Nature" },
            new Category { Id = 7, Name = "Photography" },
            new Category { Id = 8, Name = "Architecture" }
        );
        
        // Tags
        modelBuilder.Entity<Tag>().HasData(
            new Tag { Id = 1, Name = "minimal" },
            new Tag { Id = 2, Name = "modern" },
            new Tag { Id = 3, Name = "abstract" },
            new Tag { Id = 4, Name = "botanical" },
            new Tag { Id = 5, Name = "creative" },
            new Tag { Id = 6, Name = "galaxy" },
            new Tag { Id = 7, Name = "moon" },
            new Tag { Id = 8, Name = "night-drive" },
            new Tag { Id = 9, Name = "above-clouds" }
        );

        // Pins
        modelBuilder.Entity<Pin>().HasData(
            new Pin { Id = 1, Title = "Modern Living Room", ImageUrl = "https://www.realhomes.com/design/modern-living-room-ideas", CategoryId = 1 },
            new Pin { Id = 2, Title = "Galaxy Art", ImageUrl = "https://www.etsy.com/uk/listing/1064354283/starry-night-sky-galaxy-watercolor-art", CategoryId = 2 }
        );

        // PinTags (зв’язки для пошуку по тегам)
        modelBuilder.Entity<PinTag>().HasData(
            new PinTag { PinId = 1, TagId = 2 }, // modern
            new PinTag { PinId = 2, TagId = 6 }  // galaxy
        );
        
    }
}