using Microsoft.EntityFrameworkCore;
using MoodboardAI.Api.Data;
using MoodboardAI.Api.DTOs.Users;
using MoodboardAI.Api.Models;
using MoodboardAI.Api.Services;
using Xunit;

namespace MoodboardAI.Tests;

/// <summary>
/// Integration-style tests for <see cref="UserService"/> against an in-memory
/// ApplicationDbContext, covering profile reads (with interests/privacy) and updates.
/// </summary>
public class UserServiceTests
{
    private static ApplicationDbContext CreateInMemoryDb()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new ApplicationDbContext(options);
    }

    private static UserEntity CreateUser(ApplicationDbContext db, string email = "user@example.com")
    {
        var user = new UserEntity
        {
            FullName = "Test User",
            Email = email,
            Username = "testuser",
            PasswordHash = "hash"
        };
        db.Users.Add(user);
        db.SaveChanges();
        return user;
    }

    [Fact]
    public void GetCurrentUser_UnknownUser_ReturnsNull()
    {
        using var db = CreateInMemoryDb();
        var service = new UserService(db);

        var profile = service.GetCurrentUser(Guid.NewGuid().ToString());

        Assert.Null(profile);
    }

    [Fact]
    public void GetCurrentUser_NoPrivacySettings_ReturnsDefaults()
    {
        using var db = CreateInMemoryDb();
        var user = CreateUser(db);
        var service = new UserService(db);

        var profile = service.GetCurrentUser(user.Id.ToString());

        Assert.NotNull(profile);
        Assert.False(profile!.Privacy.PrivateAccount);
        Assert.True(profile.Privacy.SearchVisibility);
        Assert.True(profile.Privacy.ContentVisibility);
    }

    [Fact]
    public void GetCurrentUser_WithPrivacySettingsAndInterests_ReturnsThem()
    {
        using var db = CreateInMemoryDb();
        var user = CreateUser(db);

        db.UserPrivacySettings.Add(new UserPrivacySettings
        {
            UserId = user.Id,
            PrivateAccount = true,
            SearchVisibility = false,
            ContentVisibility = true
        });

        var interest = new Interest { Name = "Minimal", Icon = "minimal" };
        db.Interests.Add(interest);
        db.UserInterests.Add(new UserInterest { UserId = user.Id, InterestId = interest.Id });
        db.SaveChanges();

        var service = new UserService(db);
        var profile = service.GetCurrentUser(user.Id.ToString());

        Assert.NotNull(profile);
        Assert.True(profile!.Privacy.PrivateAccount);
        Assert.False(profile.Privacy.SearchVisibility);
        Assert.Single(profile.SelectedInterests);
        Assert.Equal("Minimal", profile.SelectedInterests[0].Name);
    }

    [Fact]
    public async Task UpdateCurrentUserAsync_ValidRequest_UpdatesAllFields()
    {
        using var db = CreateInMemoryDb();
        var user = CreateUser(db);
        var service = new UserService(db);

        var result = await service.UpdateCurrentUserAsync(user.Id.ToString(), new UpdateUserProfileDto
        {
            FullName = "New Name",
            Email = "new@example.com",
            AvatarUrl = "https://example.com/avatar.jpg"
        });

        Assert.True(result.Succeeded);
        Assert.Equal("New Name", result.Profile!.FullName);
        Assert.Equal("new@example.com", result.Profile.Email);
        Assert.Equal("https://example.com/avatar.jpg", result.Profile.AvatarUrl);
    }

    [Fact]
    public async Task UpdateCurrentUserAsync_PartialUpdate_OnlyChangesProvidedFields()
    {
        using var db = CreateInMemoryDb();
        var user = CreateUser(db);
        var service = new UserService(db);

        var result = await service.UpdateCurrentUserAsync(user.Id.ToString(), new UpdateUserProfileDto
        {
            FullName = "Only Name Changed"
        });

        Assert.True(result.Succeeded);
        Assert.Equal("Only Name Changed", result.Profile!.FullName);
        Assert.Equal("user@example.com", result.Profile.Email);
    }

    [Fact]
    public async Task UpdateCurrentUserAsync_DuplicateEmail_Fails()
    {
        using var db = CreateInMemoryDb();
        var user = CreateUser(db, "user@example.com");
        CreateUser(db, "taken@example.com");
        var service = new UserService(db);

        var result = await service.UpdateCurrentUserAsync(user.Id.ToString(), new UpdateUserProfileDto
        {
            Email = "taken@example.com"
        });

        Assert.False(result.Succeeded);
        Assert.Contains("already exists", result.ErrorMessage, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task UpdateCurrentUserAsync_UnknownUser_Fails()
    {
        using var db = CreateInMemoryDb();
        var service = new UserService(db);

        var result = await service.UpdateCurrentUserAsync(Guid.NewGuid().ToString(), new UpdateUserProfileDto
        {
            FullName = "Doesn't Matter"
        });

        Assert.False(result.Succeeded);
        Assert.Equal("User not found.", result.ErrorMessage);
    }

    [Fact]
    public async Task UpdateCurrentUserAsync_EmptyAvatarUrl_ClearsAvatar()
    {
        using var db = CreateInMemoryDb();
        var user = CreateUser(db);
        user.AvatarUrl = "https://example.com/old.jpg";
        db.SaveChanges();

        var service = new UserService(db);

        var result = await service.UpdateCurrentUserAsync(user.Id.ToString(), new UpdateUserProfileDto
        {
            AvatarUrl = ""
        });

        Assert.True(result.Succeeded);
        Assert.Null(result.Profile!.AvatarUrl);
    }
}