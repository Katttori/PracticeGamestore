namespace PracticeGamestore.Models.Publisher;

public class PublisherResponseModel
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public required string PageUrl { get; set; }
}