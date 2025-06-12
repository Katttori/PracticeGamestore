using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.DataAccess.Repositories.Blacklist;
using PracticeGamestore.DataAccess.Repositories.User;
using PracticeGamestore.DataAccess.UnitOfWork;

namespace PracticeGamestore.Business.Services.User;

public class UserService(
    IUserRepository userRepository,
    IBlacklistRepository blacklistRepository,
    IUnitOfWork unitOfWork) : IUserService
{
    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await userRepository.GetAllAsync();
        return users.Select(u => u.MapToUserDto());
    }
    
    public async Task<UserDto?> GetByIdAsync(Guid id)
    {
        var user = await userRepository.GetByIdAsync(id);
        return user?.MapToUserDto();
    }
    
    public async Task<Guid?> CreateAsync(UserDto dto)
    {
        var isInBlacklist = await blacklistRepository.ExistsByUserEmailAsync(dto.Email);

        dto.Status = isInBlacklist ? "Banned" : "Active";

        var blacklistCountries = await blacklistRepository.GetAllAsync();
        if (blacklistCountries.Any(b => b.UserEmail == dto.Email && b.CountryId != dto.CountryId))
        {
            dto.Status = "Banned";
        }
        
        dto.Role = string.IsNullOrWhiteSpace(dto.Role) ? "User" : dto.Role;
        
        dto.Password = HashPassword(dto.Password);
        
        var entity = dto.MapToUserEntity();
        var createdId = await userRepository.CreateAsync(entity);
        var changes = await unitOfWork.SaveChangesAsync();
        
        return changes > 0 ? createdId : null;
    }

    public async Task<bool> UpdateAsync(Guid id, UserDto dto)
    {
        var existingUser = await GetByIdAsync(id);
        if (existingUser is null) return false;
        
        dto.Password = HashPassword(dto.Password);
        var updatedUser = dto.MapToUserEntity();
        updatedUser.Id = id;
        await userRepository.Update(updatedUser);
        var changes = await unitOfWork.SaveChangesAsync();
        return changes > 0;
    }
    
    public async Task DeleteAsync(Guid id)
    {
        await userRepository.DeleteAsync(id);
        await unitOfWork.SaveChangesAsync();
    }
    
    public async Task<bool> BanUserAsync(Guid id)
    {
        var success = await userRepository.BanAsync(id);
        if (!success) return false;
        
        var changes = await unitOfWork.SaveChangesAsync();
        return changes > 0;
    }

    private string HashPassword(string password)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var salt = Guid.NewGuid().ToString();
        var bytes = System.Text.Encoding.UTF8.GetBytes(password + salt);
        var hash = sha256.ComputeHash(bytes);
        
        return Convert.ToBase64String(hash);
    }
}