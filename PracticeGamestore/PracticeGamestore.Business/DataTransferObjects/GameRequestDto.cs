using PracticeGamestore.DataAccess.Enums;

namespace PracticeGamestore.Business.DataTransferObjects;

public class GameRequestDto
{
    public Guid? Id { get; set; }
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

    public GameRequestDto(
        Guid? id,
        string name, 
        string key, 
        decimal price,
        byte[]? picture,
        string description,
        double rating,
        int ageRating,
        DateTime releaseDate,
        Guid publisherId,
        List<Guid> genreIds,
        List<Guid> platformIds)
    {
        Id = id;
        Name = name;
        Key = key;
        Price = price;
        Picture = picture;
        Description = description;
        Rating = rating;
        AgeRating = ConvertToAgeRatingEnum(ageRating);
        ReleaseDate = releaseDate;
        PublisherId = publisherId;
        GenreIds = genreIds;
        PlatformIds = platformIds;
    }

    private static AgeRating ConvertToAgeRatingEnum(int age)
    {
        if (!Enum.IsDefined(typeof(AgeRating), age))
            throw new ArgumentException($"Invalid age rating value was passed! {age}");
        return (AgeRating)age;
    }
}