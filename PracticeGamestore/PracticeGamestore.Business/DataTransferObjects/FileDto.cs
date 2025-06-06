using Microsoft.AspNetCore.Http;

namespace PracticeGamestore.Business.DataTransferObjects;

public class FileDto
{
    public Guid? Id { get; set; }
    public Guid GameId { get; set; }
    public long Size { get; set; }
    public string Path { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; }
    public string Type { get; set; } = string.Empty;
    
    public IFormFile File { get; set; } = null!; // This property is used for file upload,
                                                 // it should not be serialized to the database.
}