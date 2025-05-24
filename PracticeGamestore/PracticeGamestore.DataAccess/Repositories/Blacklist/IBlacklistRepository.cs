namespace PracticeGamestore.DataAccess.Repositories.Blacklist;

public interface IBlacklistRepository
{
    Task<IEnumerable<Entities.Blacklist>> GetAllAsync();
    Task<Entities.Blacklist?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(Entities.Blacklist list);
    void Update(Entities.Blacklist list);
    Task DeleteAsync(Guid id);
}