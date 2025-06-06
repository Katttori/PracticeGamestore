using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Models.Publisher;

namespace PracticeGamestore.Tests.TestData;

public static class Publisher
{
    public static List<DataAccess.Entities.Publisher> GeneratePublisherEntities()
    {
        return new ()
        {
            new() { Id = Guid.NewGuid(), Name = "Electronic Arts", Description = "American video game company", PageUrl = "https://www.ea.com" },
            new() { Id = Guid.NewGuid(), Name = "Ubisoft", Description = "French video game company", PageUrl = "https://www.ubisoft.com" },
            new() { Id = Guid.NewGuid(), Name = "Activision Blizzard", Description = "American video game holding company", PageUrl = "https://www.activisionblizzard.com" },
            new() { Id = Guid.NewGuid(), Name = "CD Projekt", Description = "Polish video game developer", PageUrl = "https://www.cdprojekt.com" },
            new() { Id = Guid.NewGuid(), Name = "Rockstar Games", Description = "American video game publisher", PageUrl = "https://www.rockstargames.com" },
            new() { Id = Guid.NewGuid(), Name = "Nintendo", Description = "Japanese multinational video game company", PageUrl = "https://www.nintendo.com" },
            new() { Id = Guid.NewGuid(), Name = "Valve Corporation", Description = "American video game developer", PageUrl = "https://www.valvesoftware.com" },
            new() { Id = Guid.NewGuid(), Name = "Bethesda Game Studios", Description = "American video game developer", PageUrl = "https://bethesdagamestudios.com" }
        };
    }
    
    public static List<PublisherDto> GeneratePublisherDtos()
    {
        return new()
        {
            new(Guid.NewGuid(), "Electronic Arts", "American video game company", "https://www.ea.com"),
            new(Guid.NewGuid(), "Ubisoft", "French video game company", "https://www.ubisoft.com"),
            new(Guid.NewGuid(), "Activision Blizzard", "American video game holding company", "https://www.activisionblizzard.com"),
            new (Guid.NewGuid(), "CD Projekt", "Polish video game developer", "https://www.cdprojekt.com" ),
            new (Guid.NewGuid(), "Nintendo", "American video game publisher", "https://www.rockstargames.com" ),
            new (Guid.NewGuid(), "Valve Corporation",  "Japanese multinational video game company", "https://www.nintendo.com" ),
            new (Guid.NewGuid(), "Bethesda Game Studios", "American video game developer", "https://bethesdagamestudios.com" ),
     };
    }

    public static DataAccess.Entities.Publisher GeneratePublisherEntity(Guid? id = null)
    {
        return new()
        {
            Id = id ?? Guid.NewGuid(), Name = "Electronic Arts", Description = "American video game company",
            PageUrl = "https://www.ea.com"
        };
    }
    
    public static PublisherDto GeneratePublisherDto(Guid? id = null)
    {
        return  new (id ?? Guid.NewGuid(), "Electronic Arts", "American video game company", "https://www.ea.com");

    }
    
    public static PublisherRequestModel GeneratePublisherRequestModel()
    {
        return new()
        {
            Name = "Electronic Arts",
            Description = "American video game company",
            PageUrl = "https://www.ea.com"
        };
    }
}