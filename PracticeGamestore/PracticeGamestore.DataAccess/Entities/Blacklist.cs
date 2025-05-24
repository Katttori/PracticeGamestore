namespace PracticeGamestore.DataAccess.Entities;

public class Blacklist
{
    public Guid Id { get; set; }
    public required string UserEmail { get; set; }
    public Guid? CountryId { get; set; }
    public Country Country { get; set; } = null!;
}