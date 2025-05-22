using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.DataAccess.Entities;

namespace PracticeGamestore.Business.Mappers;

public static class GenreMappingExtensions
{
    public static Genre MapToGenreEntity(this GenreDto dto)
    {
        return new()
        {
            Id = dto.Id,
            Name = dto.Name,
            ParentId = dto.ParentId,
            Description = dto.Description,
        };
    }

    public static GenreDto MapToGenreDto(this Genre entity)
    {
        return new(entity.Id, entity.Name, entity.ParentId, entity.Description);
    }
}