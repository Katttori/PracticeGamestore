using Microsoft.EntityFrameworkCore;

namespace PracticeGamestore.DataAccess.Repositories.File;

public class FileRepository(GamestoreDbContext context) : IFileRepository
{
    private readonly IQueryable<Entities.File> _filesNoTracking =
        context.Files.AsNoTracking();
    
    public async Task<IEnumerable<Entities.File>> GetAllAsync()
    {
        return await _filesNoTracking.ToListAsync();
    }
    
    public async Task<Entities.File?> GetByIdAsync(Guid id)
    {
        return await _filesNoTracking.FirstOrDefaultAsync(f => f.Id == id);
    }
    
    public async Task<Guid> CreateAsync(Entities.File file)
    {
        var entity = await context.Files.AddAsync(file);
        return entity.Entity.Id;
    }
    
    public void Update(Entities.File file)
    {
        context.Files.Update(file);
    }
    
    public async Task DeleteAsync(Guid id)
    {
        var file = await context.Files.FindAsync(id);
        if (file == null) return;

        context.Files.Remove(file);
    }
    
    public async Task BeginTransactionAsync()
    {
        await context.Database.BeginTransactionAsync();
    }
}