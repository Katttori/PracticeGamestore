using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.DataTransferObjects.Filtering;

namespace PracticeGamestore.Business.Services.Game;

public interface IGameService
{
    Task<IEnumerable<GameResponseDto>> GetAllAsync(bool hideAdultContent = false );
    Task<bool> UpdateAsync(Guid id, GameRequestDto gameRequest);
    Task<(IEnumerable<GameResponseDto>, int)> GetFilteredAsync(GameFilter filter, bool hideAdultContent = false);
    Task<Guid?> CreateAsync(GameRequestDto gameRequest);
    Task<GameResponseDto?> GetByIdAsync(Guid id);
    Task DeleteAsync(Guid id);
}