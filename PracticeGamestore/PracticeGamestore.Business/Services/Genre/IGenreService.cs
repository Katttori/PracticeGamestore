using PracticeGamestore.Business.Models;

namespace PracticeGamestore.Business.Services.Genre;

public interface IGenreService
{
    public Task<IEnumerable<GenreModel>> GetAllAsync();
    public Task<GenreModel?> GetByIdAsync(Guid id);
    public Task<Guid?> CreateAsync(GenreModel model);
    public Task<bool> UpdateAsync(GenreModel model);
    public Task DeleteAsync(Guid id);
}