using PracticeGamestore.Enums;

namespace PracticeGamestore.Business.DataTransferObjects;

public class GameResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Key { get; set; }
    public decimal Price { get; set; }
    public byte[]? Picture { get; set; }
    public string Description { get; set; }
    public double Rating { get; set; }
    public AgeRating AgeRating { get; set; }
    public DateTime ReleaseDate { get; set; }
    public PublisherDto Publisher { get; set; } 
    public List<PlatformDto> Platforms { get; set; } 
    public List<GenreDto> Genres { get; set; }

    public GameResponseDto(Guid id, string name, string key, decimal price, byte[]? picture, string description, double rating, AgeRating ageRating, DateTime releaseDate, PublisherDto publisher, List<PlatformDto> platforms, List<GenreDto> genres)
    {
        Id = id;
        Name = name;
        Key = key;
        Price = price;
        Picture = picture;
        Description = description;
        Rating = rating;
        AgeRating = ageRating;
        ReleaseDate = releaseDate;
        Publisher = publisher;
        Platforms = platforms;
        Genres = genres;
    }
}