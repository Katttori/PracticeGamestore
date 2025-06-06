using FluentValidation;
using PracticeGamestore.Business.Constants;

namespace PracticeGamestore.Validators;

public static class CommonValidationRules
{
    public static IRuleBuilderOptions<T, string> HasValidName<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .Length(ValidationConstants.StringLength.Minimum, ValidationConstants.StringLength.ShortMaximum)
            .Matches(@"^[a-zA-Z\s'-]+$")
            .WithMessage(ErrorMessages.IncorrectName);
    }
    
    public static IRuleBuilderOptions<T, string> HasValidTitle<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .Length(ValidationConstants.StringLength.Minimum, ValidationConstants.StringLength.ShortMaximum);
    }

    public static IRuleBuilderOptions<T, string> HasCorrectEmail<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(ValidationConstants.StringLength.ShortMaximum);
    }
    

    public static IRuleBuilderOptions<T, Guid> HasCorrectId<T>(this IRuleBuilder<T, Guid> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }

    public static IRuleBuilderOptions<T, List<Guid>> HasCorrectIds<T>(this IRuleBuilder<T, List<Guid>> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .Must(ids => ids != null &&ids.All(id => id != Guid.Empty))
            .WithMessage(ErrorMessages.HasIncorrectIds);
    }
}
