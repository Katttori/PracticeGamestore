using PracticeGamestore.Enums;
namespace PracticeGamestore.Business.DataTransferObjects;

public class GameDto
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
    public Guid PublisherId { get; set; }
    public List<Guid> GenreIds { get; set; }
    public List<Guid> PlatformIds { get; set; }

    public GameDto(
        Guid? id,
        string name, 
        string key, 
        decimal price,
        byte[]? picture,
        string description,
        double rating,
        AgeRating ageRating,
        DateTime releaseDate,
        Guid publisherId,
        List<Guid> genreIds,
        List<Guid> platformIds)
    {
        Id = id ?? new Guid();
        Name = name;
        Key = key;
        Price = price;
        Picture = picture;
        Description = description;
        Rating = rating;
        AgeRating = ageRating;
        ReleaseDate = releaseDate;
        PublisherId = publisherId;
        GenreIds = genreIds;
        PlatformIds = platformIds;
    }
}