using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace PracticeGamestore.Business.Services.File;

public class PhysicalFileService(
    IWebHostEnvironment env,
    IConfiguration config,
    ILogger<PhysicalFileService> logger) : IPhysicalFileService
{
    
    private readonly string root = Path.Combine(env.ContentRootPath, config["Storage:GameFilesPath"]!);
    
    public async Task<string> SaveFileAsync(IFormFile file)
    {
        if (!Directory.Exists(root))
            Directory.CreateDirectory(root);
        
        var path = Path.Combine(root, Guid.NewGuid() + Path.GetExtension(file.FileName));

        try
        {
            await using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);
            return path;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while saving the file: {FileName}", file.FileName);
            throw new Exception("An error occurred while saving the file.", ex);
        }
    }
    
    public void DeleteFile(string filePath)
    {
        try
        {
            if(System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while deleting the file: {FilePath}", filePath);
            throw new Exception("An error occurred while deleting the file.", ex);
        }
    }
}