using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MoodboardAI.Api.Data;
using MoodboardAI.Api.Models;
using Xunit;

namespace MoodboardAI.Tests;

/// <summary>
/// Integration tests verifying that EF Core-configured relationships
/// (foreign keys, cascade/restrict delete behavior, unique indexes) are
/// actually enforced at the database level for the content domain models
/// (Pin, Category, Tag, PinTag).
/// </summary>
/// <remarks>
/// Uses a real SQLite connection rather than the EF Core InMemory provider,
/// because InMemory does not enforce foreign key or unique index constraints
/// and would let all of these operations silently succeed. SQLite enforces
/// FK constraints once "PRAGMA foreign_keys = ON" is set, which EF Core's
/// SQLite provider does automatically for connections it opens.
/// </remarks>
public class ContentDomainRelationshipConstraintsTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly ApplicationDbContext _db;

    public ContentDomainRelationshipConstraintsTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection)
            .Options;

        _db = new ApplicationDbContext(options);
        _db.Database.EnsureCreated();
    }

    public void Dispose()
    {
        _db.Dispose();
        _connection.Dispose();
    }

    private UserEntity SeedUser()
    {
        var user = new UserEntity
        {
            FullName = "Test User",
            Email = $"{Guid.NewGuid()}@test.local",
            Username = Guid.NewGuid().ToString("N")[..8],
            PasswordHash = "hash"
        };
        _db.Users.Add(user);
        _db.SaveChanges();
        return user;
    }

    private Category SeedCategory()
    {
        var category = new Category { Name = Guid.NewGuid().ToString(), Icon = "icon" };
        _db.Categories.Add(category);
        _db.SaveChanges();
        return category;
    }

    private Tag SeedTag()
    {
        var tag = new Tag { Name = Guid.NewGuid().ToString() };
        _db.Tags.Add(tag);
        _db.SaveChanges();
        return tag;
    }

    // ──────────────────────────────────────────────
    // Foreign key violations
    // ──────────────────────────────────────────────

    [Fact]
    public void AddingPin_WithNonExistentCategoryId_ThrowsOnSave()
    {
        var author = SeedUser();

        _db.Pins.Add(new Pin
        {
            Title = "Broken pin",
            ImageUrl = "https://example.com/x.jpg",
            AuthorId = author.Id,
            CategoryId = Guid.NewGuid() // does not exist
        });

        Assert.Throws<DbUpdateException>(() => _db.SaveChanges());
    }

    [Fact]
    public void AddingPin_WithNonExistentAuthorId_ThrowsOnSave()
    {
        var category = SeedCategory();

        _db.Pins.Add(new Pin
        {
            Title = "Broken pin",
            ImageUrl = "https://example.com/x.jpg",
            AuthorId = Guid.NewGuid(), // does not exist
            CategoryId = category.Id
        });

        Assert.Throws<DbUpdateException>(() => _db.SaveChanges());
    }

    [Fact]
    public void AddingPinTag_WithNonExistentPinId_ThrowsOnSave()
    {
        var tag = SeedTag();

        _db.PinTags.Add(new PinTag
        {
            PinId = Guid.NewGuid(), // does not exist
            TagId = tag.Id
        });

        Assert.Throws<DbUpdateException>(() => _db.SaveChanges());
    }

    [Fact]
    public void AddingPinTag_WithNonExistentTagId_ThrowsOnSave()
    {
        var author = SeedUser();
        var category = SeedCategory();
        var pin = new Pin { Title = "Pin", ImageUrl = "https://example.com/x.jpg", AuthorId = author.Id, CategoryId = category.Id };
        _db.Pins.Add(pin);
        _db.SaveChanges();

        _db.PinTags.Add(new PinTag
        {
            PinId = pin.Id,
            TagId = Guid.NewGuid() // does not exist
        });

        Assert.Throws<DbUpdateException>(() => _db.SaveChanges());
    }

    // ──────────────────────────────────────────────
    // Unique index violations
    // ──────────────────────────────────────────────

    [Fact]
    public void AddingDuplicatePinTag_ForSamePinAndTag_ThrowsOnSave()
    {
        var author = SeedUser();
        var category = SeedCategory();
        var tag = SeedTag();

        var pin = new Pin { Title = "Pin", ImageUrl = "https://example.com/x.jpg", AuthorId = author.Id, CategoryId = category.Id };
        _db.Pins.Add(pin);
        _db.SaveChanges();

        _db.PinTags.Add(new PinTag { PinId = pin.Id, TagId = tag.Id });
        _db.SaveChanges();

        _db.PinTags.Add(new PinTag { PinId = pin.Id, TagId = tag.Id }); // duplicate pair

        Assert.Throws<DbUpdateException>(() => _db.SaveChanges());
    }

    [Fact]
    public void AddingCategory_WithDuplicateName_ThrowsOnSave()
    {
        var name = Guid.NewGuid().ToString();
        _db.Categories.Add(new Category { Name = name, Icon = "icon-1" });
        _db.SaveChanges();

        _db.Categories.Add(new Category { Name = name, Icon = "icon-2" }); // duplicate name

        Assert.Throws<DbUpdateException>(() => _db.SaveChanges());
    }

    [Fact]
    public void AddingTag_WithDuplicateName_ThrowsOnSave()
    {
        var name = Guid.NewGuid().ToString();
        _db.Tags.Add(new Tag { Name = name });
        _db.SaveChanges();

        _db.Tags.Add(new Tag { Name = name }); // duplicate name

        Assert.Throws<DbUpdateException>(() => _db.SaveChanges());
    }

    // ──────────────────────────────────────────────
    // Delete behaviors
    // ──────────────────────────────────────────────

    [Fact]
    public void DeletingCategory_WithExistingPins_ThrowsOnSave()
    {
        var author = SeedUser();
        var category = SeedCategory();

        _db.Pins.Add(new Pin { Title = "Pin", ImageUrl = "https://example.com/x.jpg", AuthorId = author.Id, CategoryId = category.Id });
        _db.SaveChanges();

        // Restrict delete behavior on a required FK: EF Core's change tracker
        // detects the conflict when severing the relationship (Remove) and
        // throws InvalidOperationException here, before a DbUpdateException
        // from the database would even be reached.
        Assert.Throws<InvalidOperationException>(() => _db.Categories.Remove(category));
    }

    [Fact]
    public void DeletingAuthor_CascadesDeleteOfTheirPins()
    {
        var author = SeedUser();
        var category = SeedCategory();

        var pin = new Pin { Title = "Pin", ImageUrl = "https://example.com/x.jpg", AuthorId = author.Id, CategoryId = category.Id };
        _db.Pins.Add(pin);
        _db.SaveChanges();

        _db.Users.Remove(author); // Cascade delete behavior
        _db.SaveChanges();

        Assert.Null(_db.Pins.Find(pin.Id));
    }

    [Fact]
    public void DeletingPin_CascadesDeleteOfItsPinTags()
    {
        var author = SeedUser();
        var category = SeedCategory();
        var tag = SeedTag();

        var pin = new Pin { Title = "Pin", ImageUrl = "https://example.com/x.jpg", AuthorId = author.Id, CategoryId = category.Id };
        _db.Pins.Add(pin);
        _db.SaveChanges();

        var pinTag = new PinTag { PinId = pin.Id, TagId = tag.Id };
        _db.PinTags.Add(pinTag);
        _db.SaveChanges();

        _db.Pins.Remove(pin); // Cascade delete behavior
        _db.SaveChanges();

        Assert.Null(_db.PinTags.Find(pinTag.Id));
    }

    [Fact]
    public void DeletingTag_CascadesDeleteOfItsPinTags()
    {
        var author = SeedUser();
        var category = SeedCategory();
        var tag = SeedTag();

        var pin = new Pin { Title = "Pin", ImageUrl = "https://example.com/x.jpg", AuthorId = author.Id, CategoryId = category.Id };
        _db.Pins.Add(pin);
        _db.SaveChanges();

        var pinTag = new PinTag { PinId = pin.Id, TagId = tag.Id };
        _db.PinTags.Add(pinTag);
        _db.SaveChanges();

        _db.Tags.Remove(tag); // Cascade delete behavior
        _db.SaveChanges();

        Assert.Null(_db.PinTags.Find(pinTag.Id));
    }
}