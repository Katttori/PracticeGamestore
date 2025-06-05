using FluentValidation;
using PracticeGamestore.Business.Constants;

namespace PracticeGamestore.Validators;

public static class CommonValidationRules
{
    public static IRuleBuilderOptions<T, string> HasValidName<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage(ErrorMessages.EmptyField)
            .Length(ValidationConstants.StringLength.Minimum, ValidationConstants.StringLength.ShortMaximum)
            .WithMessage(ErrorMessages.IncorrectLength);
    }

    public static IRuleBuilderOptions<T, string> HasCorrectDescription<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(ValidationConstants.StringLength.LongMaximum)
            .WithMessage(ErrorMessages.StringIsTooLong);
    }

    public static IRuleBuilderOptions<T, string> HasCorrectEmail<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .EmailAddress()
            .WithMessage(ErrorMessages.IncorrectEmail)
            .MaximumLength(ValidationConstants.StringLength.ShortMaximum)
            .WithMessage(ErrorMessages.StringIsTooLong);
    }

    public static IRuleBuilderOptions<T, decimal> HasCorrectPrice<T>(this IRuleBuilder<T, decimal> ruleBuilder)
    {
        return ruleBuilder
            .GreaterThan(0)
            .WithMessage(ErrorMessages.MustBeGreaterThanZero);
    }

    public static IRuleBuilderOptions<T, Guid> HasCorrectId<T>(this IRuleBuilder<T, Guid> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .NotEqual(Guid.Empty)
            .WithMessage(ErrorMessages.EmptyGuid);
    }

    public static IRuleBuilderOptions<T, List<Guid>> HasCorrectIds<T>(this IRuleBuilder<T, List<Guid>> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .Must(ids => ids.All(id => id != Guid.Empty))
            .WithMessage(ErrorMessages.HasIncorrectIds);
    }

    public static IRuleBuilderOptions<T, double?> HasCorrectGameRating<T>(
        this IRuleBuilder<T, double?> ruleBuilder)
    {
        return ruleBuilder
            .InclusiveBetween(ValidationConstants.GameRating.Min, ValidationConstants.GameRating.Max)
            .WithMessage(ErrorMessages.IncorrectGameRating);
    }
}
