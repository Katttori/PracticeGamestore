using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Enums;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.Business.Utils;
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

        dto.Status = isInBlacklist ? UserStatus.Banned : UserStatus.Active;

        if (await blacklistRepository.IsInBlacklistAsync(dto.Email, dto.CountryId))
        {
            dto.Status = UserStatus.Banned;
        }
        
        dto.PasswordSalt = Guid.NewGuid().ToString();
        dto.Password = PasswordHasher.HashPassword(dto.Password, dto.PasswordSalt);
        
        var entity = dto.MapToUserEntity();
        var createdId = await userRepository.CreateAsync(entity);
        var changes = await unitOfWork.SaveChangesAsync();
        
        return changes > 0 ? createdId : null;
    }

    public async Task<bool> UpdateAsync(Guid id, UserDto dto)
    {
        var existingUser = await GetByIdAsync(id);
        if (existingUser is null) return false;
        dto.PasswordSalt = Guid.NewGuid().ToString();
        dto.Password = PasswordHasher.HashPassword(dto.Password, dto.PasswordSalt);
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
}