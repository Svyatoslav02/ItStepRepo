namespace MoodboardAI.Api.DTOs.Auth;

/// <summary>
/// Internal result returned by the auth service layer to the controller.
/// </summary>
public class AuthResultDto
{
    public bool Succeeded { get; set; }
    public string? Token { get; set; }
    public UserDto? User { get; set; }
    public string? ErrorMessage { get; set; }
}
