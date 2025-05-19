using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.DataAccess.Repositories.Genre;
using PracticeGamestore.DataAccess.UnitOfWork;

namespace PracticeGamestore.Business.Services.Genre;

public class GenreService(IGenreRepository genreRepository, IUnitOfWork unitOfWork) : IGenreService
{
    public async Task<IEnumerable<GenreDto>> GetAllAsync()
    {
        var entities = await genreRepository.GetAllAsync();
        return entities.Select(e => e.ToDto());
    }

    public async Task<GenreDto?> GetByIdAsync(Guid id)
    {
        var entity = await genreRepository.GetByIdAsync(id);
        return entity?.ToDto();
    }

    public async Task<Guid?> CreateAsync(GenreDto model)
    {
        var createdId = await genreRepository.CreateAsync(model.ToEntity());
        var changes = await unitOfWork.SaveChangesAsync();
        return changes > 0 ? createdId : null;
    }

    public async Task<bool> UpdateAsync(GenreDto model)
    {
        var entity = await genreRepository.GetByIdAsync(model.Id);
        if (entity is null) return false;
        
        entity.Name = model.Name;
        entity.ParentId = model.ParentId;
        entity.Description = model.Description;
        
        genreRepository.Update(entity);
        var changes = await unitOfWork.SaveChangesAsync();
        return changes > 0;
    }

    public async Task DeleteAsync(Guid id)
    {
        await genreRepository.DeleteAsync(id);
        await unitOfWork.SaveChangesAsync();
    }
}