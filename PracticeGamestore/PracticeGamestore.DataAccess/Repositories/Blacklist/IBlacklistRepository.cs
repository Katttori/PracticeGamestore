namespace PracticeGamestore.DataAccess.Repositories.Blacklist;

public interface IBlacklistRepository
{
    Task<IEnumerable<Entities.Blacklist>> GetAllAsync();
    Task<Entities.Blacklist?> GetByIdAsync(Guid? id);
    Task<Guid> CreateAsync(Entities.Blacklist blacklist);
    void Update(Entities.Blacklist blacklist);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsByUserEmailAsync(string email);
}