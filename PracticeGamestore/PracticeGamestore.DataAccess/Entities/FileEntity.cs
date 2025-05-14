using System;

namespace PracticeGamestore.DataAccess.Entities;

public class FileEntity
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public long Size { get; set; }
    public string Path { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; }
    public string Type { get; set; } = string.Empty;

    public GameEntity Game { get; set; } = null!;

}