using PracticeGamestore.DataAccess.Enums;

namespace PracticeGamestore.Business.DataTransferObjects;

public class CountryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public CountryStatus CountryStatus { get; set; }
    
    public CountryDto(Guid id, string name, CountryStatus countryStatus)
    {
        Id = id;
        Name = name;
        CountryStatus = countryStatus;
    }
}