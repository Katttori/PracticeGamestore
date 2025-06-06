using FluentValidation;
using PracticeGamestore.Models.Country;

namespace PracticeGamestore.Validators;

public class CountryCreateModelValidator : AbstractValidator<CountryCreateRequestModel>
{
    public CountryCreateModelValidator()
    {
        RuleFor(x => x.Name)
            .HasValidName();
    }
}