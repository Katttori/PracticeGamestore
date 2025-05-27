namespace PracticeGamestore.Business.DataTransferObjects;

public class BlacklistDto
{
    public Guid? Id { get; set; }
    public string UserEmail { get; set; }
    public Guid? CountryId { get; set; }

    public BlacklistDto(Guid? id, string userEmail, Guid? countryId = null)
    {
        Id = id;
        UserEmail = userEmail;
        CountryId = countryId;
    }
}