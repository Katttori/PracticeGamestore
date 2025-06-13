using PracticeGamestore.Business.Enums;

namespace PracticeGamestore.Models.Country;

public class CountryResponseModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public CountryStatus Status { get; set; } = 0;
}