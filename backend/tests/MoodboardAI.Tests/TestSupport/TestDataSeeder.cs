using MoodboardAI.Api.Data;
using MoodboardAI.Api.Models;

namespace MoodboardAI.Tests.TestSupport;

/// <summary>
/// Small helpers for seeding minimal-but-valid entities directly into an
/// in-memory <see cref="ApplicationDbContext"/> for HTTP-level integration
/// tests, without going through the full registration flow.
/// </summary>
internal static class TestDataSeeder
{
    /// <summary>
    /// Creates and persists a minimal valid <see cref="UserEntity"/>.
    /// </summary>
    public static async Task<UserEntity> SeedUserAsync(
        ApplicationDbContext db,
        Guid? id = null,
        string? email = null,
        string fullName = "Test User")
    {
        var user = new UserEntity
        {
            Id = id ?? Guid.NewGuid(),
            FullName = fullName,
            Email = email ?? $"{Guid.NewGuid():N}@example.com",
            Username = $"user_{Guid.NewGuid():N}"[..12],
            PasswordHash = "not-a-real-hash"
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();

        return user;
    }

    /// <summary>
    /// Creates and persists a minimal valid <see cref="Category"/>.
    /// </summary>
    public static async Task<Category> SeedCategoryAsync(ApplicationDbContext db, string name = "Interior")
    {
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = $"{name}-{Guid.NewGuid():N}"[..20],
            Icon = "icon.svg"
        };

        db.Categories.Add(category);
        await db.SaveChangesAsync();

        return category;
    }

    /// <summary>
    /// Creates and persists a minimal valid <see cref="Pin"/>, seeding an
    /// author and category for it if none are supplied.
    /// </summary>
    public static async Task<Pin> SeedPinAsync(
        ApplicationDbContext db,
        UserEntity? author = null,
        Category? category = null,
        string title = "Test Pin")
    {
        author ??= await SeedUserAsync(db);
        category ??= await SeedCategoryAsync(db);

        var pin = new Pin
        {
            Id = Guid.NewGuid(),
            Title = title,
            ImageUrl = "https://example.com/image.png",
            AuthorId = author.Id,
            CategoryId = category.Id
        };

        db.Pins.Add(pin);
        await db.SaveChangesAsync();

        return pin;
    }
}
