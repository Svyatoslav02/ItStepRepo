using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoodboardAI.Api.Models;

/// <summary>
/// Stores privacy preferences for a user account.
/// Linked one-to-one with <see cref="UserEntity"/>.
/// </summary>
public class UserPrivacySettings
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// When true, the account is private and content is hidden from non-followers.
    /// </summary>
    public bool PrivateAccount { get; set; } = false;

    /// <summary>
    /// When true, the user appears in search results.
    /// </summary>
    public bool SearchVisibility { get; set; } = true;

    /// <summary>
    /// When true, the user's content is visible to other users.
    /// </summary>
    public bool ContentVisibility { get; set; } = true;

    // Navigation property
    public UserEntity User { get; set; } = null!;
}
