namespace PracticeGamestore.Business.DataTransferObjects;

public class PlatformDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public PlatformDto(Guid? guid, string name, string description)
    {
        Id = guid ?? Guid.NewGuid();
        Name = name;
        Description = description;
    }
}