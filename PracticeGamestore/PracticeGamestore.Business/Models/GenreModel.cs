namespace PracticeGamestore.Business.Models;

public class GenreModel
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Guid? ParentId { get; private set; }
    public string Description { get; private set; }
    
    public GenreModel(Guid? id, string name, Guid? parentId = null, string description = "")
    {
        Id = id ?? Guid.NewGuid();
        Name = name;
        ParentId = parentId;
        Description = description;
    }
    
    public void Rename(string newName)
    {
        // TODO: add validation
        
        Name = newName;
    }

    public void UpdateDescription(string newDescription)
    {
        // TODO: add validation
        
        Description = newDescription;
    }
}