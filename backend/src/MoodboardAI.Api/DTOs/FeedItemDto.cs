namespace MoodboardAI.Api.DTOs;

public class FeedItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public string ImageUrl { get; set; } = "";
    public string Category { get; set; } = "";
    public string Author { get; set; } = "";
    public List<string> Tags { get; set; } = new();
    public int LikeCount { get; set; }
    public bool IsLiked { get; set; }
    public DateTime CreatedAt { get; set; }

}