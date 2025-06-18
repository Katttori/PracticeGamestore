namespace PracticeGamestore.DataAccess.Repositories.User;

public interface IUserRepository
{
    Task<IEnumerable<Entities.User>> GetAllAsync();
    Task<Entities.User?> GetByIdAsync(Guid id);
    Task<Entities.User?> GetByEmailAsync(string email);
    Task<Guid?> CreateAsync(Entities.User user);
    Task Update(Entities.User user);
    Task DeleteAsync(Guid id);
    Task<bool> BanAsync(Guid id);
}