using PracticeGamestore.DataAccess.Enums;
namespace PracticeGamestore.DataAccess.Entities;

public class Game
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public byte[]? Picture { get; set; }
    public string Description { get; set; } = string.Empty;
    public double Rating { get; set; }
    public AgeCategory AgeCategory { get; set; }
    public DateTime ReleaseDate { get; set; }
    public Guid PublisherId { get; set; }
    public Publisher Publisher { get; set; } = null!;
}