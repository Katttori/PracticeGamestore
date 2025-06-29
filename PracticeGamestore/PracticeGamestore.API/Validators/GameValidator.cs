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

        RuleFor(x => x.Picture)
            .Must(p => p.Length is <= ValidationConstants.GamePicture.MaximumPictureSize
                           and >= ValidationConstants.GamePicture.MinimumPictureSize &&
                       IsValidPictureFormat(p))
            .When(x => x.Picture != null)
            .WithMessage(ErrorMessages.IncorrectPictureFormat);
        
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
    
    private static bool IsValidPictureFormat(byte[] bytes) =>
         bytes.Length >= 4 && ValidationConstants.GamePicture.AllowedPictureFormats
             .Any(format => format.Value
                 .Any(signature => bytes.Length >= signature.Length &&
                                   signature.Select((b, i) => b == 0x00 || bytes[i] == b)
                                       .All(x => x)));

}