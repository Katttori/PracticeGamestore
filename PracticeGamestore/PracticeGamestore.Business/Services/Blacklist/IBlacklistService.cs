using PracticeGamestore.Business.DataTransferObjects;

namespace PracticeGamestore.Business.Services.Blacklist;

public interface IBlacklistService
{
    Task<IEnumerable<BlacklistDto>> GetAllAsync();
    Task<BlacklistDto?> GetByIdAsync(Guid id);
    Task<Guid?> CreateAsync(BlacklistDto model);
    Task<bool> UpdateAsync(Guid id, BlacklistDto model);
    Task DeleteAsync(Guid id);
}