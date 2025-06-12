using PracticeGamestore.Business.DataTransferObjects;

namespace PracticeGamestore.Business.Services.Genre;

public interface IGenreService
{
    Task<IEnumerable<GenreDto>> GetAllAsync();
    Task<GenreDto?> GetByIdAsync(Guid id);
    Task<Guid?> CreateAsync(GenreDto dto);
    Task<bool> UpdateAsync(Guid id, GenreDto dto);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<GameResponseDto>?> GetGamesAsync(Guid id, bool hideAdultContent = false);
}