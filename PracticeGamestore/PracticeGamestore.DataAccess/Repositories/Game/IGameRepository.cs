namespace PracticeGamestore.DataAccess.Repositories.Game;

public interface IGameRepository
{
    Task<IEnumerable<Entities.Game>> GetAllAsync();
    Task<Entities.Game?> GetByIdAsync(Guid id);
    Task<List<Guid>> GetExistingIdsAsync(IEnumerable<Guid> ids);
    Task DeleteAsync(Guid id);
    Task<Guid> CreateAsync(Entities.Game game, List<Guid> genreIds, List<Guid> platformIds);
    Task UpdateAsync(Entities.Game game, List<Guid> genreIds, List<Guid> platformIds);
    Task<IEnumerable<Entities.Game>> GetByPublisherIdAsync(Guid id);
}