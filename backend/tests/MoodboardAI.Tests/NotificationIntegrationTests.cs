using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MoodboardAI.Api.Data;
using MoodboardAI.Api.DTOs.Notification;
using MoodboardAI.Api.Models;
using MoodboardAI.Api.Services;
using Xunit;

namespace MoodboardAI.Tests;

/// <summary>
/// Integration tests for notification list, mark-as-read, and creation-from-action flows.
/// </summary>
public class NotificationIntegrationTests : IClassFixture<NotificationWebApplicationFactory>
{
    private readonly NotificationWebApplicationFactory _factory;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public NotificationIntegrationTests(NotificationWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateFromLikeCommentFollow_PersistsNotifications()
    {
        var recipientId = Guid.NewGuid();
        var actorId = Guid.NewGuid();
        var pinId = Guid.NewGuid();

        using var scope = _factory.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<INotificationService>();

        var like = await service.NotifyLikeAsync(recipientId, actorId, pinId, "Alice");
        var comment = await service.NotifyCommentAsync(recipientId, actorId, pinId, "Alice");
        var follow = await service.NotifyNewFollowerAsync(recipientId, actorId, "Alice");

        Assert.NotNull(like);
        Assert.Equal(NotificationTypeEnum.Like, like!.Type);
        Assert.Equal(actorId, like.ActorId);
        Assert.Equal(pinId, like.RelatedEntityId);

        Assert.NotNull(comment);
        Assert.Equal(NotificationTypeEnum.Comment, comment!.Type);

        Assert.NotNull(follow);
        Assert.Equal(NotificationTypeEnum.NewFollower, follow!.Type);

        // Self-action should not create a notification.
        var selfLike = await service.NotifyLikeAsync(actorId, actorId, pinId, "Alice");
        Assert.Null(selfLike);
    }

    [Fact]
    public async Task GetNotifications_ReturnsUnreadFirst_WithFilterAndUnreadCount()
    {
        var userId = Guid.NewGuid();
        var actorId = Guid.NewGuid();

        using (var scope = _factory.Services.CreateScope())
        {
            var service = scope.ServiceProvider.GetRequiredService<INotificationService>();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var olderUnread = await service.NotifyLikeAsync(userId, actorId, Guid.NewGuid(), "Bob");
            var readLike = await service.NotifyLikeAsync(userId, actorId, Guid.NewGuid(), "Bob");
            var comment = await service.NotifyCommentAsync(userId, actorId, Guid.NewGuid(), "Bob");
            var follow = await service.NotifyNewFollowerAsync(userId, actorId, "Bob");

            Assert.NotNull(olderUnread);
            Assert.NotNull(readLike);
            Assert.NotNull(comment);
            Assert.NotNull(follow);

            // Make the second like older and mark it read so unread-first ordering is visible.
            var readEntity = await db.Notifications.FirstAsync(n => n.Id == readLike!.Id);
            readEntity.IsRead = true;
            readEntity.CreatedAt = DateTime.UtcNow.AddHours(1);

            var olderEntity = await db.Notifications.FirstAsync(n => n.Id == olderUnread!.Id);
            olderEntity.CreatedAt = DateTime.UtcNow.AddHours(-1);

            await db.SaveChangesAsync();
        }

        var client = _factory.CreateAuthenticatedClient(userId);

        var allResponse = await client.GetAsync("/api/notifications?page=1&pageSize=10");
        allResponse.EnsureSuccessStatusCode();
        var all = await allResponse.Content.ReadFromJsonAsync<NotificationListResponseDto>(JsonOptions);

        Assert.NotNull(all);
        Assert.Equal(4, all!.TotalCount);
        Assert.Equal(3, all.UnreadCount);
        Assert.Equal(4, all.Items.Count);
        Assert.All(all.Items.Take(3), n => Assert.False(n.IsRead));
        Assert.True(all.Items.Last().IsRead);

        var filteredResponse = await client.GetAsync("/api/notifications?type=Like");
        filteredResponse.EnsureSuccessStatusCode();
        var filtered = await filteredResponse.Content.ReadFromJsonAsync<NotificationListResponseDto>(JsonOptions);

        Assert.NotNull(filtered);
        Assert.Equal(2, filtered!.TotalCount);
        Assert.Equal(3, filtered.UnreadCount); // global unread badge
        Assert.All(filtered.Items, n => Assert.Equal(NotificationTypeEnum.Like, n.Type));
    }

    [Fact]
    public async Task MarkAsRead_IsIdempotent()
    {
        var userId = Guid.NewGuid();
        Guid notificationId;

        using (var scope = _factory.Services.CreateScope())
        {
            var service = scope.ServiceProvider.GetRequiredService<INotificationService>();
            var created = await service.NotifyLikeAsync(userId, Guid.NewGuid(), Guid.NewGuid(), "Carol");
            Assert.NotNull(created);
            notificationId = created!.Id;
        }

        var client = _factory.CreateAuthenticatedClient(userId);

        var first = await client.PostAsync($"/api/notifications/{notificationId}/read", null);
        Assert.Equal(System.Net.HttpStatusCode.NoContent, first.StatusCode);

        var second = await client.PostAsync($"/api/notifications/{notificationId}/read", null);
        Assert.Equal(System.Net.HttpStatusCode.NoContent, second.StatusCode);

        using var verifyScope = _factory.Services.CreateScope();
        var db = verifyScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var entity = await db.Notifications.FirstAsync(n => n.Id == notificationId);
        Assert.True(entity.IsRead);
    }

    [Fact]
    public async Task MarkAllAsRead_IsIdempotent()
    {
        var userId = Guid.NewGuid();

        using (var scope = _factory.Services.CreateScope())
        {
            var service = scope.ServiceProvider.GetRequiredService<INotificationService>();
            await service.NotifyLikeAsync(userId, Guid.NewGuid(), Guid.NewGuid(), "Dana");
            await service.NotifyCommentAsync(userId, Guid.NewGuid(), Guid.NewGuid(), "Dana");
        }

        var client = _factory.CreateAuthenticatedClient(userId);

        var first = await client.PostAsync("/api/notifications/read-all", null);
        Assert.Equal(System.Net.HttpStatusCode.NoContent, first.StatusCode);

        var second = await client.PostAsync("/api/notifications/read-all", null);
        Assert.Equal(System.Net.HttpStatusCode.NoContent, second.StatusCode);

        var listResponse = await client.GetAsync("/api/notifications");
        listResponse.EnsureSuccessStatusCode();
        var list = await listResponse.Content.ReadFromJsonAsync<NotificationListResponseDto>(JsonOptions);

        Assert.NotNull(list);
        Assert.Equal(0, list!.UnreadCount);
        Assert.All(list.Items, n => Assert.True(n.IsRead));
    }

    [Fact]
    public async Task MarkAsRead_OtherUsersNotification_ReturnsNotFound()
    {
        var ownerId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();
        Guid notificationId;

        using (var scope = _factory.Services.CreateScope())
        {
            var service = scope.ServiceProvider.GetRequiredService<INotificationService>();
            var created = await service.NotifyLikeAsync(ownerId, Guid.NewGuid(), Guid.NewGuid(), "Eve");
            Assert.NotNull(created);
            notificationId = created!.Id;
        }

        var client = _factory.CreateAuthenticatedClient(otherUserId);
        var response = await client.PostAsync($"/api/notifications/{notificationId}/read", null);

        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }
}

/// <summary>
/// WebApplicationFactory that swaps PostgreSQL for an in-memory database and
/// runs under the Testing environment so startup migrations are skipped.
/// </summary>
public class NotificationWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName = $"NotificationsTests-{Guid.NewGuid()}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(_databaseName));
        });
    }

    /// <summary>
    /// Creates an HTTP client authenticated as the given user via a signed test JWT.
    /// </summary>
    public HttpClient CreateAuthenticatedClient(Guid userId, string email = "test@example.com")
    {
        var client = CreateClient();
        var token = CreateTestJwt(userId, email);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    private static string CreateTestJwt(Guid userId, string email)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("change-this-secret-key-in-production-min32chars"));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "MoodboardAI",
            audience: "MoodboardAI",
            claims:
            [
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            ],
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
