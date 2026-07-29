using Microsoft.EntityFrameworkCore;
using MoodboardAI.Api.Data;
using MoodboardAI.Api.DTOs.Auth;
using MoodboardAI.Api.Services;
using NSubstitute;
using Xunit;

namespace MoodboardAI.Tests;

/// <summary>
/// Unit tests for <see cref="AuthService"/> covering registration and login scenarios.
/// Uses an in-memory database so no real PostgreSQL connection is required.
/// </summary>
public class AuthServiceTests
{
    // ──────────────────────────────────────────────
    // Helpers
    // ──────────────────────────────────────────────

    private static ApplicationDbContext CreateInMemoryDb()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // unique name per test
            .Options;
        return new ApplicationDbContext(options);
    }

    /// <summary>
    /// Creates an <see cref="AuthService"/> with a real <see cref="PasswordHasher"/> and
    /// a stub <see cref="IJwtTokenService"/> that always returns a fixed token string.
    /// </summary>
    private static AuthService CreateService(ApplicationDbContext db)
    {
        var jwtTokenService = Substitute.For<IJwtTokenService>();
        jwtTokenService.GenerateToken(Arg.Any<string>(), Arg.Any<string>())
            .Returns("test-jwt-token");

        return new AuthService(db, new PasswordHasher(), jwtTokenService);
    }

    private static RegisterRequestDto ValidRegisterRequest(string email = "user@example.com") =>
        new()
        {
            FullName = "Test User",
            Email = email,
            Password = "SecurePass123"
        };

    private static LoginRequestDto ValidLoginRequest(string email = "user@example.com") =>
        new()
        {
            Email = email,
            Password = "SecurePass123"
        };

    // ──────────────────────────────────────────────
    // Registration tests
    // ──────────────────────────────────────────────

    [Fact]
    public async Task Register_ValidRequest_Succeeds()
    {
        using var db = CreateInMemoryDb();
        var service = CreateService(db);

        var result = await service.RegisterAsync(ValidRegisterRequest());

        Assert.True(result.Succeeded);
        Assert.NotNull(result.Token);
        Assert.NotNull(result.User);
        Assert.Equal("user@example.com", result.User!.Email);
    }

    [Fact]
    public async Task Register_DuplicateEmail_Fails()
    {
        using var db = CreateInMemoryDb();
        var service = CreateService(db);

        // Register first time
        await service.RegisterAsync(ValidRegisterRequest());

        // Register again with the same email
        var result = await service.RegisterAsync(ValidRegisterRequest());

        Assert.False(result.Succeeded);
        Assert.NotNull(result.ErrorMessage);
        Assert.Contains("already exists", result.ErrorMessage, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Register_PasswordIsStoredAsHash()
    {
        using var db = CreateInMemoryDb();
        var service = CreateService(db);

        await service.RegisterAsync(ValidRegisterRequest());

        var user = await db.Users.FirstAsync();
        Assert.NotEqual("SecurePass123", user.PasswordHash);
        Assert.False(string.IsNullOrEmpty(user.PasswordHash));
    }

    // ──────────────────────────────────────────────
    // Login tests
    // ──────────────────────────────────────────────

    [Fact]
    public async Task Login_ValidCredentials_Succeeds()
    {
        using var db = CreateInMemoryDb();
        var service = CreateService(db);

        await service.RegisterAsync(ValidRegisterRequest());
        var result = await service.LoginAsync(ValidLoginRequest());

        Assert.True(result.Succeeded);
        Assert.NotNull(result.Token);
        Assert.NotNull(result.User);
    }

    [Fact]
    public async Task Login_InvalidPassword_Fails()
    {
        using var db = CreateInMemoryDb();
        var service = CreateService(db);

        await service.RegisterAsync(ValidRegisterRequest());

        var result = await service.LoginAsync(new LoginRequestDto
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
        using var db = CreateInMemoryDb();
        var service = CreateService(db);

        await service.RegisterAsync(ValidRegisterRequest());

        var result = await service.LoginAsync(new LoginRequestDto
        {
            Email = "nonexistent@example.com",
            Password = "SecurePass123"
        });

        Assert.False(result.Succeeded);
        Assert.NotNull(result.ErrorMessage);
    }
}
