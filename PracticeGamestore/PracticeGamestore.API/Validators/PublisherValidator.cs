using FluentValidation;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Models.Publisher;

namespace PracticeGamestore.Validators;

public class PublisherValidator : AbstractValidator<PublisherRequestModel>
{
    public PublisherValidator()
    {
        RuleFor(x => x.Name)
            .HasValidName();

        RuleFor(x => x.Description)
            .MaximumLength(ValidationConstants.StringLength.LongMaximum);

        RuleFor(x => x.PageUrl)
            .NotEmpty()
            .Must(IsValidPageUrl)
            .WithMessage(ErrorMessages.IncorrectPageUrl)
            .MaximumLength(ValidationConstants.StringLength.ShortMaximum);
    }

    private static bool IsValidPageUrl(string url) =>
         Uri.TryCreate(url, UriKind.Absolute, out var uri)
               && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
               && !string.IsNullOrEmpty(uri.Host);
}