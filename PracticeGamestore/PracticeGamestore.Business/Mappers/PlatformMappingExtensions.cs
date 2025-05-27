using PracticeGamestore.DataAccess.Entities;
using PracticeGamestore.Business.DataTransferObjects;
namespace PracticeGamestore.Business.Mappers;

public static class PlatformMappingExtensions
{
    public static Platform MapToEntity(this PlatformDto dto)
    {
        if (dto.Id.HasValue)
        {
            return new Platform
            {
                Id = dto.Id.Value,
                Name = dto.Name,
                Description = dto.Description
            };
        }
        
        return new Platform
        {
            Name = dto.Name,
            Description = dto.Description
        };
    }
    
    public static PlatformDto MapToDto(this Platform entity)
    {
        return new PlatformDto(entity.Id, entity.Name, entity.Description);
    }
}