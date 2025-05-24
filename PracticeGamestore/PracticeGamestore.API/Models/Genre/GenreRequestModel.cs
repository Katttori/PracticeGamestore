namespace PracticeGamestore.Models.Genre;

public class GenreRequestModel
{
    public required string Name { get; set; }
    public Guid? ParentId { get; set; }
    public string Description { get; set; } = string.Empty;
}