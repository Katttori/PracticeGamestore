namespace PracticeGamestore.DataAccess.Repositories.Platform;

public interface IPlatformRepository
{
    Task<IEnumerable<Entities.Platform>> GetAllAsync();
    Task<Entities.Platform?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(Entities.Platform platform);
    void Update(Entities.Platform platform);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsByIdAsync(Guid platformId);
    Task<bool> ExistsByNameAsync(string name);
}