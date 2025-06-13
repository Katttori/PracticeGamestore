using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.DataAccess.Repositories.File;
using PracticeGamestore.DataAccess.Repositories.Game;
using PracticeGamestore.DataAccess.UnitOfWork;

namespace PracticeGamestore.Business.Services.File;

public class FileService(
    IFileRepository fileRepository,
    IGameRepository gameRepository,
    IUnitOfWork unitOfWork,
    IWebHostEnvironment env,
    IConfiguration config,
    ILogger<FileService> logger
    ) : IFileService
{
    private readonly string root = Path.Combine(env.ContentRootPath, config["Storage:GameFilesPath"]!);

    public async Task<IEnumerable<FileDto>> GetAllAsync()
    {
        var files = await fileRepository.GetAllAsync();
        return files.Select(f => f.MapToFileDto());
    }
    
    public async Task<FileDto?> GetByIdAsync(Guid id)
    {
        var file = await fileRepository.GetByIdAsync(id);
        return file?.MapToFileDto();
    }
    
    public async Task<Guid?> UploadAsync(FileDto fileDto)
    {   
        
        var game = await gameRepository.GetByIdAsync(fileDto.GameId);
        if (game == null)
        {
            logger.LogError(ErrorMessages.NotFound("Game", fileDto.GameId));
            return null;
        }
        
        await unitOfWork.BeginTransactionAsync();
        var path = await SavePhysicalFileAsync(fileDto.File);
            
        var fileEntity = fileDto.MapToFileEntity();
            
        fileEntity.Id = fileDto.Id ?? Guid.NewGuid();
        fileEntity.Path = path;
            
        await fileRepository.CreateAsync(fileEntity);
        var changes = await unitOfWork.SaveChangesAsync();
        
        if (changes <= 0)
        {
            await unitOfWork.RollbackTransactionAsync();
        }
        
        await unitOfWork.CommitTransactionAsync();
        return changes > 0 ? fileEntity.Id : null;
    }
    
    public async Task DeleteAsync(Guid id)
    {
        var file = await fileRepository.GetByIdAsync(id);
        if (file == null) return;
        
        DeletePhysicalFile(file.Path);
        await fileRepository.DeleteAsync(id);
        await unitOfWork.SaveChangesAsync();
    }
    
    public async Task<string> SavePhysicalFileAsync(IFormFile file)
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
    
    public async Task<byte[]> ReadPhysicalFileAsync(string filePath)
    {
        try
        {
            if (!System.IO.File.Exists(filePath))
                throw new FileNotFoundException("File not found.", filePath);
            
            return await System.IO.File.ReadAllBytesAsync(filePath);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while reading the file: {FilePath}", filePath);
            throw new Exception("An error occurred while reading the file.", ex);
        }
    }
    
    public void DeletePhysicalFile(string filePath)
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