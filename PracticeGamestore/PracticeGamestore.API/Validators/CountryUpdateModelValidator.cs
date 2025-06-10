using FluentValidation;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Models.Country;

namespace PracticeGamestore.Validators;

public class CountryUpdateModelValidator : AbstractValidator<CountryUpdateRequestModel>
{
    public CountryUpdateModelValidator()
    {
        RuleFor(x => x.Name).
            HasValidName();

        RuleFor(x => x.Status)
            .IsInEnum();
    }
}