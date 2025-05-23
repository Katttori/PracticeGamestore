namespace PracticeGamestore.Models.Genre;

public class GenreResponseModel
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public Guid? ParentId { get; set; }
    public string Description { get; set; } = string.Empty;
}