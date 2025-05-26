namespace PracticeGamestore.Business.DataTransferObjects;

public class PlatformDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public PlatformDto(Guid? id, string name, string description)
    {
        Name = name;
        Description = description;
    }
}