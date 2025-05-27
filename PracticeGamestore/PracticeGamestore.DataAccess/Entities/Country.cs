using PracticeGamestore.DataAccess.Enums;

namespace PracticeGamestore.DataAccess.Entities;

public class Country
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public CountryStatus CountryStatus { get; set; }
    public List<Blacklist> Blacklists { get; set; } = [];
}

