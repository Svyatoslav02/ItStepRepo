using MoodboardAI.Api.Models;

namespace MoodboardAI.Api.Data;

/// <summary>
/// Provides the fixed set of default onboarding interests seeded into the database
/// via EF Core migrations (<see cref="ApplicationDbContext.OnModelCreating"/>).
/// </summary>
public static class InterestSeedData
{
    /// <summary>
    /// Fixed UTC timestamp used for all seeded rows so that the EF Core model
    /// snapshot stays stable between migrations (avoids spurious diffs).
    /// </summary>
    private static readonly DateTime SeedCreatedAt = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// Default onboarding interests. Ids are fixed (not <c>Guid.NewGuid()</c>) so
    /// that <c>HasData</c> seeding is deterministic and does not produce duplicate
    /// rows or unnecessary migrations on subsequent builds.
    /// </summary>
    public static readonly Interest[] Default =
    {
        new()
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111101"),
            Name = "Minimal",
            Icon = "minimal",
            CreatedAt = SeedCreatedAt
        },
        new()
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111102"),
            Name = "3D Art",
            Icon = "3d-art",
            CreatedAt = SeedCreatedAt
        },
        new()
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111103"),
            Name = "App Mobile",
            Icon = "app-mobile",
            CreatedAt = SeedCreatedAt
        },
        new()
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111104"),
            Name = "Retro",
            Icon = "retro",
            CreatedAt = SeedCreatedAt
        },
        new()
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111105"),
            Name = "Photography",
            Icon = "photography",
            CreatedAt = SeedCreatedAt
        },
        new()
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111106"),
            Name = "Architecture",
            Icon = "architecture",
            CreatedAt = SeedCreatedAt
        },
        new()
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111107"),
            Name = "Modern",
            Icon = "modern",
            CreatedAt = SeedCreatedAt
        },
        new()
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111108"),
            Name = "Art",
            Icon = "art",
            CreatedAt = SeedCreatedAt
        },
        new()
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111109"),
            Name = "Eco",
            Icon = "eco",
            CreatedAt = SeedCreatedAt
        },
        new()
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111110"),
            Name = "Prints",
            Icon = "prints",
            CreatedAt = SeedCreatedAt
        }
    };
}
