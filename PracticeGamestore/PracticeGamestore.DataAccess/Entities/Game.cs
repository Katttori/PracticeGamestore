namespace PracticeGamestore.DataAccess.Entities;

public class Game
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Key { get; set; }
    public decimal Price { get; set; }
    public byte[]? Picture { get; set; }
    public string Description { get; set; } = string.Empty;
    public double Rating { get; set; }
    public int AgeRating { get; set; }
    public DateTime ReleaseDate { get; set; }
    public Guid PublisherId { get; set; }
    public Publisher Publisher { get; set; } = null!;
    public List<File> Files { get; set; } = [];
    public List<GameGenre> GameGenres { get; set; } = [];
    public List<GamePlatform> GamePlatforms { get; set; } = [];
    public List<GameOrder> GameOrders { get; set; } = [];
}