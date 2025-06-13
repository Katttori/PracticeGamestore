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
    public void ShouldHaveError_WhenPublisherNameIsEmptyTooShortOrNull(string? name)
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
    public void ShouldHaveCustomError_WhenPublisherNameHasInvalidFormat(string name)
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
    public void ShouldHaveError_WhenNameIsTooLong()
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
    public void ShouldNotHaveError_WhenPublisherNameIsCorrect(string name)
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
    public void ShouldHaveError_WhenDescriptionIsTooLong()
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
    public void ShouldNotHaveError_WhenDescriptionIsValidOrEmpty(string? description)
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
    public void ShouldHaveError_WhenPageUrlIsEmpty(string? pageUrl)
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
    public void ShouldHaveCustomError_WhenPageUrlIsInvalid(string pageUrl)
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
    public void ShouldHaveError_WhenPageUrlIsTooLong()
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
    public void ShouldNotHaveError_WhenPageUrlIsValid(string pageUrl)
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
    public void ShouldPassValidation_WhenAllPropertiesAreValid()
    {
        // Arrange
        var publisher = TestData.Publisher.GeneratePublisherRequestModel();
        
        // Act
        var result = _validator.TestValidate(publisher);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void ShouldHaveMultipleErrors_WhenAllPropertiesAreInvalid()
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
