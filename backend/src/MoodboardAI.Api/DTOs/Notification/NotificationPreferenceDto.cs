namespace MoodboardAI.Api.DTOs.Notification;

/// <summary>
/// Represents the user's notification preferences for different types of notifications, 
/// including push and email notifications, as well as a quiet mode setting (supports partial updates).
/// </summary>
public class NotificationPreferenceDto
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
}

