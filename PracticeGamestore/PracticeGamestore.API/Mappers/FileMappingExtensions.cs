using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Models.File;

namespace PracticeGamestore.Mappers;

public static class FileMappingExtensions
{
    public static FileDto MapToFileDto(this FileRequestModel model)
    {
        return new FileDto
        {
            GameId = model.GameId,
            File = model.File,
        };
    }
    
    public static FileResponseModel MapToFileModel(this FileDto dto)
    {
        return new FileResponseModel
        {
            Id = dto.Id!.Value,
            GameId = dto.GameId,
            Size = dto.Size,
            Path = dto.Path,
            CreationDate = dto.CreationDate,
            Type = dto.Type
        };
    }
}