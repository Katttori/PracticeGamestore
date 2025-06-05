using FluentValidation;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Business.DataTransferObjects.Filtering;
using PracticeGamestore.Business.Enums;

namespace PracticeGamestore.Validators;

public class GameFilterValidator : AbstractValidator<GameFilter>
{
    public GameFilterValidator()
    {
        RuleFor(x => x.MinPrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage(ErrorMessages.MustBeGreaterThanZero);

        RuleFor(x => x.MaxPrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage(ErrorMessages.MustBeGreaterThanZero);

        RuleFor(x => x)
            .Must(x => !x.MinPrice.HasValue || !x.MaxPrice.HasValue || x.MinPrice <= x.MaxPrice)
            .WithMessage(ErrorMessages.FirstCannotBeGreaterThanSecond("Minimum price", "maximum price"));

        RuleFor(x => x)
            .Must(x => !x.ReleaseDateStart.HasValue || !x.ReleaseDateEnd.HasValue || x.ReleaseDateStart <= x.ReleaseDateEnd)
            .WithMessage(ErrorMessages.FirstCannotBeGreaterThanSecond("Start release date", "end release date"));

        RuleFor(x => x.RatingFrom)
            .HasCorrectGameRating();

        RuleFor(x => x.RatingTo)
            .HasCorrectGameRating();

        RuleFor(x => x)
            .Must(x => !x.RatingFrom.HasValue || !x.RatingTo.HasValue || x.RatingFrom <= x.RatingTo)
            .WithMessage(ErrorMessages.FirstCannotBeGreaterThanSecond("Rating from", "rating to"));

        RuleFor(x => x.Order)
            .IsEnumName(typeof(OrderDirections), caseSensitive: false)
            .WithMessage(ErrorMessages.IncorrectOrdering);

        RuleFor(x => x.OrderBy)
            .Must(orderBy => orderBy.All(field => ValidationConstants.OrderByFields.Contains(field.Trim())))
            .WithMessage(ErrorMessages.IncorrectOrderByFields)
            .When(x => x.OrderBy is { Count: > 0 });

        RuleFor(x => x.Age)
            .Must(ages => ages.All(age => ValidationConstants.AgeRatingValues.Contains(age)))
            .WithMessage(ErrorMessages.InvalidAgeRating)
            .When(x => x.Age is { Count: > 0 });

        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage(ErrorMessages.MustBeGreaterThanZero);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, ValidationConstants.MaxPageSize)
            .WithMessage(ErrorMessages.InvalidPageSize);

        RuleFor(x => x.Name)
            .MaximumLength(ValidationConstants.StringLength.ShortMaximum)
            .WithMessage(ErrorMessages.StringIsTooLong);
    }
}
