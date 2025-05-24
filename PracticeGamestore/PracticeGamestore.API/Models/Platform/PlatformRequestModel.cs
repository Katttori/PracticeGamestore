namespace PracticeGamestore.Models.Platform;

public class PlatformRequestModel
{
    public required string Name { get; set; }
    public string Description { get; set; } = string.Empty;
}