using Microsoft.EntityFrameworkCore;
using MoodboardAI.Api.Data;
using MoodboardAI.Api.DTOs.Auth;
using MoodboardAI.Api.Models;

namespace MoodboardAI.Api.Services;

/// <summary>
/// Implements user registration and login: duplicate-email checks, password
/// hashing/verification via <see cref="IPasswordHasher"/>, and JWT issuance via
/// <see cref="IJwtTokenService"/>.
/// </summary>
public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthService"/> class.
    /// </summary>
    public AuthService(
        ApplicationDbContext dbContext,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    /// <inheritdoc />
    public async Task<AuthResultDto> RegisterAsync(RegisterRequestDto request)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var emailAlreadyExists = await _dbContext.Users
            .AnyAsync(u => u.Email.ToLower() == normalizedEmail);

        if (emailAlreadyExists)
        {
            return new AuthResultDto
            {
                Succeeded = false,
                ErrorMessage = "A user with this email already exists."
            };
        }

        var user = new UserEntity
        {
            FullName = request.FullName,
            Email = request.Email,
            // UserEntity.Username is required by the existing schema, but the
            // registration flow only collects fullName/email/password. Derive a
            // username from the email's local part so the field is never empty.
            Username = normalizedEmail.Split('@')[0],
            PasswordHash = _passwordHasher.HashPassword(request.Password)
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        return BuildSuccessResult(user);
    }

    /// <inheritdoc />
    public async Task<AuthResultDto> LoginAsync(LoginRequestDto request)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email.ToLower() == normalizedEmail);

        if (user is null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            return new AuthResultDto
            {
                Succeeded = false,
                ErrorMessage = "Invalid email or password."
            };
        }

        return BuildSuccessResult(user);
    }

    /// <summary>
    /// Builds a successful <see cref="AuthResultDto"/> containing a freshly
    /// generated JWT token and the user's public data.
    /// </summary>
    private AuthResultDto BuildSuccessResult(UserEntity user)
    {
        var token = _jwtTokenService.GenerateToken(user.Id.ToString(), user.Email);

        return new AuthResultDto
        {
            Succeeded = true,
            Token = token,
            User = new UserDto
            {
                Id = user.Id.ToString(),
                FullName = user.FullName,
                Email = user.Email
            }
        };
    }
}
