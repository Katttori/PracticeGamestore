using PracticeGamestore.Business.Enums;

namespace PracticeGamestore.Models.User;

public class UserResponseModel
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public UserStatus Status { get; set; }
    public Guid CountryId { get; set; }
    public DateTime BirthDate { get; set; }
}