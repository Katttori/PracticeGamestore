namespace PracticeGamestore.Business.DataTransferObjects;

public class UserDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string Role { get; set; } = null!; 
    public string Status { get; set; } = null!;
    public Guid CountryId { get; set; }
    public DateTime BirthDate { get; set; }
    
    public UserDto(Guid id, string userName, string email, string phoneNumber, string passwordHash, string role, string status, Guid countryId, DateTime birthDate)
    {
        Id = id;
        UserName = userName;
        Email = email;
        PhoneNumber = phoneNumber;
        PasswordHash = passwordHash;
        Role = role;
        Status = status;
        CountryId = countryId;
        BirthDate = birthDate;
    }
}