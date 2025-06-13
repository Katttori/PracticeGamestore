using PracticeGamestore.Business.Services.Blacklist;
using PracticeGamestore.Business.Services.Location;

namespace PracticeGamestore.Business.Services.HeaderHandle;

public class HeaderHandleService(ILocationService locationService, IBlacklistService blacklistService) : IHeaderHandleService
{
    public async Task CheckAccessAsync(string countryName, string userEmail)
    {
        await blacklistService.HandleUserEmailAccessAsync(userEmail);
        await locationService.HandleLocationAccessAsync(countryName, userEmail);
    }
}