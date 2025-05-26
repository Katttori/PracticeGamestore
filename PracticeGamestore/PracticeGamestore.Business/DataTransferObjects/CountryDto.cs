using PracticeGamestore.DataAccess.Enums;

namespace PracticeGamestore.Business.DataTransferObjects;

public class CountryDto
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public CountryStatus Status { get; set; }
    
    public CountryDto(Guid? id, string name, CountryStatus status)
    {
        Id = id;
        Name = name;
        Status = status;
    }
}