namespace PracticeGamestore.DataAccess.Repositories.Game;

public interface IGameRepository
{
    Task<IEnumerable<Entities.Game>> GetAllAsync();
    Task<Entities.Game?> GetByIdAsync(Guid id);
    Task DeleteAsync(Guid id);
    Task<Guid> CreateAsync(Entities.Game game, List<Guid> genreIds, List<Guid> platformIds);
    Task UpdateAsync(Entities.Game game, List<Guid> genreIds, List<Guid> platformIds);
}