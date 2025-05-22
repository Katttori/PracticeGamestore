using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.DataAccess.Repositories.Platform;
using PracticeGamestore.DataAccess.UnitOfWork;

namespace PracticeGamestore.Business.Services.Platform;

public class PlatformService : IPlatformService
{
    private readonly IPlatformRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    
    public PlatformService(IPlatformRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IEnumerable<PlatformDto>> GetAllAsync()
    {
        var platforms = await _repository.GetAllAsync();
        return platforms.Select(p => p.MapToDto());
    }
    
    public async Task<PlatformDto?> GetByIdAsync(Guid id)
    {
        var platform = await _repository.GetByIdAsync(id);
        return platform?.MapToDto();
    }
    
    public async Task<Guid?> CreateAsync(PlatformDto platform)
    {
        var entity = platform.MapToEntity();
        var id = await _repository.CreateAsync(entity);
        var changes = await _unitOfWork.SaveChangesAsync();
        
        return changes > 0 ? entity.Id : null;
    }
    
    public async Task<bool> UpdateAsync(PlatformDto platform)
    {
        var updatedPlatform = await _repository.GetByIdAsync(platform.Id);
        if (updatedPlatform is null) return false;
        
        updatedPlatform.Name = platform.Name;
        updatedPlatform.Description = platform.Description;
        
        _repository.Update(updatedPlatform);
        var changes = await _unitOfWork.SaveChangesAsync();
        return changes > 0;
    }
    
    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }
}