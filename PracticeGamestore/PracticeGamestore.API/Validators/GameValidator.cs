using FluentValidation;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Models.Game;

namespace PracticeGamestore.Validators;

public class GameValidator : AbstractValidator<GameRequestModel>
{
    public GameValidator()
    {
        RuleFor(x => x.Name)
            .HasValidName();

        RuleFor(x => x.Key)
            .HasValidName();

        RuleFor(x => x.Price)
            .HasCorrectPrice();

        RuleFor(x => x.Description)
            .HasCorrectDescription();

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