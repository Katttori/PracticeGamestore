namespace PracticeGamestore.DataAccess.Entities;

public class Genre
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public Guid? ParentId { get; set; }
    public string Description { get; set; } = string.Empty;
    public Genre? Parent { get; set; }
    public List<Genre> Children { get; set; } = [];
    public List<GameGenre> GameGenres { get; set; } = [];
}