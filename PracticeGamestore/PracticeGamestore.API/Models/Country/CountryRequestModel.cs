using PracticeGamestore.DataAccess.Enums;

namespace PracticeGamestore.API.Models;

public class CountryRequestModel
{
    public required string Name { get; set; }
    public CountryStatus Status { get; set; } = 0;
}