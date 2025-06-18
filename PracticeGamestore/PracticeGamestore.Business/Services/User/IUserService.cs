using PracticeGamestore.Business.DataTransferObjects;

namespace PracticeGamestore.Business.Services.User;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto?> GetByIdAsync(Guid id);
    Task<Guid?> CreateAsync(UserDto model);
    Task<bool> UpdateAsync(Guid id, UserDto model);
    Task DeleteAsync(Guid id);
    Task<bool> BanUserAsync(Guid id);
}