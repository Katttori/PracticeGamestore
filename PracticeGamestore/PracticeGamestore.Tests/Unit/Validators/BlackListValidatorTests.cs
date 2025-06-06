using FluentValidation.TestHelper;
using NUnit.Framework;
using PracticeGamestore.Models.Blacklist;
using PracticeGamestore.Validators;

namespace PracticeGamestore.Tests.Unit.Validators;

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
    [TestCase("invalid-email", TestName = "Email is invalid format")]
    [TestCase("@domain.com", TestName = "Email missing local part")]
    public void ShouldHaveError_WhenUserEmailIsInvalid(string? email)
    {
        // Arrange
        var blacklist = new BlacklistRequestModel { UserEmail = email, CountryId = Guid.NewGuid() };
        
        // Act
        var result = _validator.TestValidate(blacklist);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserEmail);
    }

    [TestCase("test@example.com", TestName = "Valid email")]
    [TestCase("user.name@domain.co.uk", TestName = "Email with subdomain")]
    public void ShouldNotHaveError_WhenUserEmailIsValid(string email)
    {
        // Arrange
        var blacklist = new BlacklistRequestModel { UserEmail = email, CountryId = Guid.NewGuid() };
        
        // Act
        var result = _validator.TestValidate(blacklist);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.UserEmail);
    }

    [Test]
    public void ShouldHaveError_WhenCountryIdIsEmpty()
    {
        // Arrange
        var blacklist = new BlacklistRequestModel { UserEmail = "test@example.com", CountryId = Guid.Empty };
        
        // Act
        var result = _validator.TestValidate(blacklist);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CountryId);
    }

    [Test]
    public void ShouldNotHaveError_WhenCountryIdIsValid()
    {
        // Arrange
        var blacklist = new BlacklistRequestModel { UserEmail = "test@example.com", CountryId = Guid.NewGuid() };
        
        // Act
        var result = _validator.TestValidate(blacklist);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.CountryId);
    }
}