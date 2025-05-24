using PracticeGamestore.Business.DataTransferObjects;

namespace PracticeGamestore.Business.Services.Game;

public interface IGameService
{
    Task<IEnumerable<GameResponseDto>> GetAllAsync();
    Task<bool> UpdateAsync(GameRequestDto gameRequest);
    Task<Guid?> CreateAsync(GameRequestDto gameRequest);
    Task<GameResponseDto?> GetByIdAsync(Guid id);
    Task DeleteAsync(Guid id);
}