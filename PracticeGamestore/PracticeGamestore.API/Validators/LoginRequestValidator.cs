using FluentValidation;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Models.Auth;

namespace PracticeGamestore.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .HasCorrectEmail();

        RuleFor(x => x.Password)
            .Length(ValidationConstants.Password.MinLength, ValidationConstants.Password.MaxLength)
            .Must(password => !string.IsNullOrWhiteSpace(password?.Trim()))
            .WithMessage(ErrorMessages.PasswordRequired);
    }
}