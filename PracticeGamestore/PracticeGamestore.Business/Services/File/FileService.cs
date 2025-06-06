using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.DataAccess.Repositories.File;
using PracticeGamestore.DataAccess.UnitOfWork;

namespace PracticeGamestore.Business.Services.File;

public class FileService(IFileRepository fileRepository, IPhysicalFileService service, IUnitOfWork unitOfWork) : IFileService
{
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
    
    public async Task<Guid?> CreateAsync(FileDto file)
    {   
        await unitOfWork.BeginTransactionAsync();
        var path = await service.SaveFileAsync(file.File);
            
        var fileEntity = file.MapToFileEntity();
            
        fileEntity.Id = file.Id ?? Guid.NewGuid();
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
        
        await unitOfWork.BeginTransactionAsync();
        service.DeleteFile(file.Path);
        await fileRepository.DeleteAsync(id);
        await unitOfWork.SaveChangesAsync();
        
        await unitOfWork.CommitTransactionAsync();
    }
}