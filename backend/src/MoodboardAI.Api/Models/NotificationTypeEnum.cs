using Microsoft.EntityFrameworkCore;

namespace MoodboardAI.Api.Models;

/// <summary>
/// Enumeration of possible notification types in the application.
/// </summary>
public enum NotificationTypeEnum
{
    Like,
    Comment,
    Follow,
    Tag,
    Update,
    BoardActivity
}
