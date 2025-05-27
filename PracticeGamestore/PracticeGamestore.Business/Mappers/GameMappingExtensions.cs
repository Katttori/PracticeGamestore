using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Enums;
using PracticeGamestore.DataAccess.Entities;

namespace PracticeGamestore.Business.Mappers;

public static class GameMappingExtensions
{
    public static GameResponseDto MapToGameDto(this Game game)
    {
        return new (
            game.Id,
            game.Name,
            game.Key,
            game.Price,
            game.Picture,
            game.Description,
            game.Rating,
            (AgeRating)game.AgeRating,
            game.ReleaseDate,
            game.Publisher.MapToPublisherDto(),
            game.GamePlatforms.Select(gp => gp.Platform.MapToPlatformDto()).ToList(),
            game.GameGenres.Select(gg => gg.Genre.MapToGenreDto()).ToList()

        );
    }

    private static Game MapToGameEntity(this GameRequestDto requestDto)
    {
        return new ()
        {
            Name = requestDto.Name,
            Key = requestDto.Key,
            Price = requestDto.Price,
            Picture = requestDto.Picture,
            Description = requestDto.Description,
            Rating = requestDto.Rating,
            AgeRating = (int)requestDto.AgeRating,
            ReleaseDate = requestDto.ReleaseDate,
            PublisherId = requestDto.PublisherId
        };
    }
    
    public static (Game Game, List<Guid> GenreIds, List<Guid> PlatformIds) MapToGameEntityWithRelations(this GameRequestDto requestDto)
    {
        var game = requestDto.MapToGameEntity();
        return (game, requestDto.GenreIds, requestDto.PlatformIds);
    }
}