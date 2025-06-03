using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.DataAccess.Entities;

namespace PracticeGamestore.Business.Mappers;

public static class BlacklistMappingExtensions
{
    public static Blacklist MapToBlacklistEntity(this DataTransferObjects.BlacklistDto dto)
    {
        if (dto.Id.HasValue)
        {
            return new Blacklist
            {
                Id = dto.Id.Value,
                UserEmail = dto.UserEmail,
                CountryId = dto.CountryId,
            };
        }
        
        return new Blacklist
        {
            UserEmail = dto.UserEmail,
            CountryId = dto.CountryId,
        };
    }
    
    public static BlacklistDto MapToBlacklistDto(this Blacklist entity)
    {
        return new (entity.Id, entity.UserEmail, entity.CountryId);
    }
}