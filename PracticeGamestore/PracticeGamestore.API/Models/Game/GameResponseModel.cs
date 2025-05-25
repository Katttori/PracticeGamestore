using PracticeGamestore.Enums;
using PracticeGamestore.Models.Genre;
using PracticeGamestore.Models.Platform;
using PracticeGamestore.Models.Publisher;

namespace PracticeGamestore.Models.Game;

public class GameResponseModel
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Key { get; set; }
    public decimal Price { get; set; }
    public byte[]? Picture { get; set; }
    public string Description { get; set; } = string.Empty;
    public double Rating { get; set; }
    public AgeRating AgeRating { get; set; }
    public DateTime ReleaseDate { get; set; }
    public required PublisherResponseModel Publisher { get; set; }
    public required List<GenreResponseModel> Genres { get; set; }
    public required List<PlatformResponseModel> Platforms { get; set; }
}