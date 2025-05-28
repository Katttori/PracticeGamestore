using Microsoft.EntityFrameworkCore;
using PracticeGamestore.DataAccess.Entities;

namespace PracticeGamestore.DataAccess.Repositories.Game;

public class GameRepository(GamestoreDbContext context) : IGameRepository
{
    private readonly IQueryable<Entities.Game> _gamesNoTracking = context.Games.AsNoTracking()
        .Include(g => g.Publisher)
        .Include(g => g.GamePlatforms)
        .ThenInclude(gp => gp.Platform)
        .Include(g => g.GameGenres)
        .ThenInclude(gg => gg.Genre);

    public async Task<IEnumerable<Entities.Game>> GetAllAsync()
    {
        return await _gamesNoTracking.ToListAsync();
    }

    public async Task<Entities.Game?> GetByIdAsync(Guid id)
    {
        return await _gamesNoTracking.FirstOrDefaultAsync(g => g.Id == id);
    }
    
    public async Task<IEnumerable<Entities.Game>> GetByPlatformIdAsync(Guid platformId)
    {
        return await _gamesNoTracking
            .Where(g => g.GamePlatforms.Any(gp => gp.PlatformId == platformId))
            .ToListAsync();
    }


    public async Task DeleteAsync(Guid id)
    {
        var game = await context.Games.FindAsync(id);
        if (game != null) context.Games.Remove(game);
    }

    public async Task<Guid> CreateAsync(Entities.Game game, List<Guid> genreIds, List<Guid> platformIds)
    {
        var newGame = await context.Games.AddAsync(game);
        var gameId = newGame.Entity.Id;
        await AddGenresAsync(gameId, genreIds);
        await AddPlatformsAsync(gameId, platformIds);
        return gameId;
    }

    public async Task UpdateAsync(Entities.Game game, List<Guid> genreIds, List<Guid> platformIds)
    {
        context.Games.Update(game);
        await UpdateGenreIdsAsync(game.Id, genreIds);
        await UpdatePlatformIdsAsync(game.Id, platformIds);
    }

    public async Task<IEnumerable<Entities.Game>> GetByPublisherIdAsync(Guid id)
    {
        return await _gamesNoTracking.Where(g => g.PublisherId == id).ToListAsync();
    }

    private async Task AddPlatformsAsync(Guid gameId, IEnumerable<Guid> platformIds)
    {
        var gamePlatforms = platformIds.Select(id => new GamePlatform
        {
            GameId = gameId,
            PlatformId = id
        }).ToList();
        
        await context.GamePlatforms.AddRangeAsync(gamePlatforms);
    }

    private async Task AddGenresAsync(Guid gameId, IEnumerable<Guid> genreIds)
    {
        var gameGenres = genreIds.Select(id => new GameGenre
        {
            GameId = gameId,
            GenreId = id
        }).ToList();
        
        await context.GameGenres.AddRangeAsync(gameGenres);
    }
    
    private async Task UpdateGenreIdsAsync(Guid gameId, List<Guid> genreIds)
    {
        var existingGenres = await context.GameGenres
            .Where(gg => gg.GameId == gameId)
            .ToListAsync();
        var existingGenreIds = existingGenres.Select(gg => gg.GenreId).ToList();

        var genreIdsToRemove = existingGenreIds.Where(id => !genreIds.Contains(id)).ToList();
        var genreIdsToAdd = genreIds.Where(id => !existingGenreIds.Contains(id)).ToList();
    
        var genresToRemove = existingGenres.Where(gg => genreIdsToRemove.Contains(gg.GenreId)).ToList();
        context.GameGenres.RemoveRange(genresToRemove);
    
        await AddGenresAsync(gameId, genreIdsToAdd);
    }

    private async Task UpdatePlatformIdsAsync(Guid gameId, List<Guid> platformIds)
    {
        var existingPlatforms = await context.GamePlatforms
            .Where(gp => gp.GameId == gameId)
            .ToListAsync();
        var existingPlatformIds = existingPlatforms.Select(gp => gp.PlatformId).ToList();
    
        var platformIdsToRemove = existingPlatformIds.Where(id => !platformIds.Contains(id)).ToList();
        var platformIdsToAdd = platformIds.Where(id => !existingPlatformIds.Contains(id)).ToList();
    
        var platformsToRemove = existingPlatforms.Where(gp => platformIdsToRemove.Contains(gp.PlatformId)).ToList();
        context.GamePlatforms.RemoveRange(platformsToRemove);
    
        await AddPlatformsAsync(gameId, platformIdsToAdd);
    }
}