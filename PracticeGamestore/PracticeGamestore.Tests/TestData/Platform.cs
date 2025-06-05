using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Models.Platform;

namespace PracticeGamestore.Tests.TestData;

public class Platform
{
    public static List<PlatformDto> GeneratePlatformDtos()
    {
        return
        [
            new(Guid.NewGuid(), "PC", "Personal Computer"),
            new(Guid.NewGuid(), "PS5", "PlayStation 5")
        ];
    }
    
    public static List<DataAccess.Entities.Platform> GeneratePlatformEntities()
    {
        return  new()
        {
            new() {Id = Guid.NewGuid(), Name = "PC", Description = "Personal Computer"},
            new() {Id = Guid.NewGuid(), Name = "PS5", Description = "PlayStation 5"},
            new() {Id = Guid.NewGuid(), Name = "Xbox Series X", Description = "Microsoft Xbox Series X"},
            new() {Id = Guid.NewGuid(), Name = "Nintendo Switch", Description = "Nintendo Switch Console"},
            new() {Id = Guid.NewGuid(), Name = "Steam Deck", Description = "Valve Steam Deck"}
        };
    }

    public static PlatformDto GeneratePlatformDto()
    {
        return new(Guid.NewGuid(), "PC", "Personal Computer");
    }

    public static DataAccess.Entities.Platform GeneratePlatformEntity()
    {
        return new() { Id = Guid.NewGuid(), Name = "PC", Description = "Personal Computer" };
    }

    public static PlatformRequestModel GeneratePlatformRequestModel()
    {
        return new() { Name = "PC", Description = "Personal Computer" };
    }
}