namespace PracticeGamestore.DataAccess.Repositories.Genre;

public interface IGenreRepository
{
    public Task<IEnumerable<Entities.Genre>> GetAllAsync();
    public Task<Entities.Genre?> GetByIdAsync(Guid id);
    public Task<Guid> CreateAsync(Entities.Genre genre);
    public Task<bool> UpdateAsync(Entities.Genre genre);
    public Task DeleteAsync(Guid id);
}