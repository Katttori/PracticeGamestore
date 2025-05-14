namespace PracticeGamestore.DataAccess.Entities;

public class Genre
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid? ParentId { get; set; }
    public string Description { get; set; }
    public int MinimumAgeRating { get; set; }
}