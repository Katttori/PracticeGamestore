namespace PracticeGamestore.DataAccess.Entities;


public enum Status
{
    Allowed, Banned
}

public class CountryEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Status Status { get; set; }
}

