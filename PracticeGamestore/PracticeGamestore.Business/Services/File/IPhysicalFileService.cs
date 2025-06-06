using Microsoft.AspNetCore.Http;
using PracticeGamestore.Business.DataTransferObjects;

namespace PracticeGamestore.Business.Services.File;

public interface IPhysicalFileService
{
    Task<string> SaveFileAsync(IFormFile file);
    Task<byte[]> ReadFileAsync(string filePath);
    void DeleteFile(string filePath);
}