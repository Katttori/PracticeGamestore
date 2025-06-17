using FluentValidation;
using PracticeGamestore.Models.User;

namespace PracticeGamestore.Validators;

public class UserValidator : AbstractValidator<UserRequestModel>
{
    public UserValidator()
    {
        RuleFor(x => x.UserName)
            .HasValidName();

        RuleFor(x => x.Email)
            .HasCorrectEmail();
        
        RuleFor(x => x.PhoneNumber)
            .HasValidPhoneNumber();

        RuleFor(x => x.CountryId)
            .HasCorrectId();

        RuleFor(x => x.BirthDate)
            .LessThan(DateTime.UtcNow);

        RuleFor(x => x.Role)
            .NotEmpty();
        
        RuleFor(x => x.Password)
            .HasSecurePassword();
    }
}