namespace PracticeGamestore.DataAccess.Entities;

public class Platform
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public List<GamePlatform> GamePlatforms { get; set; } = [];
}