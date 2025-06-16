using FluentValidation;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Models.File;

namespace PracticeGamestore.Validators;

public class FileValidator : AbstractValidator<FileRequestModel>
{
    public FileValidator()
    {
        RuleFor(x => x.GameId)
            .HasCorrectId();

        RuleFor(x => x.File)
            .IsValidFile(ValidationConstants.GameFile.AllowedExtensions);
    }
}