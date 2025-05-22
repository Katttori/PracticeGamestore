using PracticeGamestore.Business.DataTransferObjects;

namespace PracticeGamestore.Business.Services;

public interface IGameService
{
    Task<IEnumerable<GameDto>> GetAllAsync();
    Task<bool> UpdateAsync(GameDto game);
    Task<Guid?> CreateAsync(GameDto game);
    Task<GameDto?> GetByIdAsync(Guid id);
    Task DeleteAsync(Guid id);
}