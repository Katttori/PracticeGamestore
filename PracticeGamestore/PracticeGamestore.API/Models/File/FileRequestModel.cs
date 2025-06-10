namespace PracticeGamestore.Models.File;

public class FileRequestModel
{
    public Guid GameId { get; set; }
    public IFormFile File { get; set; } = null!; // This property is used for file upload,
}