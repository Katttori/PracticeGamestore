using PracticeGamestore.Business.DataTransferObjects;
namespace PracticeGamestore.Business.Services.Platform;

public interface IPlatformService
{
    public Task<IEnumerable<PlatformDto>> GetAllAsync();
    public Task<PlatformDto?> GetByIdAsync(Guid id);
    public Task<Guid?> CreateAsync(PlatformDto platform);
    public Task<bool> UpdateAsync(PlatformDto platform);
    public Task DeleteAsync(Guid id);
}