using FluentValidation.TestHelper;
using NUnit.Framework;
using PracticeGamestore.Tests.TestData;
using PracticeGamestore.Validators;

namespace PracticeGamestore.Tests.Unit.Platform;

[TestFixture]
public class PlatformValidatorTests
{
    private PlatformValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new PlatformValidator();
    }

    [TestCase(null, TestName = "Name is null")]
    [TestCase("", TestName = "Name is empty")]
    [TestCase(" ", TestName = "Name is whitespace")]
    [TestCase("a", TestName = "Name is too short")]
    public void WhenNameIsEmptyTooShortOrNull_ShouldHaveError(string? name)
    {
        // Arrange
        var platform = TestData.Platform.GeneratePlatformRequestModel();
        platform.Name = name;
        
        // Act
        var result = _validator.TestValidate(platform);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Test]
    public void WhenNameIsTooLong_ShouldHaveError()
    {
        // Arrange
        var platform = TestData.Platform.GeneratePlatformRequestModel();
        platform.Name = StringConstants.LongerThatShortMaximum;
        
        // Act
        var result = _validator.TestValidate(platform);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [TestCase("PC", TestName = "Simple platform name")]
    [TestCase("PlayStation 5", TestName = "Platform with number")]
    [TestCase("Xbox Series X", TestName = "Multi-word platform name")]
    [TestCase("Nintendo Switch", TestName = "Two-word platform name")]
    [TestCase("Steam Deck", TestName = "Platform with descriptive name")]
    [TestCase("PS5", TestName = "Abbreviated platform name")]
    [TestCase("MacOS", TestName = "Operating system platform")]
    public void WhenNameIsValid_ShouldNotHaveError(string name)
    {
        // Arrange
        var platform = TestData.Platform.GeneratePlatformRequestModel();
        platform.Name = name;
        
        // Act
        var result = _validator.TestValidate(platform);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Test]
    public void WhenDescriptionIsTooLong_ShouldHaveError()
    {
        // Arrange
        var platform = TestData.Platform.GeneratePlatformRequestModel();
        platform.Description = StringConstants.LongerThatLongMaximum;
        
        // Act
        var result = _validator.TestValidate(platform);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [TestCase(null, TestName = "Description is null")]
    [TestCase("", TestName = "Description is empty")]
    [TestCase("Personal Computer", TestName = "Simple description")]
    public void WhenDescriptionIsValidOrEmpty_ShouldNotHaveError(string? description)
    {
        // Arrange
        var platform = TestData.Platform.GeneratePlatformRequestModel();
        platform.Description = description;
        
        // Act
        var result = _validator.TestValidate(platform);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }

    [Test]
    public void WhenModelIsValid_ShouldNotHaveAnyErrors()
    {
        // Arrange
        var platform = TestData.Platform.GeneratePlatformRequestModel();
        
        // Act
        var result = _validator.TestValidate(platform);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void WhenModelIsValidWithDescription_ShouldNotHaveAnyErrors()
    {
        // Arrange
        var platform = TestData.Platform.GeneratePlatformRequestModel();
        platform.Description = "Advanced gaming console with 4K capabilities";
        
        // Act
        var result = _validator.TestValidate(platform);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void WhenModelHasMultipleInvalidFields_ShouldHaveMultipleErrors()
    {
        // Arrange
        var platform = TestData.Platform.GeneratePlatformRequestModel();
        platform.Name = "";
        platform.Description = StringConstants.LongerThatLongMaximum;
        
        // Act
        var result = _validator.TestValidate(platform);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }
}