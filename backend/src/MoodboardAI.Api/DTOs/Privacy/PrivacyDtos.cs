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

/// <summary>
/// Full data export for a single user, returned by
/// <c>POST /api/users/me/data-export</c>. Never includes the password hash.
/// </summary>
public class UserDataExportDto
{
    public UserProfileExportDto Profile { get; set; } = new();
    public PrivacySettingsDto PrivacySettings { get; set; } = new();
    public List<string> LikedPinTitles { get; set; } = new();
    public List<string> SavedPinTitles { get; set; } = new();
    public List<string> BlockedUsernames { get; set; } = new();
    public DateTime ExportedAtUtc { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Profile fields included in a data export (excludes password hash).
/// </summary>
public class UserProfileExportDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string? DisplayName { get; set; }
    public string? Bio { get; set; }
    public string? AvatarUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}