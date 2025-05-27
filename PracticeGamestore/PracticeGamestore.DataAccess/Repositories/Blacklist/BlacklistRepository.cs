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

    public async Task<Guid> CreateAsync(Entities.Blacklist blacklist)
    {
        var b = await context.Blacklists.AddAsync(blacklist);
        return b.Entity.Id;
    }

    public void Update(Entities.Blacklist blacklist)
    {
        context.Blacklists.Update(blacklist);
    }

    public async Task DeleteAsync(Guid id)
    {
        var blacklist = await context.Blacklists.FindAsync(id);
        if (blacklist is null) return;

        context.Blacklists.Remove(blacklist);
    }
}