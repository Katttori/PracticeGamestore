using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.DataAccess.Entities;
using File = PracticeGamestore.DataAccess.Entities.File;

namespace PracticeGamestore.Business.Mappers;

public static class FileMappingExtensions
{
    public static FileDto MapToFileDto(this File file)
    {
        return new FileDto
        {
            Id = file.Id,
            GameId = file.GameId,
            Size = file.Size,
            Path = file.Path,
            CreationDate = file.CreationDate,
            Type = file.Type
        };
    }
    
    
    public static File MapToFileEntity(this FileDto fileDto)
    {
        if (fileDto.Id.HasValue)
        {
            return new File
            {
                Id = fileDto.Id.Value,
                GameId = fileDto.GameId,
                Size = fileDto.File.Length,
                Path = fileDto.Path,
                CreationDate = fileDto.CreationDate,
                Type = fileDto.File.ContentType
            };
        }
        
        return new File
        {
            GameId = fileDto.GameId,
            Size = fileDto.File.Length,
            Path = fileDto.Path,
            CreationDate = fileDto.CreationDate,
            Type = fileDto.File.ContentType
        };
    }
}