using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.DataAccess.Entities;

namespace PracticeGamestore.Business.Mappers;

public static class CountryMappingExtensions
{
    public static CountryDto MapToCountryDto(this Country country)
    {
        return new CountryDto(country.Id, country.Name, country.CountryStatus);
    }
    
    public static Country MapToCountryEntity(this CountryDto countryDto)
    {
        return new Country
        {
            Name = countryDto.Name,
            CountryStatus = countryDto.Status
        };
    }
}