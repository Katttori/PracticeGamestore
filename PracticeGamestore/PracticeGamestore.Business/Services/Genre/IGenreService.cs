using PracticeGamestore.Business.DataTransferObjects;

namespace PracticeGamestore.Business.Services.Genre;

public interface IGenreService
{
    public Task<IEnumerable<GenreDto>> GetAllAsync();
    public Task<GenreDto?> GetByIdAsync(Guid id);
    public Task<Guid?> CreateAsync(GenreDto model);
    public Task<bool> UpdateAsync(GenreDto model);
    public Task DeleteAsync(Guid id);
}