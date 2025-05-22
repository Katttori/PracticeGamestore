using PracticeGamestore.Business.DataTransferObjects;

namespace PracticeGamestore.Business.Services.Genre;

public interface IGenreService
{
    Task<IEnumerable<GenreDto>> GetAllAsync();
    Task<GenreDto?> GetByIdAsync(Guid id);
    Task<Guid?> CreateAsync(GenreDto model);
    Task<bool> UpdateAsync(GenreDto model);
    Task DeleteAsync(Guid id);
}