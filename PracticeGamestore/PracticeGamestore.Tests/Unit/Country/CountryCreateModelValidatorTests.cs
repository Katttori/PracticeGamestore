using FluentValidation.TestHelper;
using NUnit.Framework;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Validators;

namespace PracticeGamestore.Tests.Unit.Country;

[TestFixture]
public class CountryCreateModelValidatorTests
{
    private CountryCreateModelValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new CountryCreateModelValidator();
    }
    
    [TestCase(null, TestName = "Name is null")]
    [TestCase("", TestName = "Name is empty")]
    [TestCase(" ", TestName = "Name is blank")]
    [TestCase("a", TestName = "Name is too short")]
    public void ShouldHaveError_WhenCountryNameIsEmptyTooShortOrNull(string? name)
    {
        // Arrange
        var country = TestData.Country.GenerateCountryCreateRequestModel();
        country.Name = name;
        
        // Act
        var result = _validator.TestValidate(country);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [TestCase("99999", TestName = "Name contains numbers")]
    [TestCase("USA123", TestName = "Name contains numbers at end")]
    [TestCase("Coun@try", TestName = "Name contains special characters")]
    [TestCase("Unit#ed States", TestName = "Name contains hash symbol")]
    public void ShouldHaveCustomError_WhenCountryNameHasInvalidFormat(string name)
    {
        // Arrange
        var country = TestData.Country.GenerateCountryCreateRequestModel();
        country.Name = name;
        
        // Act
        var result = _validator.TestValidate(country);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage(ErrorMessages.IncorrectName);
    }
    
    [Test]
    public void ShouldHaveError_WhenNameIsTooLong()
    {
        // Arrange
        var country = TestData.Country.GenerateCountryCreateRequestModel();
        country.Name = TestData.StringConstants.LongerThatShortMaximum;
        
        // Act
        var result = _validator.TestValidate(country);
        
        // Assert 
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }
    
    [TestCase("Canada", TestName = "Simple country name")]
    [TestCase("United States", TestName = "Multi-word country name")]
    [TestCase("United Kingdom", TestName = "Two-word country name")]
    [TestCase("Côte d'Ivoire", TestName = "Country name with apostrophe")]
    [TestCase("Bosnia-Herzegovina", TestName = "Country name with hyphen")]
    public void ShouldNotHaveError_WhenCountryNameIsValid(string name)
    {
        // Arrange
        var country = TestData.Country.GenerateCountryCreateRequestModel();
        country.Name = name;
        
        // Act
        var result = _validator.TestValidate(country);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Test]
    public void ShouldNotHaveAnyErrors_WhenModelIsValid()
    {
        // Arrange
        var country = TestData.Country.GenerateCountryCreateRequestModel();
        
        // Act
        var result = _validator.TestValidate(country);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}