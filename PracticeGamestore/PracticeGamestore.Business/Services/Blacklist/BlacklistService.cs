using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.DataAccess.Repositories.Blacklist;
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
        if (await blacklistRepository.ExistsByUserEmailAsync(dto.UserEmail))
        {
            throw new ArgumentException($"Blacklist entry with user email '{dto.UserEmail}' already exists.");
        }
        
        var createdId = await blacklistRepository.CreateAsync(dto.MapToBlacklistEntity());
        var changes = await unitOfWork.SaveChangesAsync();
        return changes > 0 ? createdId : null;
    }
    
    public async Task<bool> UpdateAsync(Guid id, BlacklistDto dto)
    {
        var entity = await blacklistRepository.GetByIdAsync(id);
        if (entity is null) return false;
        
        if (dto.UserEmail != entity.UserEmail && await blacklistRepository.ExistsByUserEmailAsync(dto.UserEmail))
        {
            throw new ArgumentException($"Blacklist entry with user email '{dto.UserEmail}' already exists.");
        }
        
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