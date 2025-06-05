using FluentValidation;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Models.Genre;
 
namespace PracticeGamestore.Validators;

public class GenreValidator : AbstractValidator<GenreRequestModel>
{
     public GenreValidator()
     {
         RuleFor(x => x.Name)
             .HasValidName();

         RuleFor(x => x.Description).
             HasCorrectDescription();

         RuleFor(x => x.ParentId)
             .NotEqual(Guid.Empty)
             .WithMessage(ErrorMessages.EmptyGuid)
             .When(x => x.ParentId.HasValue);
     }
}