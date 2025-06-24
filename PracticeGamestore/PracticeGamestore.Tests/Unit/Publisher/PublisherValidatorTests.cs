using FluentValidation.TestHelper;
using NUnit.Framework;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Validators;

namespace PracticeGamestore.Tests.Unit.Publisher;

[TestFixture]
public class PublisherValidatorTests
{
    private PublisherValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new PublisherValidator();
    }
    
    [TestCase(null, TestName = "Name is null")]
    [TestCase("", TestName = "Name is empty")]
    [TestCase(" ", TestName = "Name is blank")]
    [TestCase("a", TestName = "Name is too short")]
    public void WhenPublisherNameIsEmptyTooShortOrNull_ShouldHaveError(string? name)
    {
        // Arrange
        var publisher = TestData.Publisher.GeneratePublisherRequestModel();
        publisher.Name = name;
        
        // Act
        var result = _validator.TestValidate(publisher);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [TestCase("99999", TestName = "Name contains numbers")]
    [TestCase("John@Smith", TestName = "Name contains special characters")]
    [TestCase("Publisher123", TestName = "Name contains numbers at end")]
    public void WhenPublisherNameHasInvalidFormat_ShouldHaveCustomError(string name)
    {
        // Arrange
        var publisher = TestData.Publisher.GeneratePublisherRequestModel();
        publisher.Name = name;
        
        // Act
        var result = _validator.TestValidate(publisher);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage(ErrorMessages.IncorrectName);
    }
    
    [Test]
    public void WhenNameIsTooLong_ShouldHaveError()
    {
        // Arrange
        var publisher = TestData.Publisher.GeneratePublisherRequestModel();
        publisher.Name = TestData.StringConstants.LongerThatShortMaximum;
        
        // Act
        var result = _validator.TestValidate(publisher);
        
        // Assert 
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }
    
    [TestCase("Electronic Arts", TestName = "Valid publisher name")]
    [TestCase("O'Reilly Media", TestName = "Name with apostrophe")]
    [TestCase("Take-Two Interactive", TestName = "Name with hyphen")]
    [TestCase("Ubisoft Entertainment", TestName = "Multi-word name")]
    public void WhenPublisherNameIsCorrect_ShouldNotHaveError(string name)
    {
        // Arrange
        var publisher = TestData.Publisher.GeneratePublisherRequestModel();
        publisher.Name = name;
        
        // Act
        var result = _validator.TestValidate(publisher);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }
    
    [Test]
    public void WhenDescriptionIsTooLong_ShouldHaveError()
    {
        // Arrange
        var publisher = TestData.Publisher.GeneratePublisherRequestModel();
        publisher.Description = TestData.StringConstants.LongerThatLongMaximum;
        
        // Act
        var result = _validator.TestValidate(publisher);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [TestCase(null, TestName = "Description is null")]
    [TestCase("", TestName = "Description is empty")]
    [TestCase("Valid description", TestName = "Valid description")]
    public void WhenDescriptionIsValidOrEmpty_ShouldNotHaveError(string? description)
    {
        // Arrange
        var publisher = TestData.Publisher.GeneratePublisherRequestModel();
        publisher.Description = description;
        
        // Act
        var result = _validator.TestValidate(publisher);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }
    
    [TestCase(null, TestName = "PageUrl is null")]
    [TestCase("", TestName = "PageUrl is empty")]
    [TestCase(" ", TestName = "PageUrl is whitespace")]
    public void WhenPageUrlIsEmpty_ShouldHaveError(string? pageUrl)
    {
        // Arrange
        var publisher = TestData.Publisher.GeneratePublisherRequestModel();
        publisher.PageUrl = pageUrl;
        
        // Act
        var result = _validator.TestValidate(publisher);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PageUrl);
    }

    [TestCase("not-a-url", TestName = "Invalid URL format")]
    [TestCase("ftp://example.com", TestName = "FTP protocol not allowed")]
    [TestCase("http://", TestName = "URL without host")]
    [TestCase("https://", TestName = "HTTPS URL without host")]
    [TestCase("javascript:alert('xss')", TestName = "JavaScript protocol")]
    public void WhenPageUrlIsInvalid_ShouldHaveCustomError(string pageUrl)
    {
        // Arrange
        var publisher = TestData.Publisher.GeneratePublisherRequestModel();
        publisher.PageUrl = pageUrl;
        
        // Act
        var result = _validator.TestValidate(publisher);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PageUrl)
            .WithErrorMessage(ErrorMessages.IncorrectPageUrl);
    }

    [Test]
    public void WhenPageUrlIsTooLong_ShouldHaveError()
    {
        // Arrange
        var publisher = TestData.Publisher.GeneratePublisherRequestModel();
        publisher.PageUrl = TestData.StringConstants.LongerThatShortMaximum;
        
        // Act
        var result = _validator.TestValidate(publisher);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PageUrl);
    }

    [TestCase("https://www.ea.com", TestName = "Valid HTTPS URL")]
    [TestCase("http://www.ubisoft.com", TestName = "Valid HTTP URL")]
    [TestCase("https://www.take2games.com/labels", TestName = "URL with path")]
    [TestCase("https://www.activision.com/?lang=en", TestName = "URL with query parameters")]
    public void WhenPageUrlIsValid_ShouldNotHaveError(string pageUrl)
    {
        // Arrange
        var publisher = TestData.Publisher.GeneratePublisherRequestModel();
        publisher.PageUrl = pageUrl;
        
        // Act
        var result = _validator.TestValidate(publisher);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.PageUrl);
    }
    
    [Test]
    public void WhenAllPropertiesAreValid_ShouldPassValidation()
    {
        // Arrange
        var publisher = TestData.Publisher.GeneratePublisherRequestModel();
        
        // Act
        var result = _validator.TestValidate(publisher);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void WhenAllPropertiesAreInvalid_ShouldHaveMultipleErrors()
    {
        // Arrange
        var publisher = TestData.Publisher.GeneratePublisherRequestModel();
        publisher.Name = "";
        publisher.Description = new string('a', ValidationConstants.StringLength.LongMaximum + 1); // Too long (built-in message)
        publisher.PageUrl = "not-a-url";
        
        // Act
        var result = _validator.TestValidate(publisher);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);

        result.ShouldHaveValidationErrorFor(x => x.Description);
            
        result.ShouldHaveValidationErrorFor(x => x.PageUrl)
            .WithErrorMessage(ErrorMessages.IncorrectPageUrl);
    }
}
