namespace PracticeGamestore.DataAccess.Entities;

public class Blacklist
{
    public Guid Id { get; set; }
    public string UserEmail { get; set; }
    public Guid CountryId { get; set; }
}