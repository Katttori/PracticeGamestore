namespace PracticeGamestore.Business.Services.Location;

public interface ILocationService
{
    Task HandleLocationAccessAsync(string countryName, string userEmail);
}