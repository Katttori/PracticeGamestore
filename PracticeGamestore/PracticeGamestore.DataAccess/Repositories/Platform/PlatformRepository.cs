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
    
    public void Update(Entities.Platform platform)
    {
        context.Platforms.Update(platform);
    }
    
    public async Task DeleteAsync(Guid id)
    {
        var platform = await context.Platforms.FindAsync(id);
        if (platform is null) return;
        
        context.Platforms.Remove(platform);
    }
    
    public async Task<bool> ExistsByIdAsync(Guid platformId)
    {
        return await _platformsNoTracking.AnyAsync(p => p.Id == platformId);
    }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await _platformsNoTracking.AnyAsync(p => p.Name == name);
    }
}