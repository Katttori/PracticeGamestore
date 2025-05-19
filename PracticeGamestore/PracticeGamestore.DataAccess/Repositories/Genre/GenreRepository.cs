namespace PracticeGamestore.DataAccess.Repositories.Genre;

public class GenreRepository(GamestoreDbContext context) : IGenreRepository
{
    public async Task<IEnumerable<Entities.Genre>> GetAllAsync()
    {
        return await context.Genres.AsNoTracking().ToListAsync();
    }

    public async Task<Entities.Genre?> GetByIdAsync(Guid id)
    {
        return await context.Genres.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<Guid> CreateAsync(Entities.Genre genre)
    {
        var entry = await context.Genres.AddAsync(genre);
        return entry.Entity.Id;
    }

    public Task<bool> UpdateAsync(Entities.Genre genre)
    {
        context.Genres.Update(genre);
        return Task.FromResult(true);
    }

    public async Task DeleteAsync(Guid id)
    {
        var genre = await context.Genres.FindAsync(id);

        if (genre is not null)
        {
            context.Genres.Remove(genre);
        }
    }
}