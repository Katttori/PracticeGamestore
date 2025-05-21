using System.Net.Mime;
using PracticeGamestore.Enums;

namespace PracticeGamestore.Models;

public class GameResponseModel
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Key { get; set; }
    public decimal Price { get; set; }
    public string? Picture { get; set; }
    public string Description { get; set; } = string.Empty;
    public double Rating { get; set; }
    public AgeRating AgeRating { get; set; }
    public DateTime ReleaseDate { get; set; }
    public Guid PublisherId { get; set; }
    // to change: replace ids with genre and platform models
    public required List<Guid> GenreIds { get; set; }
    public required List<Guid> PlatformIds { get; set; }

}