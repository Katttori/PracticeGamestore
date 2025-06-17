using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Enums;
using PracticeGamestore.Models.User;

namespace PracticeGamestore.Mappers;

public static class UserMappingExtensions
{
    public static UserDto MapToUserDto(this UserRequestModel model)
    {
        return new (
            null,
            model.UserName,
            model.Email,
            model.PhoneNumber, 
            model.Password,
            null,
            model.Role,
            UserStatus.Active,
            model.CountryId,
            model.BirthDate
        );
    }
    
    public static UserResponseModel MapToUserResponseModel(this UserDto userDto)
    {
        return new UserResponseModel
        {
            Id = userDto.Id!.Value,
            UserName = userDto.UserName,
            Email = userDto.Email,
            PhoneNumber = userDto.PhoneNumber,
            Role = userDto.Role,
            Status = userDto.Status,
            CountryId = userDto.CountryId,
            BirthDate = userDto.BirthDate
        };
    }
}