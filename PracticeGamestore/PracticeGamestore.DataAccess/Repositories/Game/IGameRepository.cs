using PracticeGamestore.DataAccess.Filtering;

namespace PracticeGamestore.DataAccess.Repositories.Game;

public interface IGameRepository
{
    Task<IEnumerable<Entities.Game>> GetAllAsync();
    Task<(IEnumerable<Entities.Game>, int)> GetFiltered(GameFilter filter);
    Task<Entities.Game?> GetByIdAsync(Guid id);
    Task<IEnumerable<Entities.Game>> GetByPlatformIdAsync(Guid platformId);
    Task<List<Guid>> GetExistingIdsAsync(IEnumerable<Guid> ids);
    Task<IEnumerable<Entities.Game>> GetByPublisherIdAsync(Guid id);
    Task<IEnumerable<Entities.Game>> GetByGenreAndItsChildrenAsync(List<Guid> ids);
    Task DeleteAsync(Guid id);
    Task<Guid> CreateAsync(Entities.Game game, List<Guid> genreIds, List<Guid> platformIds);
    Task UpdateAsync(Entities.Game game, List<Guid> genreIds, List<Guid> platformIds);
    Task<bool> ExistsByNameAsync(string name);
    Task<bool> ExistsByKeyAsync(string key);
}