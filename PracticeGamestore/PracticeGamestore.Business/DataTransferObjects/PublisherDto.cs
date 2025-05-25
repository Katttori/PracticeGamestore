namespace PracticeGamestore.Business.DataTransferObjects;

public class PublisherDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string PageUrl { get; set; }

    public PublisherDto(Guid? id, string name, string description, string pageUrl)
    {
        Id = id ?? Guid.NewGuid();
        Name = name;
        Description = description;
        PageUrl = pageUrl;
    }
}