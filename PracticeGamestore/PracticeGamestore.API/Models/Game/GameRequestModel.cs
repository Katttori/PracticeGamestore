using PracticeGamestore.Enums;
namespace PracticeGamestore.Models;

public class GameRequestModel
{
    public required string Name { get; set; }
    public required string Key { get; set; }
    public decimal Price { get; set; }
    public byte[]? Picture { get; set; }
    public string Description { get; set; } = string.Empty;
    public double Rating { get; set; }
    public AgeRating AgeRating { get; set; }
    public DateTime ReleaseDate { get; set; }
    public Guid PublisherId { get; set; }
    public required List<Guid> GenreIds { get; set; }
    public required List<Guid> PlatformIds { get; set; }
}