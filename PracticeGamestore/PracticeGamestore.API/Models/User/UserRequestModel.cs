﻿using PracticeGamestore.Business.Enums;

namespace PracticeGamestore.Models.User;

public class UserRequestModel
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.User;
    public Guid CountryId { get; set; }
    public DateTime BirthDate { get; set; }
}