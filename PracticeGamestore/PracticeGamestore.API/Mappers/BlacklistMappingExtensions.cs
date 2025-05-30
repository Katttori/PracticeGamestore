using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Models.Blacklist;

namespace PracticeGamestore.Mappers;

public static class BlacklistMappingExtensions
{
    public static BlacklistDto MapToBlacklistDto(this BlacklistRequestModel model)
    {
        return new BlacklistDto(null, model.UserEmail, model.CountryId);
    }
    
    public static BlacklistResponseModel MapToBlacklistModel(this BlacklistDto dto)
    {
        return new ()
        {
            Id = dto.Id,
            UserEmail = dto.UserEmail,
            CountryId = dto.CountryId
        };
    }
}