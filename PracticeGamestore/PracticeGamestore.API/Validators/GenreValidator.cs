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

         RuleFor(x => x.Description)
             .MaximumLength(ValidationConstants.StringLength.LongMaximum);

         RuleFor(x => x.ParentId)
             .NotEqual(Guid.Empty)
             .When(x => x.ParentId.HasValue);
     }
}