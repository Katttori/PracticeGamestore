using System.Collections;
using PracticeGamestore.Business.DataTransferObjects;

namespace PracticeGamestore.Business.Services;

public interface IGameService
{
    public Task<IEnumerable<GameDto>> GetAllAsync();
    public Task<bool> UpdateAsync(GameDto game);
    public Task<Guid?> CreateAsync(GameDto game);
    public Task<GameDto?> GetByIdAsync(Guid id);
    public Task DeleteAsync(Guid id);
}