using FluentValidation;
using PracticeGamestore.Models.Platform;

namespace PracticeGamestore.Validators;

public class PlatformValidator : AbstractValidator<PlatformRequestModel>
{
    public PlatformValidator()
    {
        RuleFor(x => x.Name)
            .HasValidName();

        RuleFor(x => x.Description)
            .HasCorrectDescription();
    }
}