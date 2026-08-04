using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoodboardAI.Api.Data;
using MoodboardAI.Api.DTOs.Privacy;
using MoodboardAI.Tests.TestSupport;
using Xunit;

namespace MoodboardAI.Tests;

/// <summary>
/// Integration tests for <see cref="MoodboardAI.Api.Controllers.PrivacyController"/>:
/// privacy settings and the blocked-users flow.
/// </summary>
public class PrivacyControllerIntegrationTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly ApiWebApplicationFactory _factory;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public PrivacyControllerIntegrationTests(ApiWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetPrivacySettings_WhenNoneSaved_ReturnsDefaults()
    {
        var client = _factory.CreateAuthenticatedClient(Guid.NewGuid());

        var response = await client.GetAsync("/api/users/me/privacy");
        response.EnsureSuccessStatusCode();

        var settings = await response.Content.ReadFromJsonAsync<PrivacySettingsDto>(JsonOptions);

        Assert.NotNull(settings);
        Assert.False(settings!.PrivateAccount);
        Assert.True(settings.SearchVisibility);
        Assert.True(settings.ContentVisibility);
    }

    [Fact]
    public async Task UpdatePrivacySettings_PersistsChanges()
    {
        var userId = Guid.NewGuid();
        var client = _factory.CreateAuthenticatedClient(userId);

        var updateResponse = await client.PutAsJsonAsync("/api/users/me/privacy", new UpdatePrivacySettingsDto
        {
            PrivateAccount = true,
            SearchVisibility = false,
            ContentVisibility = false
        });
        updateResponse.EnsureSuccessStatusCode();

        var getResponse = await client.GetAsync("/api/users/me/privacy");
        getResponse.EnsureSuccessStatusCode();
        var settings = await getResponse.Content.ReadFromJsonAsync<PrivacySettingsDto>(JsonOptions);

        Assert.NotNull(settings);
        Assert.True(settings!.PrivateAccount);
        Assert.False(settings.SearchVisibility);
        Assert.False(settings.ContentVisibility);
    }

    [Fact]
    public async Task BlockUser_Self_ReturnsBadRequest()
    {
        var userId = Guid.NewGuid();
        var client = _factory.CreateAuthenticatedClient(userId);

        var response = await client.PostAsJsonAsync("/api/users/me/blocked-users", new BlockUserRequestDto
        {
            UserId = userId
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task BlockUser_UnknownUser_ReturnsNotFound()
    {
        var client = _factory.CreateAuthenticatedClient(Guid.NewGuid());

        var response = await client.PostAsJsonAsync("/api/users/me/blocked-users", new BlockUserRequestDto
        {
            UserId = Guid.NewGuid()
        });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task BlockUser_ThenListAndUnblock_Works()
    {
        var blockerId = Guid.NewGuid();
        Guid targetId;

        using (var scope = _factory.CreateDbScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var target = await TestDataSeeder.SeedUserAsync(db, fullName: "Target User");
            targetId = target.Id;
        }

        var client = _factory.CreateAuthenticatedClient(blockerId);

        var blockResponse = await client.PostAsJsonAsync("/api/users/me/blocked-users", new BlockUserRequestDto
        {
            UserId = targetId
        });
        Assert.Equal(HttpStatusCode.OK, blockResponse.StatusCode);

        // Blocking again should be rejected — duplicates are not allowed.
        var duplicateResponse = await client.PostAsJsonAsync("/api/users/me/blocked-users", new BlockUserRequestDto
        {
            UserId = targetId
        });
        Assert.Equal(HttpStatusCode.BadRequest, duplicateResponse.StatusCode);

        var listResponse = await client.GetAsync("/api/users/me/blocked-users");
        listResponse.EnsureSuccessStatusCode();
        var blockedList = await listResponse.Content.ReadFromJsonAsync<List<BlockedUserDto>>(JsonOptions);

        Assert.NotNull(blockedList);
        Assert.Single(blockedList!);
        Assert.Equal(targetId, blockedList![0].BlockedUserId);

        var unblockResponse = await client.DeleteAsync($"/api/users/me/blocked-users/{targetId}");
        Assert.Equal(HttpStatusCode.OK, unblockResponse.StatusCode);

        var listAfterUnblockResponse = await client.GetAsync("/api/users/me/blocked-users");
        listAfterUnblockResponse.EnsureSuccessStatusCode();
        var listAfterUnblock = await listAfterUnblockResponse.Content.ReadFromJsonAsync<List<BlockedUserDto>>(JsonOptions);

        Assert.NotNull(listAfterUnblock);
        Assert.Empty(listAfterUnblock!);
    }

    [Fact]
    public async Task UnblockUser_NotBlocked_ReturnsNotFound()
    {
        var client = _factory.CreateAuthenticatedClient(Guid.NewGuid());

        var response = await client.DeleteAsync($"/api/users/me/blocked-users/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
