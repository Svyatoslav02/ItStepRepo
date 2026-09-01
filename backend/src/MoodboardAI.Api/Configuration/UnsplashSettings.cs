namespace MoodboardAI.Api.Configuration;

/// <summary>
/// Configuration for the Unsplash Search Photos API, used to power real
/// moodboard image generation. Read from the "Unsplash" section of
/// appsettings.json (or the UNSPLASH_ACCESS_KEY / UNSPLASH__ACCESSKEY
/// environment variable — see docs/environment.md).
/// </summary>
/// <remarks>
/// Unsplash's free "Demo" tier requires no payment method and allows
/// 50 requests/hour, which is enough for local development and demos.
/// </remarks>
public class UnsplashSettings
{
    /// <summary>
    /// Client-ID access key issued by Unsplash. When empty, the app falls
    /// back to <see cref="MoodboardAI.Api.Services.MockMoodboardService"/>.
    /// </summary>
    public string AccessKey { get; set; } = string.Empty;

    /// <summary>
    /// Base URL of the Unsplash API.
    /// </summary>
    public string BaseUrl { get; set; } = "https://api.unsplash.com";

    /// <summary>
    /// Number of images to request per moodboard.
    /// </summary>
    public int ResultsPerPage { get; set; } = 6;
}