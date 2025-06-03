using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Models.Genre;

namespace PracticeGamestore.Mappers;

public static class GenreMappingExtensions
{   
    public static GenreDto MapToGenreDto(this GenreRequestModel model)
    {
        return new(null, model.Name, model.ParentId, model.Description);
    }
    
    public static GenreResponseModel MapToGenreModel(this GenreDto dto)
    {
        return new()
        {
            Id = dto.Id!.Value,
            Name = dto.Name,
            ParentId = dto.ParentId,
            Description = dto.Description
        };
    }
}