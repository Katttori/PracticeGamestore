using PracticeGamestore.DataAccess.Enums;

namespace PracticeGamestore.DataAccess.Entities;

public class User
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "User"; // Default role is User
    public UserStatus Status { get; set; } = UserStatus.Active;
    public Guid CountryId { get; set; }
    public DateTime BirthDate { get; set; }
    public Country Country { get; set; } = null!;
}