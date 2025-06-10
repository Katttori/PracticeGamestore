using FluentValidation.TestHelper;
using NUnit.Framework;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Models.File;
using PracticeGamestore.Validators;

namespace PracticeGamestore.Tests.Unit.File;

[TestFixture]
public class FileValidatorTests
{
    private FileValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new FileValidator();
    }
    
    [Test]
    public void ShouldHaveError_WhenGameIdIsEmpty()
    {
        // Arrange
        var fileRequest = new FileRequestModel
        {
            GameId = Guid.Empty,
            File = TestData.File.GenerateFile($"test{ValidationConstants.GameFile.AllowedExtensions[0]}", ValidationConstants.GameFile.MinSize)
        };

        // Act
        var result = _validator.TestValidate(fileRequest);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GameId);
    }

    [Test]
    public void ShouldNotHaveError_WhenGameIdIsValid()
    {
        // Arrange
        var fileRequest = new FileRequestModel
        {
            GameId = Guid.NewGuid(),
            File = TestData.File.GenerateFile($"test{ValidationConstants.GameFile.AllowedExtensions[0]}", ValidationConstants.GameFile.MinSize)
        };

        // Act
        var result = _validator.TestValidate(fileRequest);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.GameId);
    }

    [Test]
    public void ShouldHaveError_WhenFileIsNull()
    {
        // Arrange
        var fileRequest = new FileRequestModel
        {
            GameId = Guid.NewGuid(),
            File = null!
        };

        // Act
        var result = _validator.TestValidate(fileRequest);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.File)
            .WithErrorMessage(ErrorMessages.InvalidGameFile);
    }

    [Test]
    public void ShouldHaveError_WhenFileIsEmpty()
    {
        // Arrange
        var fileRequest = new FileRequestModel
        {
            GameId = Guid.NewGuid(),
            File = TestData.File.GenerateFile($"test{ValidationConstants.GameFile.AllowedExtensions[0]}", 0)
        };

        // Act
        var result = _validator.TestValidate(fileRequest);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.File)
            .WithErrorMessage(ErrorMessages.InvalidGameFile);
    }

    [Test]
    public void ShouldHaveError_WhenFileIsTooLarge()
    {
        // Arrange
        var fileRequest = new FileRequestModel
        {
            GameId = Guid.NewGuid(),
            File = TestData.File.GenerateFile($"test{ValidationConstants.GameFile.AllowedExtensions[0]}", ValidationConstants.GameFile.MaxSize+1)
        };

        // Act
        var result = _validator.TestValidate(fileRequest);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.File)
            .WithErrorMessage(ErrorMessages.InvalidGameFile);
    }

    [TestCase(".txt", TestName = "Text file")]
    [TestCase(".pdf", TestName = "PDF file")]
    [TestCase(".doc", TestName = "Word document")]
    [TestCase(".mp3", TestName = "Audio file")]
    [TestCase(".mp4", TestName = "Video file")]
    [TestCase(".jpg", TestName = "Image file")]
    public void ShouldHaveError_WhenFileHasInvalidExtension(string extension)
    {
        // Arrange
        var fileRequest = new FileRequestModel
        {
            GameId = Guid.NewGuid(),
            File = TestData.File.GenerateFile($"test{extension}", ValidationConstants.GameFile.MinSize)
        };

        // Act
        var result = _validator.TestValidate(fileRequest);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.File)
            .WithErrorMessage(ErrorMessages.InvalidGameFile);
    }

    [TestCase(".zip", TestName = "ZIP file")]
    [TestCase(".rar", TestName = "RAR file")]
    [TestCase(".exe", TestName = "Executable file")]
    [TestCase(".msi", TestName = "MSI installer")]
    [TestCase(".dmg", TestName = "DMG file")]
    [TestCase(".pkg", TestName = "PKG file")]
    [TestCase(".deb", TestName = "DEB package")]
    [TestCase(".rpm", TestName = "RPM package")]
    [TestCase(".7z", TestName = "7ZIP file")]
    public void ShouldNotHaveError_WhenFileHasValidExtension(string extension)
    {
        // Arrange
        var fileRequest = new FileRequestModel
        {
            GameId = Guid.NewGuid(),
            File = TestData.File.GenerateFile($"test{extension}", ValidationConstants.GameFile.MinSize)
        };

        // Act
        var result = _validator.TestValidate(fileRequest);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.File);
    }

    [TestCase(".ZIP", TestName = "Uppercase ZIP")]
    [TestCase(".Exe", TestName = "Mixed case EXE")]
    [TestCase(".RAR", TestName = "Uppercase RAR")]
    public void ShouldNotHaveError_WhenFileHasValidExtensionInDifferentCase(string extension)
    {
        // Arrange
        var fileRequest = new FileRequestModel
        {
            GameId = Guid.NewGuid(),
            File = TestData.File.GenerateFile($"test{extension}", ValidationConstants.GameFile.MinSize)
        };

        // Act
        var result = _validator.TestValidate(fileRequest);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.File);
    }

    [Test]
    public void ShouldHaveError_WhenFileNameHasNoExtension()
    {
        // Arrange
        var fileRequest = new FileRequestModel
        {
            GameId = Guid.NewGuid(),
            File = TestData.File.GenerateFile("test", ValidationConstants.GameFile.MinSize)
        };

        // Act
        var result = _validator.TestValidate(fileRequest);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.File)
            .WithErrorMessage(ErrorMessages.InvalidGameFile);
    }

    [TestCase(ValidationConstants.GameFile.MinSize, TestName = "Minimum valid size")]
    [TestCase(ValidationConstants.GameFile.MaxSize - ValidationConstants.GameFile.MinSize, TestName = "Medium size")]
    [TestCase(ValidationConstants.GameFile.MaxSize, TestName = "Maximum valid size")]
    public void ShouldNotHaveError_WhenFileSizeIsValid(long fileSize)
    {
        // Arrange
        var fileRequest = new FileRequestModel
        {
            GameId = Guid.NewGuid(),
            File = TestData.File.GenerateFile($"test{ValidationConstants.GameFile.AllowedExtensions[0]}", fileSize)
        };

        // Act
        var result = _validator.TestValidate(fileRequest);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.File);
    }

    [Test]
    public void ShouldNotHaveError_WhenAllPropertiesAreValid()
    {
        // Arrange
        var fileRequest = new FileRequestModel
        {
            GameId = Guid.NewGuid(),
            File = TestData.File.GenerateFile($"test{ValidationConstants.GameFile.AllowedExtensions[0]}", ValidationConstants.GameFile.MinSize)
        };

        // Act
        var result = _validator.TestValidate(fileRequest);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void ShouldHaveMultipleErrors_WhenMultiplePropertiesAreInvalid()
    {
        // Arrange
        var fileRequest = new FileRequestModel
        {
            GameId = Guid.Empty,
            File = TestData.File.GenerateFile("invalid.txt", 0)
        };

        // Act
        var result = _validator.TestValidate(fileRequest);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GameId);
        result.ShouldHaveValidationErrorFor(x => x.File);
    }
}