using Microsoft.EntityFrameworkCore;
using PracticeGamestore.DataAccess.Entities;

namespace PracticeGamestore.DataAccess.Repositories;

public class PublisherRepository(GamestoreDbContext context) : IPublisherRepository
{
    private readonly IQueryable<Publisher> _publisherNoTracking = context.Publishers.AsNoTracking();
    
    public async Task<IEnumerable<Publisher>> GetAllAsync()
    {
        return await _publisherNoTracking.ToListAsync();
    }

    public async Task<Publisher?> GetByIdAsync(Guid id)
    {
        return await _publisherNoTracking.FirstOrDefaultAsync(p => id == p.Id);
    }

    public async Task<Guid> CreateAsync(Publisher publisher)
    {
        var entry = await context.Publishers.AddAsync(publisher);
        return entry.Entity.Id;
    }

    public void Update(Publisher publisher)
    {
        context.Publishers.Update(publisher);
    }

    public async Task DeleteAsync(Guid id)
    {
        var publisher = await context.Publishers.FindAsync(id);
        if (publisher is not null) context.Publishers.Remove(publisher);
    }
}