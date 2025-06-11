using PracticeGamestore.Business.Constants;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Enums;
using PracticeGamestore.Business.Services.Blacklist;
using PracticeGamestore.Business.Services.Country;

namespace PracticeGamestore.Business.Services.Location;

public class LocationService(ICountryService countryService, IBlacklistService blacklistService) : ILocationService
{
    public async Task HandleLocationAccessAsync(string countryName, string userEmail)
    {
        if (string.IsNullOrWhiteSpace(countryName))
            throw new ArgumentException(ErrorMessages.MissingLocationHeader);
        
        var country = await countryService.GetByNameAsync(countryName);

        if (country is null)
        {
            await countryService.CreateAsync(new CountryDto(null, countryName, CountryStatus.Allowed));
            return;
        }

        if (country.Status == CountryStatus.Banned)
        {
            await blacklistService.CreateAsync(new BlacklistDto(null, userEmail, country.Id!.Value));
            throw new UnauthorizedAccessException(ErrorMessages.BlacklistedUser);
        }
    }
}