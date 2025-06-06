using FluentValidation;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Models.Platform;

namespace PracticeGamestore.Validators;

public class PlatformValidator : AbstractValidator<PlatformRequestModel>
{
    public PlatformValidator()
    {
        RuleFor(x => x.Name)
            .HasValidTitle();

        RuleFor(x => x.Description)
            .MaximumLength(ValidationConstants.StringLength.LongMaximum);
    }
}