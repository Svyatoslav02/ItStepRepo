namespace MoodboardAI.Api.Configuration;
/// <summary>
/// Represents the JWT configuration settings read from the "Jwt" section
/// of the application configuration (appsettings.json or environment variables).
/// </summary>
public class JwtSettings
{
    public string SecretKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpirationMinutes { get; set; } = 60;
}
