using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using MoodboardAI.Api.DTOs.Search;
using MoodboardAI.Tests.TestSupport;
using Xunit;

namespace MoodboardAI.Tests;

/// <summary>
/// Integration tests for <see cref="MoodboardAI.Api.Controllers.RecentSearchesController"/>.
/// </summary>
public class RecentSearchesControllerIntegrationTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly ApiWebApplicationFactory _factory;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public RecentSearchesControllerIntegrationTests(ApiWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task AddRecentSearch_EmptyQuery_ReturnsBadRequest()
    {
        var client = _factory.CreateAuthenticatedClient(Guid.NewGuid());

        var response = await client.PostAsJsonAsync("/api/users/me/recent-searches", new AddRecentSearchRequestDto
        {
            Query = string.Empty
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task AddRecentSearch_WithoutToken_ReturnsUnauthorized()
    {
        var client = _factory.CreateAnonymousClient();

        var response = await client.PostAsJsonAsync("/api/users/me/recent-searches", new AddRecentSearchRequestDto
        {
            Query = "minimal interior"
        });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task AddRecentSearch_SameQueryTwice_DoesNotCreateDuplicate()
    {
        var client = _factory.CreateAuthenticatedClient(Guid.NewGuid());

        var first = await client.PostAsJsonAsync("/api/users/me/recent-searches", new AddRecentSearchRequestDto
        {
            Query = "japanese style"
        });
        first.EnsureSuccessStatusCode();

        var second = await client.PostAsJsonAsync("/api/users/me/recent-searches", new AddRecentSearchRequestDto
        {
            Query = "japanese style"
        });
        second.EnsureSuccessStatusCode();

        var listResponse = await client.GetAsync("/api/users/me/recent-searches");
        listResponse.EnsureSuccessStatusCode();
        var searches = await listResponse.Content.ReadFromJsonAsync<List<RecentSearchDto>>(JsonOptions);

        Assert.NotNull(searches);
        Assert.Single(searches!);
        Assert.Equal("japanese style", searches![0].Query);
    }

    [Fact]
    public async Task GetRecentSearches_ReturnsNewestFirst()
    {
        var client = _factory.CreateAuthenticatedClient(Guid.NewGuid());

        (await client.PostAsJsonAsync("/api/users/me/recent-searches",
            new AddRecentSearchRequestDto { Query = "first query" })).EnsureSuccessStatusCode();
        (await client.PostAsJsonAsync("/api/users/me/recent-searches",
            new AddRecentSearchRequestDto { Query = "second query" })).EnsureSuccessStatusCode();

        var listResponse = await client.GetAsync("/api/users/me/recent-searches");
        listResponse.EnsureSuccessStatusCode();
        var searches = await listResponse.Content.ReadFromJsonAsync<List<RecentSearchDto>>(JsonOptions);

        Assert.NotNull(searches);
        Assert.Equal(2, searches!.Count);
        Assert.Equal("second query", searches[0].Query);
        Assert.Equal("first query", searches[1].Query);
    }

    [Fact]
    public async Task ClearRecentSearches_RemovesAllEntriesForUser()
    {
        var client = _factory.CreateAuthenticatedClient(Guid.NewGuid());

        (await client.PostAsJsonAsync("/api/users/me/recent-searches",
            new AddRecentSearchRequestDto { Query = "to be cleared" })).EnsureSuccessStatusCode();

        var clearResponse = await client.DeleteAsync("/api/users/me/recent-searches");
        clearResponse.EnsureSuccessStatusCode();

        var listResponse = await client.GetAsync("/api/users/me/recent-searches");
        listResponse.EnsureSuccessStatusCode();
        var searches = await listResponse.Content.ReadFromJsonAsync<List<RecentSearchDto>>(JsonOptions);

        Assert.NotNull(searches);
        Assert.Empty(searches!);
    }
}
