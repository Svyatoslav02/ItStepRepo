namespace MoodboardAI.Api.Models;

/// <summary>
/// Standard error response shape returned by the API on validation or processing failures.
/// </summary>
public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;
}
