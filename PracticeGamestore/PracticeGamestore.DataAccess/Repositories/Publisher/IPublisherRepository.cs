using PracticeGamestore.DataAccess.Entities;

namespace PracticeGamestore.DataAccess.Repositories;

public interface IPublisherRepository
{
    Task<IEnumerable<Publisher>> GetAllAsync();
    Task<Publisher?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(Entities.Publisher publisher);
    void Update(Publisher publisher);
    Task DeleteAsync(Guid id);
}