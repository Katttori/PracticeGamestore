using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.DataAccess.Entities;

namespace PracticeGamestore.Business.Mappers;

public static class CountryMappingExtensions
{
    public static CountryDto MapToCountryDto(this Country country)
    {
        return new(country.Id, country.Name, country.CountryStatus);
    }
    
    public static Country MapToCountryEntity(this CountryDto countryDto)
    {
        return new()
        {
            Id = countryDto.Id,
            Name = countryDto.Name,
            CountryStatus = countryDto.Status
        };
    }
}