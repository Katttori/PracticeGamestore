using PracticeGamestore.DataAccess.Entities;

namespace PracticeGamestore.DataAccess.Repositories;

public interface IGameRepository
{
    Task<IEnumerable<Game>> GetAllAsync();
    Task<Game?> GetByIdAsync(Guid id);
    Task DeleteAsync(Guid id);
    Task<Guid> CreateAsync(Game game, List<Guid> genreIds, List<Guid> platformIds);
    Task UpdateAsync(Game game, List<Guid> genreIds, List<Guid> platformIds);
}