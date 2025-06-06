using FluentValidation;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Models.Game;

namespace PracticeGamestore.Validators;

public class GameValidator : AbstractValidator<GameRequestModel>
{
    public GameValidator()
    {
        RuleFor(x => x.Name)
            .HasValidTitle();

        RuleFor(x => x.Key)
            .HasValidTitle();

        RuleFor(x => x.Price)
            .GreaterThan(0);

        RuleFor(x => x.Description)
            .MaximumLength(ValidationConstants.StringLength.LongMaximum);
        
        RuleFor(x => x.Rating)
            .InclusiveBetween
                (ValidationConstants.GameRating.Min, ValidationConstants.GameRating.Max)
            .WithMessage(ErrorMessages.IncorrectGameRating);

        RuleFor(x => x.AgeRating)
            .Must(age => ValidationConstants.AgeRatingValues.Contains(age))
            .WithMessage(ErrorMessages.InvalidAgeRating);

        RuleFor(x => x.ReleaseDate)
            .LessThanOrEqualTo(DateTime.Today)
            .WithMessage(ErrorMessages.InvalidReleaseDate);
        
        RuleFor(x => x.PublisherId)
            .HasCorrectId();

        RuleFor(x => x.PlatformIds)
            .HasCorrectIds();

        RuleFor(x => x.GenreIds)
            .HasCorrectIds();
    }
}