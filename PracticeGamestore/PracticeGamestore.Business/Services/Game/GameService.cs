using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.DataTransferObjects.Filtering;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.DataAccess.Repositories.Game;
using PracticeGamestore.DataAccess.Repositories.Genre;
using PracticeGamestore.DataAccess.Repositories.Platform;
using PracticeGamestore.DataAccess.Repositories.Publisher;
using PracticeGamestore.DataAccess.UnitOfWork;

namespace PracticeGamestore.Business.Services.Game;

public class GameService(IGameRepository gameRepository, IPublisherRepository publisherRepository,
    IGenreRepository genreRepository, IPlatformRepository platformRepository, 
    IUnitOfWork unitOfWork) : IGameService
{
    public async Task<IEnumerable<GameResponseDto>> GetAllAsync()
    {
        var entities = await gameRepository.GetAllAsync();
        return entities.Select(e => e.MapToGameDto());
    }

    public async Task<(IEnumerable<GameResponseDto>, int)> GetFilteredAsync(GameFilter filter)
    {
        var (games, totalCount) = await gameRepository.GetFiltered(filter.MapToDataAccessGameFilter());
        return (games.Select(e => e.MapToGameDto()), totalCount);
    }

    public async Task<bool> UpdateAsync(Guid id, GameRequestDto gameRequestDto)
    {
        var existingGame = await gameRepository.GetByIdAsync(id);
        if (existingGame is null) return false;
        
        if (gameRequestDto.Name != existingGame.Name && await gameRepository.ExistsByNameAsync(gameRequestDto.Name))
        {
            throw new ArgumentException($"Game with name '{gameRequestDto.Name}' already exists.");
        }
        
        if (gameRequestDto.Key != existingGame.Key && await gameRepository.ExistsByKeyAsync(gameRequestDto.Key))
        {
            throw new ArgumentException($"Game with key '{gameRequestDto.Key}' already exists.");
        }
        
        if (!await AreSpecifiedRelationshipsValid(gameRequestDto))
        {
            return false;
        }
        
        var (game, genreIds, platformIds) = gameRequestDto.MapToGameEntityWithRelations();
        game.Id = id;
        await gameRepository.UpdateAsync(game, genreIds, platformIds);
        var updated = await unitOfWork.SaveChangesAsync();
        return updated > 0;
    }

    public async Task<Guid?> CreateAsync(GameRequestDto gameRequestDto)
    {
        if (await gameRepository.ExistsByNameAsync(gameRequestDto.Name))
        {
            throw new ArgumentException($"Game with name '{gameRequestDto.Name}' already exists.");
        }
        
        if (await gameRepository.ExistsByKeyAsync(gameRequestDto.Key))
        {
            throw new ArgumentException($"Game with key '{gameRequestDto.Key}' already exists.");
        }
        
        if (!await AreSpecifiedRelationshipsValid(gameRequestDto))
        {
            return null;
        }
        
        var (game, genreIds, platformIds) = gameRequestDto.MapToGameEntityWithRelations();
        var id = await gameRepository.CreateAsync(game, genreIds, platformIds);
        var created = await unitOfWork.SaveChangesAsync();
        return created > 0 ? id : null;
    }

    public async Task<GameResponseDto?> GetByIdAsync(Guid id)
    {
        var game = await gameRepository.GetByIdAsync(id);
        return game?.MapToGameDto();
    }
    
    public async Task<IEnumerable<GameResponseDto>?> GetByPlatformAsync(Guid platformId)
    {
        var platformExists = await platformRepository.ExistsByIdAsync(platformId);
        if (!platformExists) return null;

        var games = await gameRepository.GetByPlatformIdAsync(platformId);
        return games.Select(g => g.MapToGameDto());
    }

    public async Task DeleteAsync(Guid id)
    {
        await gameRepository.DeleteAsync(id);
        await unitOfWork.SaveChangesAsync();
    }
    
    private async Task<bool> AreSpecifiedRelationshipsValid(GameRequestDto gameRequestDto)
    {
        var platformsValid = await AllSpecifiedPlatformIdsExist(gameRequestDto.PlatformIds);
        if (!platformsValid) return false;
    
        var genresValid = await AllSpecifiedGenreIdsExist(gameRequestDto.GenreIds);
        if (!genresValid) return false;
    
        var publisherValid = await SpecifiedPublisherIdExist(gameRequestDto.PublisherId);
        return publisherValid;
    }

    private async Task<bool> AllSpecifiedPlatformIdsExist(IEnumerable<Guid> ids)
    {
        var existingPlatformIds = (await platformRepository.GetAllAsync()).Select(p => p.Id).ToHashSet();
        return ids.All(id => existingPlatformIds.Contains(id));
    }
    
    private async Task<bool> AllSpecifiedGenreIdsExist(IEnumerable<Guid> ids)
    {
        var existingGenreIds = (await genreRepository.GetAllAsync()).Select(g => g.Id).ToHashSet();
        return ids.All(id => existingGenreIds.Contains(id));
    }

    private async Task<bool> SpecifiedPublisherIdExist(Guid id)
    {
        var publisher = await publisherRepository.GetByIdAsync(id);
        return publisher is not null;
    }
}