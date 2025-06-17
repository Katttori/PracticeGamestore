using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Enums;
using PracticeGamestore.DataAccess.Entities;

namespace PracticeGamestore.Business.Mappers;

public static class UserMappingExtensions
{
    public static UserDto MapToUserDto(this User user)
    {
        return new(
            user.Id,
            user.UserName,
            user.Email,
            user.PhoneNumber,
            user.PasswordHash,
            user.PasswordSalt,
            user.Role,
            (UserStatus)user.Status,
            user.CountryId,
            user.BirthDate
            );
    }
    
    public static User MapToUserEntity(this UserDto userDto)
    {
        return new()
        {
            UserName = userDto.UserName.Trim(),
            Email = userDto.Email.Trim(),
            PhoneNumber = userDto.PhoneNumber,
            PasswordHash = userDto.Password,
            PasswordSalt = userDto.PasswordSalt!,
            Role = userDto.Role,
            Status = (DataAccess.Enums.UserStatus)userDto.Status,
            CountryId = userDto.CountryId,
            BirthDate = userDto.BirthDate
        };
    }
}