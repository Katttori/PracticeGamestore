using PracticeGamestore.Business.Models;
using PracticeGamestore.DTOs.Genre;

namespace PracticeGamestore.Mappers;

public static class GenreMapper
{   
    public static GenreModel ToModel(this CreateGenreDto dto)
    {
        return new GenreModel(null, dto.Name, dto.ParentId, dto.Description);
    }
    
    public static GenreDto ToDto(this GenreModel model)
    {
        return new GenreDto()
        {
            Id = model.Id,
            Name = model.Name,
            ParentId = model.ParentId,
            Description = model.Description
        };
    }
    
    public static void ApplyUpdate(this GenreModel model, UpdateGenreDto dto)
    {
        model.Rename(dto.Name);
        model.UpdateDescription(dto.Description);
    }
}