using PracticeGamestore.DataAccess.Enums;

namespace PracticeGamestore.API.Models;

public class CountryResponseModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public CountryStatus Status { get; set; } = 0;
}