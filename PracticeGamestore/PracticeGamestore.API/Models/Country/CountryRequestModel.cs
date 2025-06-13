using PracticeGamestore.Business.Enums;

namespace PracticeGamestore.Models.Country;

public class CountryUpdateRequestModel
{
    public required string Name { get; set; }
    public CountryStatus Status { get; set; }
}

public class CountryCreateRequestModel
{
    public required string Name { get; set; }
}