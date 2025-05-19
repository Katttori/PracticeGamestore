using PracticeGamestore.Business.Models;
using PracticeGamestore.DataAccess.Entities;

namespace PracticeGamestore.Business.Mappers;

public static class GenreMapper
{
    public static Genre ToEntity(this GenreModel model)
    {
        return new Genre()
        {
            Id = model.Id,
            Name = model.Name,
            ParentId = model.ParentId,
            Description = model.Description,
        };
    }

    public static GenreModel ToModel(this Genre entity)
    {
        return new GenreModel(entity.Id, entity.Name, entity.ParentId, entity.Description);
    }
}