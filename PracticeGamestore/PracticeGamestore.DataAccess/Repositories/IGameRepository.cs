using PracticeGamestore.DataAccess.Entities;

namespace PracticeGamestore.DataAccess.Repositories;

public interface IGameRepository
{
    public Task<IEnumerable<Game>> GetAllAsync();
    public Task<Game?> GetByIdAsync(Guid id);
    public Task DeleteAsync(Guid id);
    public Task<Guid> CreateAsync(Game game, List<Guid> genreIds, List<Guid> platformIds);
    public Task UpdateAsync(Game game, List<Guid> genreIds, List<Guid> platformIds);
}