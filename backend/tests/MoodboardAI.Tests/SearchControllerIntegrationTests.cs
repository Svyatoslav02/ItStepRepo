using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoodboardAI.Api.Data;
using MoodboardAI.Api.DTOs.Search;
using MoodboardAI.Tests.TestSupport;
using Xunit;

namespace MoodboardAI.Tests;

/// <summary>
/// Integration tests for <see cref="MoodboardAI.Api.Controllers.SearchController"/>.
/// </summary>
/// <remarks>
/// Like <c>FeedControllerIntegrationTests</c>, this suite does <b>not</b>
/// share an <see cref="ApiWebApplicationFactory"/> via <c>IClassFixture</c>:
/// search results are global (not scoped to a single user), so sharing one
/// in-memory database across test methods would let pins seeded by one test
/// leak into another test's counts. Each test method gets its own isolated factory.
/// </remarks>
public class SearchControllerIntegrationTests : IDisposable
{
    private readonly ApiWebApplicationFactory _factory = new();

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public void Dispose() => _factory.Dispose();

    [Fact]
    public async Task Search_WithoutQuery_ReturnsAllPins()
    {
        using (var scope = _factory.CreateDbScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var author = await TestDataSeeder.SeedUserAsync(db);
            var category = await TestDataSeeder.SeedCategoryAsync(db);
            await TestDataSeeder.SeedPinAsync(db, author, category, title: "Pin One");
            await TestDataSeeder.SeedPinAsync(db, author, category, title: "Pin Two");
        }

        var client = _factory.CreateAnonymousClient();
        var response = await client.GetAsync("/api/search");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<SearchResponseDto>(JsonOptions);

        Assert.NotNull(result);
        Assert.Equal(2, result!.TotalCount);
    }

    [Fact]
    public async Task Search_WithQuery_FiltersResults()
    {
        using (var scope = _factory.CreateDbScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var author = await TestDataSeeder.SeedUserAsync(db);
            var category = await TestDataSeeder.SeedCategoryAsync(db);
            await TestDataSeeder.SeedPinAsync(db, author, category, title: "Minimalist Desk");
            await TestDataSeeder.SeedPinAsync(db, author, category, title: "Vintage Chair");
        }

        var client = _factory.CreateAnonymousClient();
        var response = await client.GetAsync("/api/search?q=minimalist");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<SearchResponseDto>(JsonOptions);

        Assert.NotNull(result);
        Assert.Equal(1, result!.TotalCount);
        Assert.Equal("Minimalist Desk", result.Items[0].Title);
    }

    [Fact]
    public async Task Search_NoMatches_ReturnsEmptyList()
    {
        using (var scope = _factory.CreateDbScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var author = await TestDataSeeder.SeedUserAsync(db);
            var category = await TestDataSeeder.SeedCategoryAsync(db);
            await TestDataSeeder.SeedPinAsync(db, author, category, title: "Some Pin");
        }

        var client = _factory.CreateAnonymousClient();
        var response = await client.GetAsync("/api/search?q=nonexistent-xyz");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<SearchResponseDto>(JsonOptions);

        Assert.NotNull(result);
        Assert.Equal(0, result!.TotalCount);
        Assert.Empty(result.Items);
    }

    [Fact]
    public async Task Search_FiltersByCategoryId()
    {
        Guid targetCategoryId;

        using (var scope = _factory.CreateDbScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var author = await TestDataSeeder.SeedUserAsync(db);
            var categoryA = await TestDataSeeder.SeedCategoryAsync(db, "CategoryA");
            var categoryB = await TestDataSeeder.SeedCategoryAsync(db, "CategoryB");
            targetCategoryId = categoryA.Id;

            await TestDataSeeder.SeedPinAsync(db, author, categoryA, title: "In A");
            await TestDataSeeder.SeedPinAsync(db, author, categoryB, title: "In B");
        }

        var client = _factory.CreateAnonymousClient();
        var response = await client.GetAsync($"/api/search?categoryId={targetCategoryId}");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<SearchResponseDto>(JsonOptions);

        Assert.NotNull(result);
        Assert.Equal(1, result!.TotalCount);
        Assert.Equal("In A", result.Items[0].Title);
    }

