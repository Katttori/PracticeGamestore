using FluentValidation;
using PracticeGamestore.Models.Blacklist;

namespace PracticeGamestore.Validators;

public class BlackListValidator : AbstractValidator<BlacklistRequestModel>
{
    public BlackListValidator()
    {
        RuleFor(x => x.UserEmail)
            .HasCorrectEmail();

        RuleFor(x => x.CountryId)
            .HasCorrectId();
    }
}