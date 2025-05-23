namespace PracticeGamestore.Business.DataTransferObjects;

public class GenreDto
{
    public Guid Id { get; }
    public string Name { get; set; }
    public Guid? ParentId { get; set; }
    public string Description { get; set; }
    
    public GenreDto(Guid? id, string name, Guid? parentId = null, string description = "")
    {
        Id = id ?? Guid.NewGuid();
        Name = name;
        ParentId = parentId;
        Description = description;
    }
}