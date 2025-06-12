using PracticeGamestore.DataAccess.Entities.Filtering;

namespace PracticeGamestore.DataAccess.Repositories.Game;

public interface IGameRepository
{
    Task<IEnumerable<Entities.Game>> GetAllAsync(bool hideAdultContent = false);
    Task<(IEnumerable<Entities.Game>, int)> GetFiltered(GameFilter filter, bool hideAdultContent = false);
    Task<Entities.Game?> GetByIdAsync(Guid id);
    Task<IEnumerable<Entities.Game>> GetByPlatformIdAsync(Guid platformId, bool hideAdultContent = false);
    Task<List<Guid>> GetExistingIdsAsync(IEnumerable<Guid> ids);
    Task<IEnumerable<Entities.Game>> GetByPublisherIdAsync(Guid id, bool hideAdultContent = false);
    Task<IEnumerable<Entities.Game>> GetByGenreAndItsChildrenAsync(List<Guid> ids, bool hideAdultContent = false);
    Task DeleteAsync(Guid id);
    Task<Guid> CreateAsync(Entities.Game game, List<Guid> genreIds, List<Guid> platformIds);
    Task UpdateAsync(Entities.Game game, List<Guid> genreIds, List<Guid> platformIds);
    Task<bool> ExistsByNameAsync(string name);
    Task<bool> ExistsByKeyAsync(string key);
}