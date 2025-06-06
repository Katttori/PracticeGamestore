using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.DataTransferObjects.Filtering;

namespace PracticeGamestore.Business.Services.Game;

public interface IGameService
{
    Task<IEnumerable<GameResponseDto>> GetAllAsync();
    Task<bool> UpdateAsync(Guid id, GameRequestDto gameRequest);
    Task<(IEnumerable<GameResponseDto>, int)> GetFilteredAsync(GameFilter filter);
    Task<Guid?> CreateAsync(GameRequestDto gameRequest);
    Task<GameResponseDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<GameResponseDto>?> GetByPlatformAsync(Guid platformId);
    Task DeleteAsync(Guid id);
}