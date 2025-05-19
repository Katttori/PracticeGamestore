namespace PracticeGamestore.DTOs.Genre;

public class GenreDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public Guid? ParentId { get; set; }
    public string Description { get; set; } = string.Empty;
}