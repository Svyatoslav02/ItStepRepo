namespace MoodboardAI.Api.Models;

/// <summary>
/// Standard error response returned by the API on validation or processing failures.
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Error message.
    /// </summary>
    public string Message { get; set; } = string.Empty;
}