using Microsoft.EntityFrameworkCore;
using MoodboardAI.Api.Data;
using MoodboardAI.Api.DTOs.Auth;
using MoodboardAI.Api.Models;
using MoodboardAI.Api.Services;
using Xunit;
using NSubstitute;

namespace MoodboardAI.Tests;

internal static class DbHelper
{
    public static ApplicationDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new ApplicationDbContext(options);
    }

    public static AuthService CreateAuthService(ApplicationDbContext db)
    {
        var jwt = Substitute.For<IJwtTokenService>();
        jwt.GenerateToken(Arg.Any<string>(), Arg.Any<string>()).Returns("test-jwt-token");
        return new AuthService(db, new PasswordHasher(), jwt);
    }

    public static RegisterRequestDto ValidRegister(string email = "user@example.com") => new()
    {
        FullName = "Test User",
        Email = email,
        Password = "SecurePass123"
    };

    public static LoginRequestDto ValidLogin(string email = "user@example.com") => new()
    {
        Email = email,
        Password = "SecurePass123"
    };

    public static async Task<UserEntity> SeedUserAsync(ApplicationDbContext db, string email = "user@example.com")
    {
        await CreateAuthService(db).RegisterAsync(ValidRegister(email));
        return await db.Users.FirstAsync(u => u.Email == email);
    }

    public static async Task<List<Guid>> SeedInterestsAsync(ApplicationDbContext db, int count = 5)
    {
        var interests = Enumerable.Range(1, count).Select(i => new Interest
        {
            Id = Guid.NewGuid(),
            Name = $"Interest{i}",
            Icon = $"icon{i}"
        }).ToList();

        db.Interests.AddRange(interests);
        await db.SaveChangesAsync();
        return interests.Select(i => i.Id).ToList();
    }
}

// ──────────────────────────────────────────────────────────────────────────────
// Auth tests
// ──────────────────────────────────────────────────────────────────────────────

public class AuthRegistrationTests
{
    [Fact]
    public async Task Register_ValidRequest_Succeeds()
    {
        using var db = DbHelper.CreateDb();
        var result = await DbHelper.CreateAuthService(db).RegisterAsync(DbHelper.ValidRegister());

        Assert.True(result.Succeeded);
        Assert.Equal("test-jwt-token", result.Token);
        Assert.NotNull(result.User);
        Assert.Equal("user@example.com", result.User!.Email);
    }

    [Fact]
    public async Task Register_DuplicateEmail_Fails()
    {
        using var db = DbHelper.CreateDb();
        var svc = DbHelper.CreateAuthService(db);

        await svc.RegisterAsync(DbHelper.ValidRegister());
        var result = await svc.RegisterAsync(DbHelper.ValidRegister());

        Assert.False(result.Succeeded);
        Assert.Contains("already exists", result.ErrorMessage, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Register_PasswordIsStoredAsHash()
    {
        using var db = DbHelper.CreateDb();
        await DbHelper.CreateAuthService(db).RegisterAsync(DbHelper.ValidRegister());

        var user = await db.Users.FirstAsync();
        Assert.NotEqual("SecurePass123", user.PasswordHash);
        Assert.False(string.IsNullOrWhiteSpace(user.PasswordHash));
    }
}

public class AuthLoginTests
{
    [Fact]
    public async Task Login_ValidCredentials_Succeeds()
    {
        using var db = DbHelper.CreateDb();
        var svc = DbHelper.CreateAuthService(db);

        await svc.RegisterAsync(DbHelper.ValidRegister());
        var result = await svc.LoginAsync(DbHelper.ValidLogin());

        Assert.True(result.Succeeded);
        Assert.Equal("test-jwt-token", result.Token);
        Assert.NotNull(result.User);
    }

    [Fact]
    public async Task Login_InvalidPassword_Fails()
    {
        using var db = DbHelper.CreateDb();
        var svc = DbHelper.CreateAuthService(db);

        await svc.RegisterAsync(DbHelper.ValidRegister());
        var result = await svc.LoginAsync(new LoginRequestDto
        {
            Email = "user@example.com",
            Password = "WrongPassword!"
        });

        Assert.False(result.Succeeded);
        Assert.NotNull(result.ErrorMessage);
    }

    [Fact]
    public async Task Login_InvalidEmail_Fails()
    {
        using var db = DbHelper.CreateDb();
        var svc = DbHelper.CreateAuthService(db);

        await svc.RegisterAsync(DbHelper.ValidRegister());
        var result = await svc.LoginAsync(new LoginRequestDto
        {
            Email = "nobody@example.com",
            Password = "SecurePass123"
        });

        Assert.False(result.Succeeded);
        Assert.NotNull(result.ErrorMessage);
    }
}

// ──────────────────────────────────────────────────────────────────────────────
// Interests tests
// ──────────────────────────────────────────────────────────────────────────────

public class InterestsServiceTests
{
    [Fact]
    public async Task GetAllAsync_ReturnsAllInterests()
    {
        using var db = DbHelper.CreateDb();
        await DbHelper.SeedInterestsAsync(db, count: 4);

        var svc = new InterestsService(db);
        var result = await svc.GetAllAsync();

        Assert.Equal(4, result.Count);
    }

    [Fact]
    public async Task SaveUserInterests_ValidIds_Succeeds()
    {
        using var db = DbHelper.CreateDb();
        var user = await DbHelper.SeedUserAsync(db);
        var interestIds = await DbHelper.SeedInterestsAsync(db, count: 3);

        var svc = new InterestsService(db);
        var result = await svc.SaveUserInterestsAsync(user.Id, interestIds);

        Assert.True(result.Succeeded);
        Assert.Equal(3, result.Interests!.Count);
    }

    [Fact]
    public async Task SaveUserInterests_FewerThan3_DoesNotCompleteOnboarding()
    {
        using var db = DbHelper.CreateDb();
        var user = await DbHelper.SeedUserAsync(db);
        var interestIds = await DbHelper.SeedInterestsAsync(db, count: 2);

        var svc = new InterestsService(db);
        var result = await svc.SaveUserInterestsAsync(user.Id, interestIds);

        Assert.True(result.Succeeded);
        var savedUser = await db.Users.FirstAsync(u => u.Id == user.Id);
        Assert.False(savedUser.IsOnboardingCompleted);
    }
}

// ──────────────────────────────────────────────────────────────────────────────
// User profile tests
// ──────────────────────────────────────────────────────────────────────────────

public class UserProfileTests
{
    [Fact]
    public async Task GetCurrentUser_ValidId_ReturnsProfile()
    {
        using var db = DbHelper.CreateDb();
        var user = await DbHelper.SeedUserAsync(db);

        var svc = new UserService(db);
        var profile = svc.GetCurrentUser(user.Id.ToString());

        Assert.NotNull(profile);
        Assert.Equal(user.Email, profile!.Email);
        Assert.Equal(user.FullName, profile.FullName);
    }
}
