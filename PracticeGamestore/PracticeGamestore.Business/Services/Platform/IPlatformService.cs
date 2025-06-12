using PracticeGamestore.Business.DataTransferObjects;
namespace PracticeGamestore.Business.Services.Platform;

public interface IPlatformService
{ 
    Task<IEnumerable<PlatformDto>> GetAllAsync(); 
    Task<PlatformDto?> GetByIdAsync(Guid id); 
    Task<Guid?> CreateAsync(PlatformDto platform); 
    Task<bool> UpdateAsync(PlatformDto platform); 
    Task DeleteAsync(Guid id);
    Task<IEnumerable<GameResponseDto>?> GetGamesAsync(Guid platformId, bool hideAdultContent = false);
}