using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Enums;
using PracticeGamestore.DataAccess.Entities;

namespace PracticeGamestore.Business.Mappers;

public static class CountryMappingExtensions
{
    public static CountryDto MapToCountryDto(this Country country)
    {
        return new CountryDto(country.Id, country.Name, (CountryStatus)country.CountryStatus);
    }
    
    public static Country MapToCountryEntity(this CountryDto countryDto)
    {
        if (countryDto.Id.HasValue)
        {
            return new Country
            {
                Id = countryDto.Id.Value,
                Name = countryDto.Name,
                CountryStatus = (DataAccess.Enums.CountryStatus)countryDto.Status
            };
        }
        return new Country
        {
            Name = countryDto.Name,
            CountryStatus = (DataAccess.Enums.CountryStatus)countryDto.Status
        };
    }
}