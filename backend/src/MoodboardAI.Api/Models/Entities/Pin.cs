namespace MoodboardAI.Api.Models.Entities;

public class Pin
{
    public int Id { get; set; }
    public string Title { get; set; } = String.Empty;
    public string ImageUrl { get; set; } = String.Empty;
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public ICollection<PinTag> PinTags { get; set; } = new List<PinTag>();
}