using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.DataAccess.Entities;

namespace PracticeGamestore.Business.Mappers;

public static class BlacklistMappingExtensions
{
    public static Blacklist MapToBlacklistEntity(this BlacklistDto dto)
    {
        return new()
        {
            Id = dto.Id,
            UserEmail = dto.UserEmail,
            CountryId = dto.CountryId,
        };
    }

    public static BlacklistDto MapToBlacklistDto(this Blacklist entity)
    {
        return new(entity.Id, entity.UserEmail, entity.CountryId);
    }
}