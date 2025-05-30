using PracticeGamestore.DataAccess.Entities;
using PracticeGamestore.Business.DataTransferObjects;
namespace PracticeGamestore.Business.Mappers;

public static class PlatformMappingExtensions
{
    public static Platform MapToEntity(this PlatformDto dto)
    {
        return new Platform
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description
        };
    }
    
    public static PlatformDto MapToPlatformDto(this Platform entity)
    {
        return new PlatformDto(entity.Id, entity.Name, entity.Description);
    }
}