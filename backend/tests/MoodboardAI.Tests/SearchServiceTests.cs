using Microsoft.EntityFrameworkCore;
using MoodboardAI.Api.Data;
using MoodboardAI.Api.Models;
using MoodboardAI.Api.Services;
using MoodboardAI.Tests.TestSupport;
using Xunit;

namespace MoodboardAI.Tests;

/// <summary>
/// Unit tests for <see cref="SearchService"/> covering search filtering,
/// trending ranking, and category listing. Uses EF Core InMemory, which is
/// sufficient here since these tests don't rely on FK/unique constraint
/// enforcement (see ContentDomainRelationshipConstraintsTests for that).
/// </summary>
public class SearchServiceTests
{
    private static ApplicationDbContext CreateInMemoryDb()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new ApplicationDbContext(options);
    }

    // ──────────────────────────────────────────────
    // SearchAsync — text query
    // ──────────────────────────────────────────────

    [Fact]
    public async Task SearchAsync_MatchesTitle()
    {
        using var db = CreateInMemoryDb();
        var author = await TestDataSeeder.SeedUserAsync(db);
        var category = await TestDataSeeder.SeedCategoryAsync(db);
        await TestDataSeeder.SeedPinAsync(db, author, category, title: "Minimalist Kitchen");
        await TestDataSeeder.SeedPinAsync(db, author, category, title: "Retro Car");

        var service = new SearchService(db);
        var result = await service.SearchAsync("minimalist", null, null, 1, 10);

        Assert.Equal(1, result.TotalCount);
        Assert.Equal("Minimalist Kitchen", result.Items[0].Title);
    }

    [Fact]
    public async Task SearchAsync_MatchesDescription()
    {
        using var db = CreateInMemoryDb();
        var author = await TestDataSeeder.SeedUserAsync(db);
        var category = await TestDataSeeder.SeedCategoryAsync(db);
        await TestDataSeeder.SeedPinAsync(db, author, category, title: "Living Room", description: "Cozy japanese-style corner");
        await TestDataSeeder.SeedPinAsync(db, author, category, title: "Office", description: "Modern desk setup");

        var service = new SearchService(db);
        var result = await service.SearchAsync("japanese", null, null, 1, 10);

        Assert.Equal(1, result.TotalCount);
        Assert.Equal("Living Room", result.Items[0].Title);
    }

    [Fact]
    public async Task SearchAsync_MatchesCategoryName()
    {
        using var db = CreateInMemoryDb();
        var author = await TestDataSeeder.SeedUserAsync(db);
        var interiorCategory = await TestDataSeeder.SeedCategoryAsync(db, "Interior");
        var techCategory = await TestDataSeeder.SeedCategoryAsync(db, "Technology");
        await TestDataSeeder.SeedPinAsync(db, author, interiorCategory, title: "Sofa");
        await TestDataSeeder.SeedPinAsync(db, author, techCategory, title: "Laptop");

        var service = new SearchService(db);
        var result = await service.SearchAsync("interior", null, null, 1, 10);

        Assert.Equal(1, result.TotalCount);
        Assert.Equal("Sofa", result.Items[0].Title);
    }

    [Fact]
    public async Task SearchAsync_MatchesTagName()
    {
        using var db = CreateInMemoryDb();
        var author = await TestDataSeeder.SeedUserAsync(db);
        var category = await TestDataSeeder.SeedCategoryAsync(db);
        var tag = await TestDataSeeder.SeedTagAsync(db, "sunset");
        var taggedPin = await TestDataSeeder.SeedPinAsync(db, author, category, title: "Beach Photo");
        await TestDataSeeder.AttachTagAsync(db, taggedPin, tag);
        await TestDataSeeder.SeedPinAsync(db, author, category, title: "Mountain Photo");

        var service = new SearchService(db);
        var result = await service.SearchAsync("sunset", null, null, 1, 10);

        Assert.Equal(1, result.TotalCount);
        Assert.Equal("Beach Photo", result.Items[0].Title);
        Assert.Contains(result.Items[0].Tags, t => t.StartsWith("sunset"));
    }

    [Fact]
    public async Task SearchAsync_NoMatches_ReturnsEmptyList()
    {
        using var db = CreateInMemoryDb();
        var author = await TestDataSeeder.SeedUserAsync(db);
        var category = await TestDataSeeder.SeedCategoryAsync(db);
        await TestDataSeeder.SeedPinAsync(db, author, category, title: "Some Pin");

        var service = new SearchService(db);
        var result = await service.SearchAsync("nonexistent-query-xyz", null, null, 1, 10);

        Assert.Equal(0, result.TotalCount);
        Assert.Empty(result.Items);
    }

    [Fact]
    public async Task SearchAsync_EmptyQuery_ReturnsAllPins()
    {
        using var db = CreateInMemoryDb();
        var author = await TestDataSeeder.SeedUserAsync(db);
        var category = await TestDataSeeder.SeedCategoryAsync(db);
        await TestDataSeeder.SeedPinAsync(db, author, category, title: "Pin A");
        await TestDataSeeder.SeedPinAsync(db, author, category, title: "Pin B");

        var service = new SearchService(db);
        var result = await service.SearchAsync(null, null, null, 1, 10);

        Assert.Equal(2, result.TotalCount);
    }

    // ──────────────────────────────────────────────
    // SearchAsync — filters
    // ──────────────────────────────────────────────

    [Fact]
    public async Task SearchAsync_FiltersByCategoryId()
    {
        using var db = CreateInMemoryDb();
        var author = await TestDataSeeder.SeedUserAsync(db);
        var categoryA = await TestDataSeeder.SeedCategoryAsync(db, "CategoryA");
        var categoryB = await TestDataSeeder.SeedCategoryAsync(db, "CategoryB");
        await TestDataSeeder.SeedPinAsync(db, author, categoryA, title: "In A");
        await TestDataSeeder.SeedPinAsync(db, author, categoryB, title: "In B");

        var service = new SearchService(db);
        var result = await service.SearchAsync(null, categoryA.Id, null, 1, 10);

        Assert.Equal(1, result.TotalCount);
        Assert.Equal("In A", result.Items[0].Title);
    }

    [Fact]
    public async Task SearchAsync_FiltersByTagId()
    {
        using var db = CreateInMemoryDb();
        var author = await TestDataSeeder.SeedUserAsync(db);
        var category = await TestDataSeeder.SeedCategoryAsync(db);
        var tagA = await TestDataSeeder.SeedTagAsync(db, "tagA");
        var tagB = await TestDataSeeder.SeedTagAsync(db, "tagB");
        var pinWithTagA = await TestDataSeeder.SeedPinAsync(db, author, category, title: "Tagged A");
        await TestDataSeeder.AttachTagAsync(db, pinWithTagA, tagA);
        var pinWithTagB = await TestDataSeeder.SeedPinAsync(db, author, category, title: "Tagged B");
        await TestDataSeeder.AttachTagAsync(db, pinWithTagB, tagB);

        var service = new SearchService(db);
        var result = await service.SearchAsync(null, null, tagA.Id, 1, 10);

        Assert.Equal(1, result.TotalCount);
        Assert.Equal("Tagged A", result.Items[0].Title);
    }

    [Fact]
    public async Task SearchAsync_CombinesQueryAndCategoryFilter()
    {
        using var db = CreateInMemoryDb();
        var author = await TestDataSeeder.SeedUserAsync(db);
        var categoryA = await TestDataSeeder.SeedCategoryAsync(db, "CategoryA");
        var categoryB = await TestDataSeeder.SeedCategoryAsync(db, "CategoryB");
        await TestDataSeeder.SeedPinAsync(db, author, categoryA, title: "Modern Chair");
        await TestDataSeeder.SeedPinAsync(db, author, categoryB, title: "Modern Phone");

        var service = new SearchService(db);
        var result = await service.SearchAsync("modern", categoryA.Id, null, 1, 10);

        Assert.Equal(1, result.TotalCount);
        Assert.Equal("Modern Chair", result.Items[0].Title);
    }

    // ──────────────────────────────────────────────
    // SearchAsync — pagination
    // ──────────────────────────────────────────────

    [Fact]
    public async Task SearchAsync_RespectsPageSize()
    {
        using var db = CreateInMemoryDb();
        var author = await TestDataSeeder.SeedUserAsync(db);
        var category = await TestDataSeeder.SeedCategoryAsync(db);

        for (var i = 0; i < 5; i++)
        {
            await TestDataSeeder.SeedPinAsync(db, author, category, title: $"Pin {i}");
        }

        var service = new SearchService(db);
        var result = await service.SearchAsync(null, null, null, 1, 2);

        Assert.Equal(5, result.TotalCount);
        Assert.Equal(2, result.Items.Count);
        Assert.Equal(1, result.Page);
        Assert.Equal(2, result.PageSize);
    }

    [Fact]
    public async Task SearchAsync_SecondPage_ReturnsRemainingItems()
    {
        using var db = CreateInMemoryDb();
        var author = await TestDataSeeder.SeedUserAsync(db);
        var category = await TestDataSeeder.SeedCategoryAsync(db);

        for (var i = 0; i < 5; i++)
        {
            await TestDataSeeder.SeedPinAsync(db, author, category, title: $"Pin {i}");
        }

        var service = new SearchService(db);
        var result = await service.SearchAsync(null, null, null, 2, 2);

        Assert.Equal(5, result.TotalCount);
        Assert.Equal(2, result.Items.Count);
        Assert.Equal(2, result.Page);
    }

    [Fact]
    public async Task SearchAsync_PageSizeAboveMax_IsCapped()
    {
        using var db = CreateInMemoryDb();
        var author = await TestDataSeeder.SeedUserAsync(db);
        var category = await TestDataSeeder.SeedCategoryAsync(db);
        await TestDataSeeder.SeedPinAsync(db, author, category);

        var service = new SearchService(db);
        var result = await service.SearchAsync(null, null, null, 1, 500);

        Assert.Equal(100, result.PageSize);
    }

    [Fact]
    public async Task SearchAsync_InvalidPageOrPageSize_FallsBackToDefaults()
    {
        using var db = CreateInMemoryDb();
        var author = await TestDataSeeder.SeedUserAsync(db);
        var category = await TestDataSeeder.SeedCategoryAsync(db);
        await TestDataSeeder.SeedPinAsync(db, author, category);

        var service = new SearchService(db);
        var result = await service.SearchAsync(null, null, null, 0, 0);

        Assert.Equal(1, result.Page);
        Assert.Equal(10, result.PageSize);
    }

    // ──────────────────────────────────────────────
    // SearchAsync — result shape
    // ──────────────────────────────────────────────

    [Fact]
    public async Task SearchAsync_ResultIncludesIdTitleImageUrlCategoryTagsCreatedAt()
    {
        using var db = CreateInMemoryDb();
        var author = await TestDataSeeder.SeedUserAsync(db);
        var category = await TestDataSeeder.SeedCategoryAsync(db, "Interior");
        var tag = await TestDataSeeder.SeedTagAsync(db, "cozy");
        var pin = await TestDataSeeder.SeedPinAsync(db, author, category, title: "Reading Nook");
        await TestDataSeeder.AttachTagAsync(db, pin, tag);

        var service = new SearchService(db);
        var result = await service.SearchAsync(null, null, null, 1, 10);

        var item = Assert.Single(result.Items);
        Assert.Equal(pin.Id, item.Id);
        Assert.Equal("Reading Nook", item.Title);
        Assert.Equal(pin.ImageUrl, item.ImageUrl);
        Assert.StartsWith("Interior", item.Category);
        Assert.Contains(item.Tags, t => t.StartsWith("cozy"));
        Assert.Equal(pin.CreatedAt, item.CreatedAt);
    }

    // ──────────────────────────────────────────────
    // GetTrendingAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetTrendingAsync_OrdersByLikeCountDescending()
    {
        using var db = CreateInMemoryDb();
        var author = await TestDataSeeder.SeedUserAsync(db);
        var category = await TestDataSeeder.SeedCategoryAsync(db);
        var lessLiked = await TestDataSeeder.SeedPinAsync(db, author, category, title: "Less Liked");
        var moreLiked = await TestDataSeeder.SeedPinAsync(db, author, category, title: "More Liked");

        db.Likes.Add(new Like { PinId = lessLiked.Id, UserId = Guid.NewGuid() });
        db.Likes.Add(new Like { PinId = moreLiked.Id, UserId = Guid.NewGuid() });
        db.Likes.Add(new Like { PinId = moreLiked.Id, UserId = Guid.NewGuid() });
        await db.SaveChangesAsync();

        var service = new SearchService(db);
        var result = await service.GetTrendingAsync(10);

        Assert.Equal("More Liked", result[0].Title);
        Assert.Equal("Less Liked", result[1].Title);
    }

    [Fact]
    public async Task GetTrendingAsync_RespectsCount()
    {
        using var db = CreateInMemoryDb();
        var author = await TestDataSeeder.SeedUserAsync(db);
        var category = await TestDataSeeder.SeedCategoryAsync(db);

        for (var i = 0; i < 5; i++)
        {
            await TestDataSeeder.SeedPinAsync(db, author, category, title: $"Pin {i}");
        }

        var service = new SearchService(db);
        var result = await service.GetTrendingAsync(3);

        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetTrendingAsync_NoPins_ReturnsEmptyList()
    {
        using var db = CreateInMemoryDb();

        var service = new SearchService(db);
        var result = await service.GetTrendingAsync(10);

        Assert.Empty(result);
    }

    // ──────────────────────────────────────────────
    // GetCategoriesAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetCategoriesAsync_ReturnsAllCategoriesOrderedByName()
    {
        using var db = CreateInMemoryDb();
        await TestDataSeeder.SeedCategoryAsync(db, "Zebra");
        await TestDataSeeder.SeedCategoryAsync(db, "Alpha");

        var service = new SearchService(db);
        var result = await service.GetCategoriesAsync();

        Assert.Equal(2, result.Count);
        Assert.True(string.Compare(result[0].Name, result[1].Name, StringComparison.Ordinal) <= 0);
    }
}