using Microsoft.EntityFrameworkCore;

namespace PracticeGamestore.DataAccess.Repositories.Platform;

public class PlatformRepository(GamestoreDbContext context): IPlatformRepository
{
    private readonly IQueryable<Entities.Platform> _platformsNoTracking = context.Platforms.AsNoTracking();
    
    public async Task<IEnumerable<Entities.Platform>> GetAllAsync()
    {
        return await _platformsNoTracking.ToListAsync();
    }
    
    public async Task<Entities.Platform?> GetByIdAsync(Guid id)
    {
        return await _platformsNoTracking.FirstOrDefaultAsync(p => p.Id == id);
    }
    
    public async Task<Guid> CreateAsync(Entities.Platform platform)
    {
        var p = await context.Platforms.AddAsync(platform);
        return p.Entity.Id;
    }
    
    public async Task<Entities.Platform?> UpdateAsync(Entities.Platform platform)
    {
        var p = await context.Platforms.FirstOrDefaultAsync(p => p.Id == platform.Id);
        if (p is null) return null;
        
        p.Name = platform.Name;
        p.Description = platform.Description;
        
        context.Platforms.Update(p);
        return p;
    }
    
    public async Task DeleteAsync(Guid id)
    {
        var platform = await context.Platforms.FindAsync(id);
        if (platform is null) return;
        
        context.Platforms.Remove(platform);
    }
}