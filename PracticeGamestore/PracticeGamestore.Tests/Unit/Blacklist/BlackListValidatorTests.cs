using FluentValidation.TestHelper;
using NUnit.Framework;
using PracticeGamestore.Validators;

namespace PracticeGamestore.Tests.Unit.Blacklist;

[TestFixture]
public class BlackListValidatorTests
{
    private BlackListValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new BlackListValidator();
    }

    [TestCase(null, TestName = "Email is null")]
    [TestCase("", TestName = "Email is empty")]
    [TestCase(" ", TestName = "Email is whitespace")]
    [TestCase("invalid-email", TestName = "Email without @ symbol")]
    [TestCase("@domain.com", TestName = "Email missing local part")]
    [TestCase("user@", TestName = "Email missing domain")]
    [TestCase("user.domain.com", TestName = "Email missing @ symbol")]
    [TestCase("user@domain", TestName = "Email missing TLD")]
    [TestCase("user name@domain.com", TestName = "Email with space in local part")]
    public void ShouldHaveError_WhenUserEmailIsInvalid(string? email)
    {
        // Arrange
        var blacklist = TestData.Blacklist.GenerateBlacklistRequestModel();
        blacklist.UserEmail = email;
        
        // Act
        var result = _validator.TestValidate(blacklist);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserEmail);
    }
    
    [Test]
    public void ShouldHaveError_WhenUserEmailIsTooLong()
    {
        // Arrange
        var blacklist = TestData.Blacklist.GenerateBlacklistRequestModel();
        blacklist.UserEmail = TestData.StringConstants.LongerThatShortMaximum;
        
        // Act
        var result = _validator.TestValidate(blacklist);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserEmail);
    }

    [TestCase("test@example.com", TestName = "Standard valid email")]
    [TestCase("user.name@domain.co.uk", TestName = "Email with subdomain")]
    [TestCase("user+tag@example.org", TestName = "Email with plus sign")]
    [TestCase("user_name@example-domain.com", TestName = "Email with underscore and hyphen")]
    [TestCase("firstname.lastname@company.travel", TestName = "Email with long TLD")]
    [TestCase("a@b.co", TestName = "Minimal valid email")]
    public void ShouldNotHaveError_WhenUserEmailIsValid(string email)
    {
        // Arrange
        var blacklist = TestData.Blacklist.GenerateBlacklistRequestModel();
        blacklist.UserEmail = email;
        
        // Act
        var result = _validator.TestValidate(blacklist);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.UserEmail);
    }

    [Test]
    public void ShouldHaveError_WhenCountryIdIsEmpty()
    {
        // Arrange
        var blacklist = TestData.Blacklist.GenerateBlacklistRequestModel();
        blacklist.CountryId = Guid.Empty;
        
        // Act
        var result = _validator.TestValidate(blacklist);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CountryId);
    }

    [Test]
    public void ShouldNotHaveAnyErrors_WhenModelIsValid()
    {
        // Arrange
        var blacklist = TestData.Blacklist.GenerateBlacklistRequestModel();
        
        // Act
        var result = _validator.TestValidate(blacklist);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void ShouldHaveMultipleErrors_WhenAllFieldsAreInvalid()
    {
        // Arrange
        var blacklist = TestData.Blacklist.GenerateBlacklistRequestModel();
        blacklist.UserEmail = "invalid-email";
        blacklist.CountryId = Guid.Empty;
        
        // Act
        var result = _validator.TestValidate(blacklist);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserEmail);
        result.ShouldHaveValidationErrorFor(x => x.CountryId);
    }
}