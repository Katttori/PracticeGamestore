using PracticeGamestore.API.Models;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.DataAccess.Enums;

namespace PracticeGamestore.Tests.TestData;

public class Country
{
    public static List<CountryDto> GenerateCountryDtos()
    {
        return
        [
            new(Guid.NewGuid(), "Canada", CountryStatus.Allowed),
            new(Guid.NewGuid(), "USA", CountryStatus.Allowed)
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
        return new(Guid.NewGuid(), "Canada", CountryStatus.Allowed);
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
        return new() { Name = "UK", Status = CountryStatus.Banned };
    }
}