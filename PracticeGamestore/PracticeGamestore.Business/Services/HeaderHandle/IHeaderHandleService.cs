namespace PracticeGamestore.Business.Services.HeaderHandle;

public interface IHeaderHandleService
{
    Task CheckAccessAsync(string countryName, string userEmail);
}