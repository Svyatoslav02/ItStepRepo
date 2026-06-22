namespace MoodboardAI.Api.Services;

/// <summary>
/// Service for securely hashing and verifying passwords using Argon2.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Hashes a plain text password using Argon2 algorithm.
    /// Each call generates a unique hash with random salt.
    /// </summary>
    /// <param name="password">The plain text password to hash</param>
    /// <returns>Argon2 hash string safe to store in database</returns>
    string HashPassword(string password);
    
    /// <summary>
    /// Verifies if a plain text password matches a stored Argon2 hash.
    /// </summary>
    /// <param name="password">The plain text password to verify</param>
    /// <param name="hash">The stored Argon2 hash from database</param>
    /// <returns>True if password matches hash, false otherwise</returns>
    bool VerifyPassword(string password, string hash);
}