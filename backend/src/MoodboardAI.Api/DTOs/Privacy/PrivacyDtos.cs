namespace MoodboardAI.Api.DTOs.Privacy;

/// <summary>
/// Response DTO for the user's privacy settings.
/// </summary>
public class PrivacySettingsDto
{
    public bool PrivateAccount { get; set; }
    public bool SearchVisibility { get; set; }
    public bool ContentVisibility { get; set; }
}

/// <summary>
/// Request DTO for updating the user's privacy settings.
/// </summary>
public class UpdatePrivacySettingsDto
{
    public bool PrivateAccount { get; set; }
    public bool SearchVisibility { get; set; }
    public bool ContentVisibility { get; set; }
}

/// <summary>
/// Represents a blocked user returned in the blocked users list.
/// </summary>
public class BlockedUserDto
{
    public Guid BlockedUserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public DateTime BlockedAt { get; set; }
}

/// <summary>
/// Request DTO for blocking a user.
/// </summary>
public class BlockUserRequestDto
{
    public Guid UserId { get; set; }
}
