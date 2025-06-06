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
            .When(x => x.MinPrice.HasValue);

        RuleFor(x => x.MaxPrice)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MaxPrice.HasValue);

        RuleFor(x => x)
            .Must(x => !x.MinPrice.HasValue || !x.MaxPrice.HasValue || x.MinPrice <= x.MaxPrice)
            .WithMessage(ErrorMessages.FirstCannotBeGreaterThanSecond("Minimum price", "maximum price"));

        RuleFor(x => x.ReleaseDateStart)
            .LessThanOrEqualTo(DateTime.Today)
            .WithMessage(ErrorMessages.InvalidReleaseDate)
            .When(x => x.ReleaseDateStart.HasValue);
        
        RuleFor(x => x.ReleaseDateEnd)
            .LessThanOrEqualTo(DateTime.Today)
            .WithMessage(ErrorMessages.InvalidReleaseDate)
            .When(x => x.ReleaseDateEnd.HasValue);
        
        RuleFor(x => x)
            .Must(x => !x.ReleaseDateStart.HasValue || !x.ReleaseDateEnd.HasValue || x.ReleaseDateStart <= x.ReleaseDateEnd)
            .WithMessage(ErrorMessages.FirstCannotBeGreaterThanSecond("Start release date", "end release date"));

        RuleFor(x => x.RatingFrom)
            .InclusiveBetween(ValidationConstants.GameRating.Min, ValidationConstants.GameRating.Max)
            .When(x => x.RatingFrom.HasValue); 
        
        RuleFor(x => x.RatingTo)
            .InclusiveBetween(ValidationConstants.GameRating.Min, ValidationConstants.GameRating.Max)
            .When(x => x.RatingTo.HasValue); 

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
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, ValidationConstants.MaxPageSize);

        RuleFor(x => x.Name)
            .MaximumLength(ValidationConstants.StringLength.ShortMaximum);
    }
}
