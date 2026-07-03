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

public static class ErrorHelper
{
    public static ErrorResponse Create(int code, string description)
    {
        return new ErrorResponse
        {
            Message = $"{code} - {description}"
        };
    }
}