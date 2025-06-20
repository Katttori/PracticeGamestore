using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Models.Platform;

namespace PracticeGamestore.Mappers;

public static class PlatformMappingExtensions
{
    public static PlatformDto MapToPlatformDto(this PlatformRequestModel platform)
    {
        return new PlatformDto(null, platform.Name, platform.Description);
    }
    
    public static PlatformResponseModel MapToPlatformModel(this PlatformDto platform)
    {
        return new()
        {
            Id = platform.Id!.Value,
            Name = platform.Name,
            Description = platform.Description
        };
    }
}