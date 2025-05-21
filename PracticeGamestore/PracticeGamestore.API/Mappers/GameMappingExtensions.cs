using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Models;

namespace PracticeGamestore.Mappers;

public static class GameMappingExtensions
{
    private static byte[]? ConvertPictureToByteArray(IFormFile? picture)
    {
        if(picture == null || picture.Length==0)return null;
        using var memoryStream = new MemoryStream();
        picture.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }

    private static string? ConvertByteArrayToPicture(byte[]? picture) => picture == null? null : Convert.ToBase64String(picture);
    
    public static GameDto ToDto(this GameRequestModel model, Guid? id = null, byte[]? pictureData = null)
    {
        return new (
            id,
            model.Name,
            model.Key,
            model.Price,
            pictureData ?? ConvertPictureToByteArray(model.Picture),
            model.Description,
            model.Rating,
            model.AgeRating,
            model.ReleaseDate,
            model.PublisherId,
            model.GenreIds,
            model.PlatformIds
        );
    }
    
    public static GameResponseModel ToModel(this GameDto dto)
    {
        return new ()
        {
            Id = dto.Id,
            Name = dto.Name,
            Key = dto.Key,
            AgeRating = dto.AgeRating,
            Price = dto.Price,
            Description = dto.Description,
            PublisherId = dto.PublisherId,
            Picture = ConvertByteArrayToPicture(dto.Picture),
            GenreIds = dto.GenreIds,
            PlatformIds = dto.PlatformIds
        };
    }
}