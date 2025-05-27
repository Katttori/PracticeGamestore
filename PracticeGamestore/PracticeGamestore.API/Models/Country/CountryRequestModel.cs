using PracticeGamestore.DataAccess.Enums;

namespace PracticeGamestore.API.Models;

public class CountryUpdateRequestModel
{
    public required string Name { get; set; }
    public CountryStatus Status { get; set; }
}

public class CountryCreateRequestModel
{
    public required string Name { get; set; }
}