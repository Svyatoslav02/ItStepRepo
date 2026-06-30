namespace MoodboardAI.Api.Services;
/// <summary>
/// Defines a contract for generating JWT tokens for authenticated users.
/// </summary>
public interface IJwtTokenService
{
    /// <summary>
    /// Generates a signed JWT token for the given user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="email">The email address of the user.</param>
    /// <returns>A signed JWT token string.</returns>
    string GenerateToken(string userId, string email);
}
