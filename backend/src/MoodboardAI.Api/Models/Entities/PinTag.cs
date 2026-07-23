namespace MoodboardAI.Api.Models.Entities;

public class PinTag
{
    public int PinId { get; set; }
    public Pin? Pin { get; set; } 
    public int TagId { get; set; }
    public Tag? Tag { get; set; }
}