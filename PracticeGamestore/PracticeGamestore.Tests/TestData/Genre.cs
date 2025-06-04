namespace PracticeGamestore.Tests.TestData;

public class Genre
{
    public static List<DataAccess.Entities.Genre> GenerateGenreEntities()
    {
        var strategyId = Guid.NewGuid();
        var sportsId = Guid.NewGuid();
        var rpgId = Guid.NewGuid();
        var racingId = Guid.NewGuid();
        var actionGenre = GenerateActionGenre();

        return new()
        {
            actionGenre,
            new() { Id = strategyId, Name = "Strategy", Description = "Strategic thinking and planning games" },
            new() { Id = sportsId, Name = "Sports", Description = "Sports simulation and arcade games" },
            new() { Id = rpgId, Name = "RPG", Description = "Role-playing games" },
            new() { Id = racingId, Name = "Racing", Description = "Racing and driving games" },
            new() { Id = Guid.NewGuid(), Name = "Adventure", Description = "Story-driven adventure games" },
            new() { Id = Guid.NewGuid(), Name = "Simulation", Description = "Life and business simulation games" },
            new() { Id = Guid.NewGuid(), Name = "Horror", Description = "Scary and thriller games" },
            new() { Id = Guid.NewGuid(), Name = "Puzzle", Description = "Brain teasers and logic games" },
            new() { Id = Guid.NewGuid(), Name = "FPS", Description = "First-person shooter", ParentId = actionGenre.Id },
            new() { Id = Guid.NewGuid(), Name = "TPS", Description = "Third-person shooter", ParentId = actionGenre.Id },
            new() { Id = Guid.NewGuid(), Name = "Beat 'em up", Description = "Fighting action games", ParentId = actionGenre.Id },
            new() { Id = Guid.NewGuid(), Name = "Platformer", Description = "Platform jumping games", ParentId = actionGenre.Id },
            new() { Id = Guid.NewGuid(), Name = "RTS", Description = "Real-time strategy", ParentId = strategyId },
            new() { Id = Guid.NewGuid(), Name = "TBS", Description = "Turn-based strategy", ParentId = strategyId },
            new() { Id = Guid.NewGuid(), Name = "4X", Description = "Explore, expand, exploit, exterminate", ParentId = strategyId },
            new() { Id = Guid.NewGuid(), Name = "Tower Defense", Description = "Defensive strategy games", ParentId = strategyId },
            new() { Id = Guid.NewGuid(), Name = "Football", Description = "American football games", ParentId = sportsId },
            new() { Id = Guid.NewGuid(), Name = "Soccer", Description = "Association football games", ParentId = sportsId },
            new() { Id = Guid.NewGuid(), Name = "Basketball", Description = "Basketball simulation games", ParentId = sportsId },
            new() { Id = Guid.NewGuid(), Name = "Baseball", Description = "Baseball simulation games", ParentId = sportsId },
            new() { Id = Guid.NewGuid(), Name = "JRPG", Description = "Japanese role-playing games", ParentId = rpgId },
            new() { Id = Guid.NewGuid(), Name = "Action RPG", Description = "Action-oriented RPGs", ParentId = rpgId },
            new() { Id = Guid.NewGuid(), Name = "Tactical RPG", Description = "Strategy-based RPGs", ParentId = rpgId },
            new() { Id = Guid.NewGuid(), Name = "MMORPG", Description = "Massively multiplayer online RPGs", ParentId = rpgId },
            new() { Id = Guid.NewGuid(), Name = "Formula", Description = "Formula racing games", ParentId = racingId },
            new() { Id = Guid.NewGuid(), Name = "Rally", Description = "Rally racing games", ParentId = racingId },
            new() { Id = Guid.NewGuid(), Name = "Arcade Racing", Description = "Arcade-style racing", ParentId = racingId },
            new() { Id = Guid.NewGuid(), Name = "Off-road", Description = "Off-road racing games", ParentId = racingId }
        };
    }
    

    public static DataAccess.Entities.Genre GenerateActionGenre()
    {
        return new() { Id = Guid.NewGuid(), Name = "Action", Description = "Fast-paced action games" };
    }
    
    public static List<Guid> GenerateGenreChildren(Guid parentId)
    {
        var genres = GenerateGenreEntities();
        var result  = new List<Guid> { parentId };
        GetGenreChildrenRecursively(genres, parentId, result);
        return result;
    }

    private static void GetGenreChildrenRecursively(List<DataAccess.Entities.Genre> genres, Guid parentId, List<Guid> result)
    {
        var children = genres.Where(g => g.ParentId == parentId);
        foreach (var child in children)
        {
            result.Add(child.Id);
            GetGenreChildrenRecursively(genres, child.Id, result);
        }
    }
}
