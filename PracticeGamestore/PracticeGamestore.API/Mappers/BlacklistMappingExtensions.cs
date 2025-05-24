using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Models.Blacklist;

namespace PracticeGamestore.Mappers;

public static class BlacklistMappingExtensions
{
    public static BlacklistDto MapToBlacklistDto(this BlacklistRequestModel dto)
    {
        return new BlacklistDto(null, dto.UserEmail, null);
    }
    
    public static BlacklistResponseModel MapToBlacklistModel(this BlacklistDto dto)
    {
        return new BlacklistResponseModel
        {
            Id = dto.Id,
            UserEmail = dto.UserEmail,
            CountryId = dto.CountryId ?? Guid.Empty
        };
    }
}