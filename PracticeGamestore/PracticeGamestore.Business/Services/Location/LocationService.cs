using Microsoft.AspNetCore.Http;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Enums;
using PracticeGamestore.Business.Services.Blacklist;
using PracticeGamestore.Business.Services.Country;

namespace PracticeGamestore.Business.Services.Location;

public class LocationService(ICountryService countryService, IBlacklistService blacklistService) : ILocationService
{
    public async Task HandleLocationAccessAsync(HttpContext context)
    {
        var countryName = context.Request.Headers["X-Location-Country"].FirstOrDefault();
        var userEmail = context.Request.Headers["X-User-Email"].FirstOrDefault();
        
        if (string.IsNullOrWhiteSpace(countryName) || string.IsNullOrWhiteSpace(userEmail))
        {
            throw new ArgumentException(ErrorMessages.MissingLocationHeaders);
        }
        
        var country = await countryService.GetByNameAsync(countryName);

        if (country is null)
        {
            await countryService.CreateAsync(new CountryDto(null, countryName, CountryStatus.Allowed));
            return;
        }

        if (await blacklistService.ExistsByUserEmailAsync(userEmail))
        {
            throw new UnauthorizedAccessException(ErrorMessages.BlacklistedUser);
        }

        if (country.Status == CountryStatus.Banned)
        {
            await blacklistService.CreateAsync(new BlacklistDto(null, userEmail, country.Id!.Value));
            throw new UnauthorizedAccessException(ErrorMessages.BlacklistedUser);
        }
    }
}