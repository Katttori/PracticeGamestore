using PracticeGamestore.Business.Mappers;
using PracticeGamestore.Business.Models;
using PracticeGamestore.DataAccess.Repositories.Genre;

namespace PracticeGamestore.Business.Services.Genre;

public class GenreService(IGenreRepository genreRepository, IUnitOfWork unitOfWork) : IGenreService
{
    public async Task<IEnumerable<GenreModel>> GetAllAsync()
    {
        var entities = await genreRepository.GetAllAsync();
        return entities.Select(e => e.ToModel());
    }

    public async Task<GenreModel?> GetByIdAsync(Guid id)
    {
        var entity = await genreRepository.GetByIdAsync(id);
        return entity?.ToModel();
    }

    public async Task<Guid?> CreateAsync(GenreModel model)
    {
        var id = await genreRepository.CreateAsync(model.ToEntity());
        var changes = await unitOfWork.SaveChangesAsync();
        return changes > 0 ? id : null;
    }

    public async Task<bool> UpdateAsync(GenreModel model)
    {
        await genreRepository.UpdateAsync(model.ToEntity());
        var changes = await unitOfWork.SaveChangesAsync();
        return changes > 0;
    }

    public async Task DeleteAsync(Guid id)
    {
        await genreRepository.DeleteAsync(id);
        await unitOfWork.SaveChangesAsync();
    }
}