using PracticeGamestore.Business.Enums;

namespace PracticeGamestore.Business.DataTransferObjects;

public class UserDto
{
    public Guid? Id { get; set; }
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Role { get; set; } = null!; 
    public UserStatus Status { get; set; }
    public Guid CountryId { get; set; }
    public DateTime BirthDate { get; set; }
    
    public UserDto(Guid? id, string userName, string email, string phoneNumber, string password, string role, UserStatus status, Guid countryId, DateTime birthDate)
    {
        Id = id;
        UserName = userName;
        Email = email;
        PhoneNumber = phoneNumber;
        Password = password;
        Role = role;
        Status = status;
        CountryId = countryId;
        BirthDate = birthDate;
    }
}