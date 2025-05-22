using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.DataAccess.Entities;
using PracticeGamestore.DataAccess.Enums;

namespace PracticeGamestore.Business.Mappers;

public static class GameMappingExtensions
{
    public static GameDto MapToGameDto(this Game game)
    {
        return new (
            game.Id,
            game.Name,
            game.Key,
            game.Price,
            game.Picture,
            game.Description,
            game.Rating,
            (Enums.AgeRating)(int)game.AgeRating,
            game.ReleaseDate,
            game.PublisherId,
            // to change: use genre's and platform's dtos instead of ids
            game.GameGenres.Select(gg => gg.GenreId).ToList(),
            game.GamePlatforms.Select(gp => gp.PlatformId).ToList()
        );
    }

    private static Game MapToGameEntity(this GameDto dto)
    {
        return new ()
        {
            Id = dto.Id,
            Name = dto.Name,
            Key = dto.Key,
            Price = dto.Price,
            Picture = dto.Picture,
            Description = dto.Description,
            Rating = dto.Rating,
            AgeRating = (AgeRating)(int)dto.AgeRating,
            ReleaseDate = dto.ReleaseDate,
            PublisherId = dto.PublisherId
        };
    }
    
    public static (Game Game, List<Guid> GenreIds, List<Guid> PlatformIds) MapToGameEntityWithRelations(this GameDto dto)
    {
        var game = dto.MapToGameEntity();
        return (game, dto.GenreIds, dto.PlatformIds);
    }
}