using MoodboardAI.Api.DTOs.Auth;

namespace MoodboardAI.Api.Services;

/// <summary>
/// Defines authentication operations for registering and logging in users.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Registers a new user account. Fails if the email is already registered.
    /// </summary>
    /// <param name="request">Registration request containing full name, email, and password.</param>
    /// <returns>An <see cref="AuthResultDto"/> with the issued JWT token and user data on success.</returns>
    Task<AuthResultDto> RegisterAsync(RegisterRequestDto request);

    /// <summary>
    /// Authenticates an existing user by email and password.
    /// </summary>
    /// <param name="request">Login request containing email and password.</param>
    /// <returns>An <see cref="AuthResultDto"/> with the issued JWT token and user data on success.</returns>
    Task<AuthResultDto> LoginAsync(LoginRequestDto request);
}
