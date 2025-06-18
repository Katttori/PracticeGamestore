using FluentValidation.TestHelper;
using NUnit.Framework;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Models.Auth;
using PracticeGamestore.Validators;

namespace PracticeGamestore.Tests.Unit.Auth;

[TestFixture]
public class LoginRequestValidatorTests
{
    private LoginRequestValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new LoginRequestValidator();
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
    public void WhenUserEmailIsInvalid_ShouldHaveError(string? email)
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Email = email!,
            Password = "ValidPassword123!"
        };
        
        // Act
        var result = _validator.TestValidate(loginRequest);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }
    
    [Test]
    public void WhenUserEmailIsTooLong_ShouldHaveError()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Email = TestData.StringConstants.LongerThatShortMaximum,
            Password = "ValidPassword123!"
        };
        
        // Act
        var result = _validator.TestValidate(loginRequest);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [TestCase("test@example.com", TestName = "Standard valid email")]
    [TestCase("user.name@domain.co.uk", TestName = "Email with subdomain")]
    [TestCase("user+tag@example.org", TestName = "Email with plus sign")]
    [TestCase("user_name@example-domain.com", TestName = "Email with underscore and hyphen")]
    [TestCase("firstname.lastname@company.travel", TestName = "Email with long TLD")]
    [TestCase("a@b.co", TestName = "Minimal valid email")]
    public void WhenUserEmailIsValid_ShouldNotHaveError(string email)
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Email = email,
            Password = "ValidPassword123!"
        };
        
        // Act
        var result = _validator.TestValidate(loginRequest);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }

    [TestCase(null, TestName = "Password is null")]
    [TestCase("", TestName = "Password is empty")]
    [TestCase(" ", TestName = "Password is whitespace")]
    public void WhenPasswordIsInvalid_ShouldHaveError(string? password)
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Email = "test@example.com",
            Password = password!
        };
        
        // Act
        var result = _validator.TestValidate(loginRequest);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage(ErrorMessages.PasswordRequired);
    }
    
    [Test]
    public void WhenPasswordIsTooShort_ShouldHaveError()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Email = "test@example.com",
            Password = "12345"
        };
        
        // Act
        var result = _validator.TestValidate(loginRequest);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Test]
    public void WhenPasswordIsTooLong_ShouldHaveError()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Email = "test@example.com",
            Password = TestData.StringConstants.LongerThatShortMaximum
        };
        
        // Act
        var result = _validator.TestValidate(loginRequest);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Test]
    public void WhenModelIsValid_ShouldNotHaveAnyErrors()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Email = "test@example.com",
            Password = "ValidPassword123!"
        };

        // Act
        var result = _validator.TestValidate(loginRequest);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void WhenModelHasMultipleInvalidFields_ShouldHaveMultipleErrors()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Email = "invalidemail",
            Password = ""
        };

        // Act
        var result = _validator.TestValidate(loginRequest);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
}