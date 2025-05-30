namespace PracticeGamestore.Models.Blacklist;

public class BlacklistResponseModel
{
    public Guid? Id { get; set; }
    public required string UserEmail { get; set; }
    public Guid CountryId { get; set; }
}