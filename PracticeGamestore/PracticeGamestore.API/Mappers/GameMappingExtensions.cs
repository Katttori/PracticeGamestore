using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Models;

namespace PracticeGamestore.Mappers;

public static class GameMappingExtensions
{
    public static GameDto MapToGameDto(this GameRequestModel model, Guid? id = null)
    {
        return new (
            id,
            model.Name,
            model.Key,
            model.Price,
            model.Picture,
            model.Description,
            model.Rating,
            model.AgeRating,
            model.ReleaseDate,
            model.PublisherId,
            model.GenreIds,
            model.PlatformIds
        );
    }
    
    public static GameResponseModel MapToGameModel(this GameDto dto)
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
            Picture = dto.Picture,
            GenreIds = dto.GenreIds,
            PlatformIds = dto.PlatformIds
        };
    }
}