using PracticeGamestore.API.Models;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.DataAccess.Enums;

namespace PracticeGamestore.API.Mappers;

public static class CountryMappingExtensions
{
    public static CountryDto MapToCountryDto(this CountryUpdateRequestModel countryUpdateRequestModel)
    {
        return new CountryDto(null, countryUpdateRequestModel.Name, countryUpdateRequestModel.Status);
    }
    
    public static CountryDto MapToCountryDto(this CountryCreateRequestModel countryCreateRequestModel)
    {
        return new CountryDto(null, countryCreateRequestModel.Name, CountryStatus.Allowed);
    }
    public static CountryResponseModel MapToCountryModel(this CountryDto countryDto)
    {
        return new()
        {
            Id = countryDto.Id ?? Guid.NewGuid(),
            Name = countryDto.Name,
            Status = countryDto.Status
        };
    }
}