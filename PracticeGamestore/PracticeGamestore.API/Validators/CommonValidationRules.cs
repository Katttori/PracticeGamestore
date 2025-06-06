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
            .Matches(@"^[\p{L}\s'-]+$")
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
            .MaximumLength(ValidationConstants.StringLength.ShortMaximum)
            .EmailAddress()
            .Must(x =>
            {   if (x == null) return false;
                var atIndex = x.LastIndexOf('@');
                var domain = x.Substring(atIndex + 1);
                return !x.Contains(' ') && domain.Contains('.') &&
                       domain.Split('.').Length >= 2 &&
                       domain.Split('.').All(part => !string.IsNullOrEmpty(part)) &&
                       !domain.StartsWith('.') &&
                       !domain.EndsWith('.');
            });
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
