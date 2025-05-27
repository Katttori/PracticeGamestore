using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Models.Game;

namespace PracticeGamestore.Mappers;

public static class GameMappingExtensions
{
    public static GameRequestDto MapToGameDto(this GameRequestModel model, Guid? id = null)
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
    
    public static GameResponseModel MapToGameModel(this GameResponseDto responseDto)
    {
        return new ()
        {
            Id = responseDto.Id,
            Name = responseDto.Name,
            Key = responseDto.Key,
            AgeRating = responseDto.AgeRating,
            Price = responseDto.Price,
            Description = responseDto.Description,
            Rating = responseDto.Rating,
            ReleaseDate = responseDto.ReleaseDate,  
            Picture = responseDto.Picture,
            Publisher = responseDto.Publisher.MapToPublisherModel(),
            Genres = responseDto.Genres.Select(g => g.MapToGenreModel()).ToList(),
            Platforms = responseDto.Platforms.Select(g => g.MapToPlatformModel()).ToList()
        };
    }
}