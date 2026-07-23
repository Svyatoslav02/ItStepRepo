namespace MoodboardAI.Api.Models.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Pin> Pins { get; set; } = new List<Pin>();
}