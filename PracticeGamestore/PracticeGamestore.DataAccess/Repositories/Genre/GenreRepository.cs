using Microsoft.EntityFrameworkCore;

namespace PracticeGamestore.DataAccess.Repositories.Genre;

public class GenreRepository(GamestoreDbContext context) : IGenreRepository
{
    private readonly IQueryable<Entities.Genre> _genresNoTracking = context.Genres.AsNoTracking();
    
    public async Task<IEnumerable<Entities.Genre>> GetAllAsync()
    {
        return await _genresNoTracking.ToListAsync();
    }

    public async Task<Entities.Genre?> GetByIdAsync(Guid id)
    {
        return await _genresNoTracking.FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<Guid> CreateAsync(Entities.Genre genre)
    {
        var entry = await context.Genres.AddAsync(genre);
        return entry.Entity.Id;
    }

    public void Update(Entities.Genre genre)
    {
        context.Genres.Update(genre);
    }

    public async Task DeleteAsync(Guid id)
    {
        var genre = await context.Genres.FindAsync(id);

        if (genre is not null)
        {
            context.Genres.Remove(genre);
        }
    }

    public Task<bool> ExistsAsync(Guid id)
    {
        return _genresNoTracking.AnyAsync(g => g.Id == id);
    }
    
    public async Task<List<Guid>> GetGenreChildrenIdsAsync(Guid id)
    {
        return await context.Database
            .SqlQuery<Guid>($@"
                WITH children AS (
                    SELECT id FROM genres
                    WHERE id = {id}
                    UNION ALL
                    SELECT g.id FROM genres g
                    JOIN children c ON g.parent_id = c.id
                 )
                SELECT id FROM children")
            .ToListAsync();
    }
}