using Microsoft.AspNetCore.Http;

namespace PracticeGamestore.Business.Services.Location;

public interface ILocationService
{
    Task HandleLocationAccessAsync(HttpContext context);
}