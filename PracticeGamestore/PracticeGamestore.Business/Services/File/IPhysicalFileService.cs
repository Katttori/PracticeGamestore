using Microsoft.AspNetCore.Http;
namespace PracticeGamestore.Business.Services.File;

public interface IPhysicalFileService
{
    Task<string> SaveFileAsync(IFormFile file);
    void DeleteFile(string filePath);
}