namespace PracticeGamestore.Models.Blacklist;

public class BlacklistRequestModel
{
    public required string UserEmail { get; set; } 
    public Guid CountryId { get; set; }
}