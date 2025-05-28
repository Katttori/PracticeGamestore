namespace PracticeGamestore.DataAccess.Repositories.Game;

public interface IGameRepository
{
    Task<IEnumerable<Entities.Game>> GetAllAsync();
    Task<Entities.Game?> GetByIdAsync(Guid id);
    public Task<IEnumerable<Entities.Game>> GetByPlatformIdAsync(Guid platformId);
    Task DeleteAsync(Guid id);
    Task<Guid> CreateAsync(Entities.Game game, List<Guid> genreIds, List<Guid> platformIds);
    Task UpdateAsync(Entities.Game game, List<Guid> genreIds, List<Guid> platformIds);
    Task<IEnumerable<Entities.Game>> GetByPublisherIdAsync(Guid id);
}