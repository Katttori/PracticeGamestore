using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.DataAccess.Repositories.Platform;
using PracticeGamestore.DataAccess.UnitOfWork;

namespace PracticeGamestore.Business.Services.Platform;

public class PlatformService(IPlatformRepository repository, IUnitOfWork unitOfWork) : IPlatformService
{
    
    public async Task<IEnumerable<PlatformDto>> GetAllAsync()
    {
        var platforms = await repository.GetAllAsync();
        return platforms.Select(p => p.MapToPlatformDto());
    }
    
    public async Task<PlatformDto?> GetByIdAsync(Guid id)
    {
        var platform = await repository.GetByIdAsync(id);
        return platform?.MapToPlatformDto();
    }
    
    public async Task<Guid?> CreateAsync(PlatformDto platform)
    {
        var entity = platform.MapToPlatformEntity();
        var id = await repository.CreateAsync(entity);
        var changes = await unitOfWork.SaveChangesAsync();
        
        return changes > 0 ? entity.Id : null;
    }
    
    public async Task<bool> UpdateAsync(PlatformDto platform)
    {
        var platformEntity = platform.MapToPlatformEntity();
        var updatedPlatform = await repository.GetByIdAsync(platformEntity.Id);
        if (updatedPlatform is null) return false;
        
        updatedPlatform.Name = platform.Name;
        updatedPlatform.Description = platform.Description;
        
        repository.Update(updatedPlatform);
        var changes = await unitOfWork.SaveChangesAsync();
        return changes > 0;
    }
    
    public async Task DeleteAsync(Guid id)
    {
        await repository.DeleteAsync(id);
        await unitOfWork.SaveChangesAsync();
    }
}