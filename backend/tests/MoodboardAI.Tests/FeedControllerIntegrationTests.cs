using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoodboardAI.Api.Data;
using MoodboardAI.Api.Models;
using MoodboardAI.Tests.TestSupport;
using Xunit;

namespace MoodboardAI.Tests;

/// <summary>
/// Integration tests for <see cref="MoodboardAI.Api.Controllers.FeedController"/>.
/// </summary>
/// <remarks>
/// Unlike the other controller test suites, this class does <b>not</b> share
/// an <see cref="ApiWebApplicationFactory"/> via <c>IClassFixture</c>: the
/// feed endpoint returns pins globally (not scoped to a single user), so
/// assertions here check total/ordered results — sharing one in-memory
/// database across test methods would let pins seeded by one test leak into
/// another test's counts. Each test method gets its own fresh factory (and
/// therefore its own isolated in-memory database) instead.
/// </remarks>
public class FeedControllerIntegrationTests : IDisposable
{
    private readonly ApiWebApplicationFactory _factory = new();

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public void Dispose() => _factory.Dispose();

    private record FeedItemDto(Guid Id, string Title, string Category, string Author, DateTime CreatedAt);
    private record FeedResponseDto(int TotalCount, int Page, int PageSize, List<FeedItemDto> Items);

    [Fact]
    public async Task GetFeed_InvalidPagination_ReturnsBadRequest()
    {
        var client = _factory.CreateAnonymousClient();

        var response = await client.GetAsync("/api/feed?page=0");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetFeed_ReturnsPinsNewestFirst_WithTotalCount()
    {
        using (var scope = _factory.CreateDbScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var author = await TestDataSeeder.SeedUserAsync(db);
            var category = await TestDataSeeder.SeedCategoryAsync(db);

            db.Pins.AddRange(
                new Pin
                {
                    Title = "Older Pin",
                    ImageUrl = "https://example.com/older.png",
                    AuthorId = author.Id,
                    CategoryId = category.Id,
                    CreatedAt = DateTime.UtcNow.AddHours(-2)
                },
                new Pin
                {
                    Title = "Newer Pin",
                    ImageUrl = "https://example.com/newer.png",
                    AuthorId = author.Id,
                    CategoryId = category.Id,
                    CreatedAt = DateTime.UtcNow.AddHours(-1)
                });

            await db.SaveChangesAsync();
        }

        var client = _factory.CreateAnonymousClient();
        var response = await client.GetAsync("/api/feed?page=1&pageSize=10");
        response.EnsureSuccessStatusCode();

        var feed = await response.Content.ReadFromJsonAsync<FeedResponseDto>(JsonOptions);

        Assert.NotNull(feed);
        Assert.Equal(2, feed!.TotalCount);
        Assert.Equal(2, feed.Items.Count);
        Assert.Equal("Newer Pin", feed.Items[0].Title);
        Assert.Equal("Older Pin", feed.Items[1].Title);
    }

    [Fact]
    public async Task GetFeed_FiltersByCategory()
    {
        Guid targetCategoryId;

        using (var scope = _factory.CreateDbScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var author = await TestDataSeeder.SeedUserAsync(db);
            var categoryA = await TestDataSeeder.SeedCategoryAsync(db, "CategoryA");
            var categoryB = await TestDataSeeder.SeedCategoryAsync(db, "CategoryB");
            targetCategoryId = categoryA.Id;

            db.Pins.AddRange(
                new Pin
                {
                    Title = "In Category A",
                    ImageUrl = "https://example.com/a.png",
                    AuthorId = author.Id,
                    CategoryId = categoryA.Id
                },
                new Pin
                {
                    Title = "In Category B",
                    ImageUrl = "https://example.com/b.png",
                    AuthorId = author.Id,
                    CategoryId = categoryB.Id
                });

            await db.SaveChangesAsync();
        }

        var client = _factory.CreateAnonymousClient();
        var response = await client.GetAsync($"/api/feed?categoryId={targetCategoryId}");
        response.EnsureSuccessStatusCode();

        var feed = await response.Content.ReadFromJsonAsync<FeedResponseDto>(JsonOptions);

        Assert.NotNull(feed);
        Assert.Equal(1, feed!.TotalCount);
        Assert.Single(feed.Items);
        Assert.Equal("In Category A", feed.Items[0].Title);
    }

    [Fact]
    public async Task GetFeed_RespectsPageSize()
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
        var response = await client.GetAsync("/api/feed?page=1&pageSize=2");
        response.EnsureSuccessStatusCode();

        var feed = await response.Content.ReadFromJsonAsync<FeedResponseDto>(JsonOptions);

        Assert.NotNull(feed);
        Assert.Equal(5, feed!.TotalCount);
        Assert.Equal(2, feed.Items.Count);
    }
}
