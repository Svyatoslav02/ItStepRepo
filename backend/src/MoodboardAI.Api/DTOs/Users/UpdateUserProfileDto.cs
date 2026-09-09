using System.ComponentModel.DataAnnotations;

namespace MoodboardAI.Api.DTOs.Users;

/// <summary>
/// Request payload for partially updating the current user's profile.
/// Only the supplied fields are updated; at least one must be provided.
/// </summary>
public class UpdateUserProfileDto : IValidatableObject
{
    [MinLength(2, ErrorMessage = "Full name must be at least 2 characters long.")]
    [MaxLength(100, ErrorMessage = "Full name must be at most 100 characters long.")]
    public string? FullName { get; set; }
    [EmailAddress(ErrorMessage = "Email is not valid.")]
    public string? Email { get; set; }
    [Url(ErrorMessage = "AvatarUrl must be a valid URL.")]
    [MaxLength(500, ErrorMessage = "AvatarUrl must be at most 500 characters long.")]
    public string? AvatarUrl { get; set; }

    /// <summary>
    /// Validates that at least one updatable field was supplied, since an
    /// empty payload carries no update intent and would otherwise silently no-op.
    /// </summary>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (FullName is null && Email is null && AvatarUrl is null)
        {
            yield return new ValidationResult(
                "At least one of FullName, Email, or AvatarUrl must be provided.",
                new[] { nameof(UpdateUserProfileDto) });
        }
    }
}