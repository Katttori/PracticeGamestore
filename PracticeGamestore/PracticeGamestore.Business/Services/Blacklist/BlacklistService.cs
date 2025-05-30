using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.Business.Services.Country;
using PracticeGamestore.DataAccess.Repositories.Blacklist;
using PracticeGamestore.DataAccess.Repositories.Country;
using PracticeGamestore.DataAccess.UnitOfWork;

namespace PracticeGamestore.Business.Services.Blacklist;

public class BlacklistService(IBlacklistRepository blacklistRepository, IUnitOfWork unitOfWork) : IBlacklistService
{
    public async Task<IEnumerable<BlacklistDto>> GetAllAsync()
    {
        var entities = await blacklistRepository.GetAllAsync();
        return entities.Select(e => e.MapToBlacklistDto());
    }
    
    public async Task<BlacklistDto?> GetByIdAsync(Guid id)
    {
        var entity = await blacklistRepository.GetByIdAsync(id);
        return entity?.MapToBlacklistDto();
    }
    
    public async Task<Guid?> CreateAsync(BlacklistDto dto)
    {
        var createdId = await blacklistRepository.CreateAsync(dto.MapToBlacklistEntity());
        var changes = await unitOfWork.SaveChangesAsync();
        return changes > 0 ? createdId : null;
    }
    
    public async Task<bool> UpdateAsync(Guid id, BlacklistDto dto)
    {
        var entity = await blacklistRepository.GetByIdAsync(id);
        if (entity is null) return false;
        
        dto.Id = id;
        blacklistRepository.Update(dto.MapToBlacklistEntity());
        var changes = await unitOfWork.SaveChangesAsync();
        return changes > 0;
    }
    
    public async Task DeleteAsync(Guid id)
    {
        await blacklistRepository.DeleteAsync(id);
        await unitOfWork.SaveChangesAsync();
    }
}