    [Fact]
    public async Task Search_FiltersByTagId()
    {
        Guid targetTagId;

        using (var scope = _factory.CreateDbScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var author = await TestDataSeeder.SeedUserAsync(db);
            var category = await TestDataSeeder.SeedCategoryAsync(db);
            var tagA = await TestDataSeeder.SeedTagAsync(db, "tagA");
            var tagB = await TestDataSeeder.SeedTagAsync(db, "tagB");
            targetTagId = tagA.Id;

            var pinWithTagA = await TestDataSeeder.SeedPinAsync(db, author, category, title: "Tagged A");
            await TestDataSeeder.AttachTagAsync(db, pinWithTagA, tagA);

            var pinWithTagB = await TestDataSeeder.SeedPinAsync(db, author, category, title: "Tagged B");
            await TestDataSeeder.AttachTagAsync(db, pinWithTagB, tagB);
        }

        var client = _factory.CreateAnonymousClient();
        var response = await client.GetAsync($"/api/search?tagId={targetTagId}");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<SearchResponseDto>(JsonOptions);

        Assert.NotNull(result);
        Assert.Equal(1, result!.TotalCount);
        Assert.Equal("Tagged A", result.Items[0].Title);
    }

    [Fact]
    public async Task Search_RespectsPagination()
    {
        using (var scope = _factory.CreateDbScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var author = await TestDataSeeder.SeedUserAsync(db);
            var category = await TestDataSeeder.SeedCategoryAsync(db);

            for (var i = 0; i < 5; i++)
            {
                await TestDataSeeder.SeedPinAsync(db, author, category, title: $"Pin {i}");
            }
        }

        var client = _factory.CreateAnonymousClient();
        var response = await client.GetAsync("/api/search?page=1&pageSize=2");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<SearchResponseDto>(JsonOptions);

        Assert.NotNull(result);
        Assert.Equal(5, result!.TotalCount);
        Assert.Equal(2, result.Items.Count);
    }

    [Fact]
    public async Task Trending_ReturnsOk()
    {
        using (var scope = _factory.CreateDbScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var author = await TestDataSeeder.SeedUserAsync(db);
            var category = await TestDataSeeder.SeedCategoryAsync(db);
            await TestDataSeeder.SeedPinAsync(db, author, category, title: "Trending Pin");
        }

        var client = _factory.CreateAnonymousClient();
        var response = await client.GetAsync("/api/search/trending");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<SearchResultDto>>(JsonOptions);

        Assert.NotNull(result);
        Assert.Single(result!);
    }

    [Fact]
    public async Task Trending_NoPins_ReturnsEmptyList()
    {
        var client = _factory.CreateAnonymousClient();
        var response = await client.GetAsync("/api/search/trending");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<SearchResultDto>>(JsonOptions);

        Assert.NotNull(result);
        Assert.Empty(result!);
    }

    [Fact]
    public async Task Categories_ReturnsAllCategories()
    {
        using (var scope = _factory.CreateDbScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await TestDataSeeder.SeedCategoryAsync(db, "Interior");
            await TestDataSeeder.SeedCategoryAsync(db, "Fashion");
        }

        var client = _factory.CreateAnonymousClient();
        var response = await client.GetAsync("/api/search/categories");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<CategoryDto>>(JsonOptions);

        Assert.NotNull(result);
        Assert.Equal(2, result!.Count);
    }

    [Fact]
    public async Task Search_DoesNotRequireAuthentication()
    {
        var client = _factory.CreateAnonymousClient();

        var response = await client.GetAsync("/api/search");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}