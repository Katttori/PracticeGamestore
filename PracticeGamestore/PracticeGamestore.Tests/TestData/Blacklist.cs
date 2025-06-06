using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Models.Blacklist;

namespace PracticeGamestore.Tests.TestData;

public class Blacklist
{
    public static List<BlacklistDto> GenerateBlacklistDtos()
    {
        return
        [
            new(Guid.NewGuid(), "example@gmail.com", Guid.NewGuid()),
            new(Guid.NewGuid(), "example2@gmail.com", Guid.NewGuid())
        ];
    }
    
    public static List<DataAccess.Entities.Blacklist> GenerateBlacklistEntities()
    {
        return
        [
            new() { Id = Guid.NewGuid(), UserEmail = "user@example.com"},
            new() { Id = Guid.NewGuid(), UserEmail = "user2@example.com" },
        ];
    }

    public static BlacklistDto GenerateBlacklistDto()
    {
        return new(Guid.NewGuid(), "example@gmail.com", Guid.NewGuid());
    }
    
    public static DataAccess.Entities.Blacklist GenerateBlacklistEntity()
    {
        return new() { Id = Guid.NewGuid(), UserEmail = "user@example.com" };
    }
    
    public static BlacklistRequestModel GenerateBlacklistRequestModel()
    {
        return new() { CountryId = Guid.NewGuid(), UserEmail = "example@gmail.com" };
    }
}