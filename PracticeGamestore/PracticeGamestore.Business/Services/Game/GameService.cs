using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.DataAccess.Repositories;
using PracticeGamestore.DataAccess.Repositories.Game;
using PracticeGamestore.DataAccess.Repositories.Genre;
using PracticeGamestore.DataAccess.Repositories.Platform;
using PracticeGamestore.DataAccess.Repositories.Publisher;
using PracticeGamestore.DataAccess.UnitOfWork;

namespace PracticeGamestore.Business.Services.Game;

public class GameService(IGameRepository gameRepository, IPublisherRepository publisherRepository, IGenreRepository genreRepository, IPlatformRepository platformRepository, IUnitOfWork unitOfWork) : IGameService
{
    public async Task<IEnumerable<GameResponseDto>> GetAllAsync()
    {
        var entities =  await gameRepository.GetAllAsync();
        return entities.Select(e => e.MapToGameDto());
    }

    public async Task<bool> UpdateAsync(GameRequestDto gameRequestDto)
    {
        var existingGame = await gameRepository.GetByIdAsync(gameRequestDto.Id);
        if (existingGame is null) return false;
        if (! await AllSpecifiedRelationshipsAreCorrect(gameRequestDto)) return false;
        var (game, genreIds, platformIds) = gameRequestDto.MapToGameEntityWithRelations();
        await gameRepository.UpdateAsync(game, genreIds, platformIds);
        var updated = await unitOfWork.SaveChangesAsync();
        return updated > 0;
    }

    public async Task<Guid?> CreateAsync(GameRequestDto gameRequestDto)
    {
        if (! await AllSpecifiedRelationshipsAreCorrect(gameRequestDto)) return null;
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

    public async Task DeleteAsync(Guid id)
    {
        await gameRepository.DeleteAsync(id);
        await unitOfWork.SaveChangesAsync();
    }
    
    private async Task<bool> AllSpecifiedRelationshipsAreCorrect(GameRequestDto gameRequestDto)
    {
        var platformTask = AllSpecifiedPlatformIdsExist(gameRequestDto.PlatformIds);
        var genreTask = AllSpecifiedGenreIdsExist(gameRequestDto.GenreIds);
        var publisherTask = SpecifiedPublisherIdExist(gameRequestDto.PublisherId);
    
        var results = await Task.WhenAll(platformTask, genreTask, publisherTask);
        return results.All(result => result);
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