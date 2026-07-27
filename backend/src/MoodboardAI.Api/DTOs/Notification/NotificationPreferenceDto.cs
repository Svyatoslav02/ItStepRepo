using System.ComponentModel.DataAnnotations;

namespace MoodboardAI.Api.DTOs.Notification;

/// <summary>
/// Represents the user's notification preferences for different types of notifications, 
/// including push and email notifications, as well as a quiet mode setting (supports partial updates).
/// </summary>
public class NotificationPreferenceDto : IValidatableObject
{
    /// <summary>
    /// Indicates whether the user wants to receive push notifications for likes.
    /// </summary>
    public bool? PushLikes { get; set; }
    /// <summary>
    /// Indicates whether the user wants to receive push notifications for comments.
    /// </summary>
    public bool? PushComments { get; set; }
    /// <summary>
    /// Indicates whether the user wants to receive push notifications for tags.
    /// </summary>
    public bool? PushTags { get; set; }
    /// <summary>
    /// Indicates whether the user wants to receive push notifications for friend requests.
    /// </summary>
    public bool? PushFriendRequests { get; set; }
    /// <summary>
    /// Indicates whether the user wants to receive push notifications for updates.
    /// </summary>
    public bool? PushUpdates { get; set; }
    /// <summary>
    /// Indicates whether the user wants to receive push notifications for recommendations.
    /// </summary>
    public bool? PushRecommendations { get; set; }
    /// <summary>
    /// Indicates whether the user wants to receive push notifications for mentions.
    /// </summary>
    public bool? PushMentions { get; set; }

    /// <summary>
    /// Indicates whether the user wants to receive email notifications for likes.
    /// </summary>
    public bool? EmailLikes { get; set; }
    /// <summary>
    /// Indicates whether the user wants to receive email notifications for comments.
    /// </summary>
    public bool? EmailComments { get; set; }
    /// <summary>
    /// Indicates whether the user wants to receive email notifications for tags.
    /// </summary>
    public bool? EmailTags { get; set; }
    /// <summary>
    /// Indicates whether the user wants to receive email notifications for friend requests.
    /// </summary>
    public bool? EmailFriendRequests { get; set; }
    /// <summary>
    /// Indicates whether the user wants to receive email notifications for updates.
    /// </summary>
    public bool? EmailUpdates { get; set; }
    /// <summary>
    /// Indicates whether the user wants to receive email notifications for recommendations.
    /// </summary>
    public bool? EmailRecommendations { get; set; }
    /// <summary>
    /// Indicates whether the user wants to receive email notifications for mentions.
    /// </summary>
    public bool? EmailMentions { get; set; }

    /// <summary>
    /// Indicates whether the user has enabled quiet mode, which suppresses most notifications.
    /// </summary>
    public bool? QuietMode { get; set; }

    /// <summary>
    /// Validates the DTO. Since every field is an optional <see cref="bool"/> used for
    /// partial updates, [Required]/[Range]-style attributes cannot express what an
    /// "invalid" payload means here. The one payload that is genuinely invalid for a
    /// PUT is an empty one (no fields set at all) — it carries no update intent and
    /// would otherwise silently no-op. That case is rejected with a 400.
    /// </summary>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var hasAnyValue =
            PushLikes.HasValue || PushComments.HasValue || PushTags.HasValue ||
            PushFriendRequests.HasValue || PushUpdates.HasValue || PushRecommendations.HasValue ||
            PushMentions.HasValue ||
            EmailLikes.HasValue || EmailComments.HasValue || EmailTags.HasValue ||
            EmailFriendRequests.HasValue || EmailUpdates.HasValue || EmailRecommendations.HasValue ||
            EmailMentions.HasValue ||
            QuietMode.HasValue;

        if (!hasAnyValue)
        {
            yield return new ValidationResult(
                "At least one notification preference field must be provided.",
                new[] { nameof(NotificationPreferenceDto) });
        }
    }
}