using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoodboardAI.Api.Data;
using MoodboardAI.Api.DTOs.Users;
using MoodboardAI.Tests.TestSupport;
using Xunit;

namespace MoodboardAI.Tests;

/// <summary>
/// Integration tests for <c>GET /api/users/me</c>.
/// </summary>
public class UsersControllerIntegrationTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly ApiWebApplicationFactory _factory;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public UsersControllerIntegrationTests(ApiWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetMe_ForExistingUser_ReturnsProfile()
    {
        Guid userId;

        using (var scope = _factory.CreateDbScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var user = await TestDataSeeder.SeedUserAsync(db, email: "profile-owner@example.com", fullName: "Profile Owner");
            userId = user.Id;
        }

        var client = _factory.CreateAuthenticatedClient(userId, "profile-owner@example.com");

        var response = await client.GetAsync("/api/users/me");
        response.EnsureSuccessStatusCode();

        var profile = await response.Content.ReadFromJsonAsync<UserProfileDto>(JsonOptions);

        Assert.NotNull(profile);
        Assert.Equal(userId.ToString(), profile!.Id);
        Assert.Equal("profile-owner@example.com", profile.Email);
        Assert.Equal("Profile Owner", profile.FullName);
    }

    [Fact]
    public async Task GetMe_WithoutToken_ReturnsUnauthorized()
    {
        var client = _factory.CreateAnonymousClient();

        var response = await client.GetAsync("/api/users/me");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetMe_TokenForUnknownUser_ReturnsNotFound()
    {
        var client = _factory.CreateAuthenticatedClient(Guid.NewGuid());

        var response = await client.GetAsync("/api/users/me");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
