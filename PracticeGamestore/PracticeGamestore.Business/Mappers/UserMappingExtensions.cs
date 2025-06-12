using PracticeGamestore.Business.DataTransferObjects;
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
            user.Role,
            user.Status,
            user.CountryId,
            user.BirthDate
            );
    }
    
    public static User MapToUserEntity(this UserDto userDto)
    {
        return new()
        {
            UserName = userDto.UserName,
            Email = userDto.Email,
            PhoneNumber = userDto.PhoneNumber,
            PasswordHash = userDto.PasswordHash,
            Role = userDto.Role,
            Status = userDto.Status,
            CountryId = userDto.CountryId,
            BirthDate = userDto.BirthDate
        };
    }
    
}