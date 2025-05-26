using PracticeGamestore.API.Models;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.DataAccess.Enums;

namespace PracticeGamestore.API.Mappers;

public static class CountryMappingExtensions
{
    public static CountryDto MapToCountryDto(this CountryRequestModel countryRequestModel)
    {
        return new(Guid.Empty, countryRequestModel.Name, CountryStatus.Allowed);
    }
    
    public static CountryResponseModel MapToCountryModel(this CountryDto countryDto)
    {
        return new()
        {
            Id = countryDto.Id,
            Name = countryDto.Name,
            Status = countryDto.Status
        };
    }
}