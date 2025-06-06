using FluentValidation.TestHelper;
using NUnit.Framework;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Validators;
using PracticeGamestore.Tests.TestData;

namespace PracticeGamestore.Tests.Unit.Validators;

[TestFixture]
public class GenreValidatorTests
{
    private GenreValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new GenreValidator();
    }

    [TestCase(null, TestName = "Name is null")]
    [TestCase("", TestName = "Name is empty")]
    [TestCase(" ", TestName = "Name is whitespace")]
    [TestCase("a", TestName = "Name is too short")]
    public void ShouldHaveError_WhenNameIsEmptyTooShortOrNull(string? name)
    {
        // Arrange
        var genre = TestData.Genre.GenerateGenreRequestModel();
        genre.Name = name;
        
        // Act
        var result = _validator.TestValidate(genre);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Test]
    public void ShouldHaveError_WhenNameIsTooLong()
    {
        // Arrange
        var genre = TestData.Genre.GenerateGenreRequestModel();
        genre.Name = StringConstants.LongerThatShortMaximum;
        
        // Act
        var result = _validator.TestValidate(genre);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [TestCase("99999", TestName = "Name contains numbers")]
    [TestCase("Action@Game", TestName = "Name contains special characters")]
    [TestCase("FPS123", TestName = "Name contains numbers at end")]
    [TestCase("Shoot@em-up", TestName = "Name contains @ symbol")]
    [TestCase("RPG#1", TestName = "Name contains hash symbol")]
    public void ShouldHaveCustomError_WhenNameHasInvalidFormat(string name)
    {
        // Arrange
        var genre = TestData.Genre.GenerateGenreRequestModel();
        genre.Name = name;
        
        // Act
        var result = _validator.TestValidate(genre);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage(ErrorMessages.IncorrectName);
    }

    [TestCase("Action", TestName = "Simple valid name")]
    [TestCase("FPS", TestName = "Acronym name")]
    [TestCase("Beat 'em up", TestName = "Name with apostrophe")]
    [TestCase("Role-Playing Game", TestName = "Name with hyphen")]
    [TestCase("Real Time Strategy", TestName = "Multi-word name")]
    [TestCase("Action RPG", TestName = "Combined genre name")]
    [TestCase("First-Person Shooter", TestName = "Hyphenated genre name")]
    [TestCase("Tower Defense", TestName = "Two-word genre")]
    public void ShouldNotHaveError_WhenNameIsValid(string name)
    {
        // Arrange
        var genre = TestData.Genre.GenerateGenreRequestModel();
        genre.Name = name;
        
        // Act
        var result = _validator.TestValidate(genre);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Test]
    public void ShouldHaveError_WhenDescriptionIsTooLong()
    {
        // Arrange
        var genre = TestData.Genre.GenerateGenreRequestModel();
        genre.Description = StringConstants.LongerThatLongMaximum;
        
        // Act
        var result = _validator.TestValidate(genre);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [TestCase(null, TestName = "Description is null")]
    [TestCase("", TestName = "Description is empty")]
    [TestCase("Fast-paced action games", TestName = "Valid description")]
    [TestCase("Strategic thinking and planning games", TestName = "Longer valid description")]
    public void ShouldNotHaveError_WhenDescriptionIsValidOrEmpty(string? description)
    {
        // Arrange
        var genre = TestData.Genre.GenerateGenreRequestModel();
        genre.Description = description;
        
        // Act
        var result = _validator.TestValidate(genre);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }

    [Test]
    public void ShouldHaveError_WhenParentIdIsEmpty()
    {
        // Arrange
        var genre = TestData.Genre.GenerateGenreRequestModel();
        genre.ParentId = Guid.Empty;
        
        // Act
        var result = _validator.TestValidate(genre);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ParentId);
    }

    [Test]
    public void ShouldNotHaveError_WhenParentIdIsNull()
    {
        // Arrange
        var genre = TestData.Genre.GenerateGenreRequestModel();
        genre.ParentId = null;
        
        // Act
        var result = _validator.TestValidate(genre);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.ParentId);
    }

    [Test]
    public void ShouldNotHaveError_WhenParentIdIsValidGuid()
    {
        // Arrange
        var genre = TestData.Genre.GenerateGenreRequestModel();
        genre.ParentId = Guid.NewGuid();
        
        // Act
        var result = _validator.TestValidate(genre);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.ParentId);
    }

    [Test]
    public void ShouldNotHaveAnyErrors_WhenModelIsValid()
    {
        // Arrange
        var genre = TestData.Genre.GenerateGenreRequestModel();
        
        // Act
        var result = _validator.TestValidate(genre);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void ShouldNotHaveAnyErrors_WhenModelIsValidWithParentId()
    {
        // Arrange
        var genre = TestData.Genre.GenerateGenreRequestModel();
        genre.ParentId = Guid.NewGuid();
        
        // Act
        var result = _validator.TestValidate(genre);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void ShouldNotHaveAnyErrors_WhenModelIsValidWithDescription()
    {
        // Arrange
        var genre = TestData.Genre.GenerateGenreRequestModel();
        genre.Description = "Fast-paced action games with intense gameplay";
        
        // Act
        var result = _validator.TestValidate(genre);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Test]
    public void ShouldHaveMultipleErrors_WhenModelHasMultipleInvalidFields()
    {
        // Arrange
        var genre = TestData.Genre.GenerateGenreRequestModel();
        genre.Name = "";
        genre.Description = StringConstants.LongerThatLongMaximum;
        genre.ParentId = Guid.Empty;
        
        // Act
        var result = _validator.TestValidate(genre);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
        result.ShouldHaveValidationErrorFor(x => x.Description);
        result.ShouldHaveValidationErrorFor(x => x.ParentId);
    }
}