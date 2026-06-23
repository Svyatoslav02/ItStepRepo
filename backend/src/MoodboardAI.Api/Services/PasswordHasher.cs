using Isopoh.Cryptography.Argon2;
namespace MoodboardAI.Api.Services;

/// <summary>
/// Argon2 implementation of <see cref="IPasswordHasher"/>.
/// </summary>
public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
	{
    	return Argon2.Hash(password);
	}

	public bool VerifyPassword(string password, string hash)
    {
        try
        {
            return Argon2.Verify(hash, password);
        }
        catch
        {
            return false;
        }
    }
}