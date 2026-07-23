namespace MoodboardAI.Api.Models.Entities;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<PinTag> PinTags { get; set; } = new List<PinTag>();
}