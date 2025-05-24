namespace PracticeGamestore.DataAccess.Repositories.Platform;

public interface IPlatformRepository
{
    public Task<IEnumerable<Entities.Platform>> GetAllAsync();
    public Task<Entities.Platform?> GetByIdAsync(Guid id);
    public Task<Guid> CreateAsync(Entities.Platform platform);
    public void Update(Entities.Platform platform);
    public Task DeleteAsync(Guid id);
}