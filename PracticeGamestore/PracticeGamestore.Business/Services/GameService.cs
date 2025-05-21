using System.Collections;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.DataAccess.Repositories;
using PracticeGamestore.DataAccess.UnitOfWork;

namespace PracticeGamestore.Business.Services;

public class GameService(IGameRepository repository, IUnitOfWork unitOfWork): IGameService
{
    public async Task<IEnumerable<GameDto>> GetAllAsync()
    {
        var entities =  await repository.GetAllAsync();
        return entities.Select(e => e.ToDto());
    }

    public async Task<bool> UpdateAsync(GameDto gameDto)
    {
        var existingGame = await repository.GetByIdAsync(gameDto.Id);
        if (existingGame is null) return false;
        //to do: check if all provided genres and platforms, publisher exist, if no -> false
        var (game, genreIds, platformIds) = gameDto.ToEntityWithRelations();
        await repository.UpdateAsync(game, genreIds, platformIds);
        var updated = await unitOfWork.SaveChangesAsync();
        return updated > 0;
    }

    public async Task<Guid?> CreateAsync(GameDto gameDto)
    {
        //to do: check if all provided genres and platforms, publisher exist. If no, return null
        var (game, genreIds, platformIds) = gameDto.ToEntityWithRelations();
        var id = await repository.CreateAsync(game, genreIds, platformIds);
        var created = await unitOfWork.SaveChangesAsync();
        return created > 0 ? id : null;

    }

    public async Task<GameDto?> GetByIdAsync(Guid id)
    {
        var game = await repository.GetByIdAsync(id);
        return game?.ToDto();
    }

    public async Task DeleteAsync(Guid id)
    {
        await repository.DeleteAsync(id);
        await unitOfWork.SaveChangesAsync();
    }
}