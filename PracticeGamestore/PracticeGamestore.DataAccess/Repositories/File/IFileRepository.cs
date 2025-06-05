namespace PracticeGamestore.DataAccess.Repositories.File;

public interface IFileRepository
{
    Task <IEnumerable<Entities.File>> GetAllAsync();
    Task<Entities.File?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(Entities.File file);
    void Update(Entities.File file);
    Task DeleteAsync(Guid id);
    
    Task BeginTransactionAsync();
}