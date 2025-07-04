﻿using FluentValidation;
using PracticeGamestore.Business.Constants;
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

        RuleFor(x => x.Password)
            .HasSecurePassword();
        
        RuleFor(x => x.Role)
            .Must(role => ValidationConstants.UserRoleValues.Contains(role.ToString()))
            .WithMessage(ErrorMessages.InvalidRole);
    }
}