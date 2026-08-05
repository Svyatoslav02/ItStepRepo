using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoodboardAI.Api.Data;
using MoodboardAI.Tests.TestSupport;
using Xunit;

namespace MoodboardAI.Tests;

/// <summary>
/// Integration tests for <see cref="MoodboardAI.Api.Controllers.PinsController"/>.
/// </summary>
public class PinsControllerIntegrationTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly ApiWebApplicationFactory _factory;

    public PinsControllerIntegrationTests(ApiWebApplicationFactory factory)
    {
        _factory = factory;
    }

    private async Task<Guid> SeedPinAsync()
    {
        using var scope = _factory.CreateDbScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var pin = await TestDataSeeder.SeedPinAsync(db);
        return pin.Id;
    }

    [Fact]
    public async Task GetPinById_ExistingPin_ReturnsOk()
    {
        var pinId = await SeedPinAsync();
        var client = _factory.CreateAnonymousClient();

        var response = await client.GetAsync($"/api/pins/{pinId}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetPinById_UnknownPin_ReturnsNotFound()
    {
        var client = _factory.CreateAnonymousClient();

        var response = await client.GetAsync($"/api/pins/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task LikePin_ThenDuplicateLike_ReturnsBadRequest()
    {
        var pinId = await SeedPinAsync();
        var client = _factory.CreateAuthenticatedClient(Guid.NewGuid());

        var first = await client.PostAsync($"/api/pins/{pinId}/like", null);
        Assert.Equal(HttpStatusCode.OK, first.StatusCode);

        var second = await client.PostAsync($"/api/pins/{pinId}/like", null);
        Assert.Equal(HttpStatusCode.BadRequest, second.StatusCode);
    }

    [Fact]
    public async Task LikePin_UnknownPin_ReturnsNotFound()
    {
        var client = _factory.CreateAuthenticatedClient(Guid.NewGuid());

        var response = await client.PostAsync($"/api/pins/{Guid.NewGuid()}/like", null);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task LikeThenUnlikePin_RemovesLike_AllowingReLike()
    {
        var pinId = await SeedPinAsync();
        var client = _factory.CreateAuthenticatedClient(Guid.NewGuid());

        (await client.PostAsync($"/api/pins/{pinId}/like", null)).EnsureSuccessStatusCode();

        var unlike = await client.DeleteAsync($"/api/pins/{pinId}/like");
        Assert.Equal(HttpStatusCode.OK, unlike.StatusCode);

        // Having unliked, liking again should succeed rather than be treated as a duplicate.
        var reLike = await client.PostAsync($"/api/pins/{pinId}/like", null);
        Assert.Equal(HttpStatusCode.OK, reLike.StatusCode);
    }

    [Fact]
    public async Task UnlikePin_NotLiked_ReturnsNotFound()
    {
        var pinId = await SeedPinAsync();
        var client = _factory.CreateAuthenticatedClient(Guid.NewGuid());

        var response = await client.DeleteAsync($"/api/pins/{pinId}/like");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task SavePin_ThenDuplicateSave_ReturnsBadRequest()
    {
        var pinId = await SeedPinAsync();
        var client = _factory.CreateAuthenticatedClient(Guid.NewGuid());

        var first = await client.PostAsync($"/api/pins/{pinId}/save", null);
        Assert.Equal(HttpStatusCode.OK, first.StatusCode);

        var second = await client.PostAsync($"/api/pins/{pinId}/save", null);
        Assert.Equal(HttpStatusCode.BadRequest, second.StatusCode);
    }

    [Fact]
    public async Task SaveThenUnsavePin_RemovesSave()
    {
        var pinId = await SeedPinAsync();
        var client = _factory.CreateAuthenticatedClient(Guid.NewGuid());

        (await client.PostAsync($"/api/pins/{pinId}/save", null)).EnsureSuccessStatusCode();

        var unsave = await client.DeleteAsync($"/api/pins/{pinId}/save");
        Assert.Equal(HttpStatusCode.OK, unsave.StatusCode);

        var unsaveAgain = await client.DeleteAsync($"/api/pins/{pinId}/save");
        Assert.Equal(HttpStatusCode.NotFound, unsaveAgain.StatusCode);
    }

    [Fact]
    public async Task LikePin_WithoutToken_ReturnsUnauthorized()
    {
        var pinId = await SeedPinAsync();
        var client = _factory.CreateAnonymousClient();

        var response = await client.PostAsync($"/api/pins/{pinId}/like", null);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
