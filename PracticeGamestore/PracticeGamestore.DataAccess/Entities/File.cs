namespace PracticeGamestore.DataAccess.Entities;

public class File
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public long Size { get; set; }
    public required string Path { get; set; }
    public DateTime CreationDate { get; set; }
    public string Type { get; set; } = string.Empty;
    public Game Game { get; set; } = null!;
}