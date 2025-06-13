using Microsoft.EntityFrameworkCore;
using PracticeGamestore.DataAccess.Entities;
using PracticeGamestore.DataAccess.Entities.Filtering;

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

    public async Task<(IEnumerable<Entities.Game>, int)> GetFiltered(GameFilter filter)
    {
        var query = _gamesNoTracking.AsQueryable();
        query = ApplyFiltering(query, filter);
        query = ApplyOrdering(query, filter.OrderByFields, filter.OrderType);
        var totalCount = await query.CountAsync();
        var skip = (filter.Page - 1) * filter.PageSize;
        var games =  await query.Skip(skip).Take(filter.PageSize).ToListAsync();
        return (games, totalCount);
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
    
    public async Task<List<Guid>> GetExistingIdsAsync(IEnumerable<Guid> ids)
    {
        return await context.Games
            .AsNoTracking()
            .Where(g => ids.Contains(g.Id))
            .Select(g => g.Id)
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
    
    private static IQueryable<Entities.Game> ApplyFiltering(IQueryable<Entities.Game> query, GameFilter filter)
    {
        if (filter.Name is not null)
            query = query.Where(g => g.Name.ToLower().Contains(filter.Name));
        if (filter.MinPrice is not null)
            query = query.Where(g => g.Price >= filter.MinPrice.Value);
        if (filter.MaxPrice.HasValue)
            query = query.Where(g => g.Price <= filter.MaxPrice.Value);
        if (filter.ReleaseDateStart.HasValue)
            query = query.Where(g => g.ReleaseDate >= filter.ReleaseDateStart.Value);
        if (filter.ReleaseDateEnd.HasValue)
            query = query.Where(g => g.ReleaseDate <= filter.ReleaseDateEnd.Value);
        if (filter.Age != null && filter.Age.Count != 0)
            query = query.Where(g => filter.Age.Contains(g.AgeRating));
        if (filter.RatingFrom.HasValue)
            query = query.Where(g => g.Rating >= filter.RatingFrom.Value);
        if (filter.RatingTo.HasValue)
            query = query.Where(g => g.Rating <= filter.RatingTo.Value);
        return query;
    }

    private static IQueryable<Entities.Game> ApplyOrdering(IQueryable<Entities.Game> query, List<string> orderByFields, string order)
    {
        var descending = order == "desc";
        var orderedQuery = ApplyPrimaryOrdering(query, orderByFields[0], descending);
        for (var i = 1; i < orderByFields.Count; i++)
        {
            orderedQuery = ApplySecondaryOrdering(orderedQuery, orderByFields[i], descending);
        }
        return orderedQuery;
    }
    
    private static IOrderedQueryable<Entities.Game> ApplyPrimaryOrdering(IQueryable<Entities.Game> query, string propertyName, bool descending)
    {
        return propertyName switch
        {
            "price" => descending ? query.OrderByDescending(g => g.Price) : query.OrderBy(g => g.Price),
            "rating" => descending ? query.OrderByDescending(g => g.Rating) : query.OrderBy(g => g.Rating),
            "age" => descending ? query.OrderByDescending(g => g.AgeRating) : query.OrderBy(g => g.AgeRating),
            "release-date" => descending ? query.OrderByDescending(g => g.ReleaseDate) : query.OrderBy(g => g.ReleaseDate),
            _ => descending ? query.OrderByDescending(g => g.Name) : query.OrderBy(g => g.Name)
        };
    } 
    
    private static IOrderedQueryable<Entities.Game> ApplySecondaryOrdering(IOrderedQueryable<Entities.Game> query, string propertyName, bool descending)
    {
        return propertyName switch
        {
            "price" => descending ? query.ThenByDescending(g => g.Price) : query.ThenBy(g => g.Price),
            "rating" => descending ? query.ThenByDescending(g => g.Rating) : query.ThenBy(g => g.Rating),
            "age" => descending ? query.ThenByDescending(g => g.AgeRating) : query.ThenBy(g => g.AgeRating),
            "release-date" => descending ? query.ThenByDescending(g => g.ReleaseDate) : query.ThenBy(g => g.ReleaseDate),
            _ => descending ? query.ThenByDescending(g => g.Name) : query.ThenBy(g => g.Name)
        };
    }
    
    public async Task<IEnumerable<Entities.Game>> GetByPublisherIdAsync(Guid id)
    {
        return await _gamesNoTracking.Where(g => g.PublisherId == id).ToListAsync();
    }

    public async Task<IEnumerable<Entities.Game>> GetByGenreAndItsChildrenAsync(List<Guid> ids)
    {
        return await _gamesNoTracking.Where(g => g.GameGenres.Any(gg => ids.Contains(gg.GenreId))).ToListAsync();
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

    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await _gamesNoTracking.AnyAsync(g => g.Name == name);
    }
    
    public async Task<bool> ExistsByKeyAsync(string key)
    {
        return await _gamesNoTracking.AnyAsync(g => g.Key == key);
    }
}