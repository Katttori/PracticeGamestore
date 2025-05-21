using PracticeGamestore.DataAccess.Enums;

namespace PracticeGamestore.API.Models;

public class CountryResponceModel
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public CountryStatus countryStatus { get; set; } = 0;
}