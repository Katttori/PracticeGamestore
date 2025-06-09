using Microsoft.AspNetCore.Http;
using PracticeGamestore.Business.DataTransferObjects;

namespace PracticeGamestore.Business.Services.File;

public interface IFileService
{
    Task<IEnumerable<FileDto>> GetAllAsync();
    Task<FileDto?> GetByIdAsync(Guid id);
    Task<Guid?> UploadAsync(FileDto fileDto);
    Task DeleteAsync(Guid id);
    
    // Physical file service methods
    Task<string> SavePhysicalFileAsync(IFormFile file);
    Task<byte[]> ReadPhysicalFileAsync(string filePath);
    void DeletePhysicalFile(string filePath);
}