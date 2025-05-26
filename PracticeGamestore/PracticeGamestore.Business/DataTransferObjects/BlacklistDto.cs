using PracticeGamestore.DataAccess.Entities;

namespace PracticeGamestore.Business.DataTransferObjects;

public class BlacklistDto
{
    public Guid Id { get; set; }
    public string UserEmail { get; set; }
    public Guid? CountryId { get; set; }

    public BlacklistDto(Guid? id, string userEmail, Guid? countryId = null)
    {
        UserEmail = userEmail;
        CountryId = countryId;
    }
}