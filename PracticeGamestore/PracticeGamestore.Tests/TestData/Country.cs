using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.DataAccess.Enums;
using PracticeGamestore.Models.Country;

namespace PracticeGamestore.Tests.TestData;

public class Country
{
    public static List<CountryDto> GenerateCountryDtos()
    {
        return
        [
            new(Guid.NewGuid(), "Canada", Business.Enums.CountryStatus.Allowed),
            new(Guid.NewGuid(), "USA", Business.Enums.CountryStatus.Allowed)
        ];
    }

    public static List<DataAccess.Entities.Country> GenerateCountryEntities()
    {
        return
        [
            new()
            {
                Id = Guid.NewGuid(),
                Name = "USA",
                CountryStatus = CountryStatus.Allowed
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Canada",
                CountryStatus = CountryStatus.Allowed
            }
        ];
    }

    public static CountryDto GenerateCountryDto()
    {
        return new(Guid.NewGuid(), "Canada", Business.Enums.CountryStatus.Allowed);
    }
    
    public static CountryDto GenerateBannedCountryDto()
    {
        return new(Guid.NewGuid(), "russia", Business.Enums.CountryStatus.Banned);
    }
    
    public static DataAccess.Entities.Country GenerateCountryEntity()
    {
        return new() { Id = Guid.NewGuid(), Name = "USA", CountryStatus = CountryStatus.Allowed };
    }
    
    public static CountryCreateRequestModel GenerateCountryCreateRequestModel()
    {
        return new() { Name = "Canada" };
    }
    
    public static CountryUpdateRequestModel GenerateCountryUpdateRequestModel()
    {
        return new() { Name = "UK", Status = Business.Enums.CountryStatus.Banned };
    }
}