namespace PracticeGamestore.DataAccess.Entities;

public class Publisher
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public required string PageUrl { get; set; }
    public List<Game> Games { get; set; } = [];
}