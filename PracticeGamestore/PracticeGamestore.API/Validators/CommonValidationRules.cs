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

    public static IRuleBuilderOptions<T, string> HasValidPhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .Matches(@"^\+?[0-9\s-]+$")
            .WithMessage(ErrorMessages.IncorrectPhoneNumber)
            .Length(ValidationConstants.PhoneNumber.MinLength, ValidationConstants.PhoneNumber.MaxLength);
    }

    public static IRuleBuilderOptions<T, IFormFile?> IsValidFile<T>(this IRuleBuilder<T, IFormFile?> ruleBuilder, List<string> allowedExtensions)
    {
        return ruleBuilder
            .Must(x =>
            {
                if (x == null || x.Length > ValidationConstants.GameFile.MaxSize) return false;
                var extension = Path.GetExtension(x.FileName)?.ToLowerInvariant();
                return !string.IsNullOrEmpty(extension) && allowedExtensions.Contains(extension);
            }).WithMessage(ErrorMessages.InvalidGameFile);
    }

    public static IRuleBuilderOptions<T, string> HasSecurePassword<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .Length(ValidationConstants.Password.MinLength, ValidationConstants.Password.MaxLength)
            .Must(password => 
                password != null &&
                password.Any(char.IsUpper) &&
                password.Any(char.IsLower) &&
                password.Any(char.IsDigit) &&
                ValidationConstants.Password.SpecialCharRegex.IsMatch(password))
            .WithMessage(ErrorMessages.InsecurePassword);
    }
}
