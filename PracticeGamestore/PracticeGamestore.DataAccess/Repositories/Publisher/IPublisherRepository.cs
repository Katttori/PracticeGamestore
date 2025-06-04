namespace PracticeGamestore.DataAccess.Repositories.Publisher;

public interface IPublisherRepository
{
    Task<IEnumerable<Entities.Publisher>> GetAllAsync();
    Task<Entities.Publisher?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(Entities.Publisher publisher);
    void Update(Entities.Publisher publisher);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> ExistsByNameAsync(string name);
    Task<bool> ExistsByPageUrlAsync(string pageUrl);
}