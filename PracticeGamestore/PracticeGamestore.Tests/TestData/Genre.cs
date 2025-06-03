namespace PracticeGamestore.Tests.TestData;

public class Genre
{
    public static List<DataAccess.Entities.Genre> GenerateGenreEntities()
    {
        return new ()
        {
            new() { Id = Guid.NewGuid(), Name = "Action" },
            new() { Id = Guid.NewGuid(), Name = "FPS" },
            new() { Id = Guid.NewGuid(), Name = "RPG" },
            new() { Id = Guid.NewGuid(), Name = "Strategy" },
            new() { Id = Guid.NewGuid(), Name = "Adventure" },
            new() { Id = Guid.NewGuid(), Name = "Racing" },
            new() { Id = Guid.NewGuid(), Name = "Sports" },
            new() { Id = Guid.NewGuid(), Name = "Simulation" },
            new() { Id = Guid.NewGuid(), Name = "Horror" },
            new() { Id = Guid.NewGuid(), Name = "Puzzle" }
        };
    }
}