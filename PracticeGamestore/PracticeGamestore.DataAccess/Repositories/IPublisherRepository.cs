using PracticeGamestore.DataAccess.Entities;
namespace PracticeGamestore.DataAccess.Repositories;

public interface IPublisherRepository
{
    public Task<IEnumerable<Publisher>> GetAllAsync();
    public Task<Publisher?> GetByIdAsync(Guid id);
    public Task<Guid> CreateAsync(Entities.Publisher publisher);
    public void Update(Publisher publisher);
    public Task DeleteAsync(Guid id);
}