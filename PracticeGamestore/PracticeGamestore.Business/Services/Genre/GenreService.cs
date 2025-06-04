using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.DataAccess.Repositories.Game;
using PracticeGamestore.DataAccess.Repositories.Genre;
using PracticeGamestore.DataAccess.UnitOfWork;

namespace PracticeGamestore.Business.Services.Genre;

public class GenreService(IGenreRepository genreRepository, IGameRepository gameRepository, IUnitOfWork unitOfWork) : IGenreService
{
    public async Task<IEnumerable<GenreDto>> GetAllAsync()
    {
        var entities = await genreRepository.GetAllAsync();
        return entities.Select(e => e.MapToGenreDto());
    }

    public async Task<GenreDto?> GetByIdAsync(Guid id)
    {
        var entity = await genreRepository.GetByIdAsync(id);
        return entity?.MapToGenreDto();
    }

    public async Task<Guid?> CreateAsync(GenreDto dto)
    {
        var createdId = await genreRepository.CreateAsync(dto.MapToGenreEntity());
        var changes = await unitOfWork.SaveChangesAsync();
        return changes > 0 ? createdId : null;
    }

    public async Task<bool> UpdateAsync(Guid id, GenreDto dto)
    {
        var entity = await genreRepository.GetByIdAsync(id);
        if (entity is null) return false;
        
        entity.Name = dto.Name;
        entity.ParentId = dto.ParentId;
        entity.Description = dto.Description;
        
        genreRepository.Update(entity);
        var changes = await unitOfWork.SaveChangesAsync();
        return changes > 0;
    }

    public async Task DeleteAsync(Guid id)
    {
        await genreRepository.DeleteAsync(id);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<GameResponseDto>?> GetGamesByGenreAsync(Guid id)
    {
        var genreExists = await genreRepository.ExistsAsync(id);
        if (!genreExists) return null;
        var genreChildrenIds = await genreRepository.GetGenreChildrenIdsAsync(id);
        var games = await gameRepository.GetByGenreAndItsChildrenAsync(genreChildrenIds);
        return games.Select(g => g.MapToGameDto());
    }
}