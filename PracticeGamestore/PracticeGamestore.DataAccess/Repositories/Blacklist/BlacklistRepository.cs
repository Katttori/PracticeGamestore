using Microsoft.EntityFrameworkCore;

namespace PracticeGamestore.DataAccess.Repositories.Blacklist;

public class BlacklistRepository(GamestoreDbContext context) : IBlacklistRepository
{
    private readonly IQueryable<Entities.Blacklist> _blacklistsNoTracking = context.Blacklists.AsNoTracking();
    
    public async Task<IEnumerable<Entities.Blacklist>> GetAllAsync()
    {
        return await _blacklistsNoTracking.ToListAsync();
    }

    public async Task<Entities.Blacklist?> GetByIdAsync(Guid id)
    {
        return await _blacklistsNoTracking.FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Guid> CreateAsync(Entities.Blacklist list)
    {
        var entry = await context.Blacklists.AddAsync(list);
        return entry.Entity.Id;
    }

    public void Update(Entities.Blacklist list)
    {
        context.Blacklists.Update(list);
    }

    public async Task DeleteAsync(Guid id)
    {
        var list = await context.Blacklists.FindAsync(id);

        if (list is not null)
        {
            context.Blacklists.Remove(list);
        }
    }
}