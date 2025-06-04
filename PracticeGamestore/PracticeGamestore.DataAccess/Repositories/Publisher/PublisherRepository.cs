using Microsoft.EntityFrameworkCore;

namespace PracticeGamestore.DataAccess.Repositories.Publisher;

public class PublisherRepository(GamestoreDbContext context) : IPublisherRepository
{
    private readonly IQueryable<Entities.Publisher> _publisherNoTracking = context.Publishers.AsNoTracking();
    
    public async Task<IEnumerable<Entities.Publisher>> GetAllAsync()
    {
        return await _publisherNoTracking.ToListAsync();
    }

    public async Task<Entities.Publisher?> GetByIdAsync(Guid id)
    {
        return await _publisherNoTracking.FirstOrDefaultAsync(p => id == p.Id);
    }

    public async Task<Guid> CreateAsync(Entities.Publisher publisher)
    {
        var entry = await context.Publishers.AddAsync(publisher);
        return entry.Entity.Id;
    }

    public void Update(Entities.Publisher publisher)
    {
        context.Publishers.Update(publisher);
    }

    public async Task DeleteAsync(Guid id)
    {
        var publisher = await context.Publishers.FindAsync(id);
        if (publisher is not null) context.Publishers.Remove(publisher);
    }

    public Task<bool> ExistsAsync(Guid id)
    {
        return _publisherNoTracking.AnyAsync(p => p.Id == id);
    }
}