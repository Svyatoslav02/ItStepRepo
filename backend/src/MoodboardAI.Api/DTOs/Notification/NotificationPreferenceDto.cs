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
    /// Indicates whether the user has enabled quiet mode start time, 
    /// which suppresses notifications starting from a specific time.
    /// </summary>
    public TimeSpan? QuietModeStart { get; set; }

    /// <summary>
    /// Indicates whether the user has enabled quiet mode end time,
    /// which suppresses notifications ending at a specific time.   
    /// </summary>
    public TimeSpan? QuietModeEnd { get; set; }

    /// <summary>
    /// Validates the DTO. Since this DTO is used for partial updates, 
    /// it allows for null values. However, at least one field must be 
    /// provided for the update to be valid.
    /// Validates that if QuietModeStart is provided, QuietModeEnd must 
    /// also be provided, and vice versa.    
    /// </summary>
    /// <param name="validationContext">The context in which the validation is performed.</param>
    /// <returns>An enumerable of validation results.</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var hasAnyValue =
            PushLikes.HasValue || PushComments.HasValue || PushTags.HasValue ||
            PushFriendRequests.HasValue || PushUpdates.HasValue || PushRecommendations.HasValue ||
            PushMentions.HasValue ||
            EmailLikes.HasValue || EmailComments.HasValue || EmailTags.HasValue ||
            EmailFriendRequests.HasValue || EmailUpdates.HasValue || EmailRecommendations.HasValue ||
            EmailMentions.HasValue ||
            QuietMode.HasValue || QuietModeStart.HasValue || QuietModeEnd.HasValue;

        if (!hasAnyValue)
        {
            yield return new ValidationResult(
                "At least one notification preference field must be provided.",
                new[] { nameof(NotificationPreferenceDto) });
        }

        if (QuietModeStart.HasValue && QuietModeEnd.HasValue)
        {
            yield return new ValidationResult(
                "Both quiet mode start and end times must be provided.",
                new[] { nameof(QuietModeStart), nameof(QuietModeEnd) });
        }
    }
}