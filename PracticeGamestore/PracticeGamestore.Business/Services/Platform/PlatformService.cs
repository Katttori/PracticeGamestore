using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.DataAccess.Repositories.Platform;
using PracticeGamestore.DataAccess.UnitOfWork;

namespace PracticeGamestore.Business.Services.Platform;

public class PlatformService: IPlatformService
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
        await _unitOfWork.SaveChangesAsync();
        return id != Guid.Empty ? id : null;
    }
    
    public async Task<bool> UpdateAsync(PlatformDto platform)
    {
        var p = await _repository.GetByIdAsync(platform.Id);
        if (p is null) return false;
        
        p.Name = platform.Name;
        p.Description = platform.Description;
        
        var updatedPlatform = await _repository.UpdateAsync(p);
        if (updatedPlatform is null) return false;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
    
    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }
}