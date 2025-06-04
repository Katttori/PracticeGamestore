namespace PracticeGamestore.DataAccess.Repositories.Genre;

public interface IGenreRepository
{
    Task<IEnumerable<Entities.Genre>> GetAllAsync();
    Task<Entities.Genre?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(Entities.Genre genre);
    void Update(Entities.Genre genre);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<List<Guid>> GetGenreChildrenIdsAsync(Guid id);
}