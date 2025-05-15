namespace PracticeGamestore.DataAccess.Entities;

public class Publisher
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string PageUrl { get; set; } = string.Empty;
}