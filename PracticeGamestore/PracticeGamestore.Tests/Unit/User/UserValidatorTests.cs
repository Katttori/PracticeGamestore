using FluentValidation.TestHelper;
using NUnit.Framework;
using PracticeGamestore.Models.User;
using PracticeGamestore.Validators;

namespace PracticeGamestore.Tests.Unit.User;

[TestFixture]
public class UserValidatorTests
{
    private UserValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new UserValidator();
    }

    [TestCase(null, TestName = "UserName is null")]
    [TestCase("", TestName = "UserName is empty")]
    [TestCase(" ", TestName = "UserName is whitespace")]
    [TestCase("A", TestName = "UserName too short")]
    [TestCase("Invalid#Name", TestName = "UserName with invalid characters")]
    public void WhenUserNameIsInvalid_ShouldHaveError(string? userName)
    {
        // Arrange
        var model = GenerateValidModel();
        model.UserName = userName!;
        
        // Act
        var result = _validator.TestValidate(model);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserName);
    }

    [TestCase("John Doe", TestName = "Valid normal name")]
    [TestCase("O'Connor", TestName = "Valid name with apostrophe")]
    [TestCase("Jean-Luc Picard", TestName = "Valid name with dash")]
    public void WhenUserNameIsValid_ShouldNotHaveError(string userName)
    {
        // Arrange
        var model = GenerateValidModel();
        model.UserName = userName;

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.UserName);
    }

    [TestCase(null, TestName = "Email is null")]
    [TestCase("", TestName = "Email is empty")]
    [TestCase("plainaddress", TestName = "Email missing @")]
    [TestCase("@domain.com", TestName = "Email missing local part")]
    [TestCase("user@", TestName = "Email missing domain")]
    [TestCase("user@domain", TestName = "Email missing TLD")]
    [TestCase("user name@domain.com", TestName = "Email with space")]
    public void WhenEmailIsInvalid_ShouldHaveError(string? email)
    {
        // Arrange
        var model = GenerateValidModel();
        model.Email = email!;
        
        // Act
        var result = _validator.TestValidate(model);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [TestCase("valid@example.com", TestName = "Standard valid email")]
    [TestCase("user.name+alias@sub.domain.com", TestName = "Standard valid email with subdomain")]
    public void WhenEmailIsValid_ShouldNotHaveError(string email)
    {
        // Arrange
        var model = GenerateValidModel();
        model.Email = email;
        
        // Act
        var result = _validator.TestValidate(model);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }

    [TestCase(null, TestName = "Phone is null")]
    [TestCase("", TestName = "Phone is empty")]
    [TestCase("123", TestName = "Phone too short")]
    [TestCase("VeryVeryLongPhoneNumberThatIsTooLong", TestName = "Phone too long")]
    [TestCase("123abc", TestName = "Phone contains letters")]
    public void WhenPhoneNumberIsInvalid_ShouldHaveError(string? phone)
    {
        // Arrange
        var model = GenerateValidModel();
        model.PhoneNumber = phone!;
        
        // Act
        var result = _validator.TestValidate(model);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [TestCase("+1234567890", TestName = "Valid international format without prefix")]
    [TestCase("00380501234567", TestName = "Valid international format with prefix")]
    [TestCase("+1 234 567 890", TestName = "Valid international format with spaces")]
    public void WhenPhoneNumberIsValid_ShouldNotHaveError(string phone)
    {
        // Arrange
        var model = GenerateValidModel();
        model.PhoneNumber = phone;
        
        // Act
        var result = _validator.TestValidate(model);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [Test]
    public void WhenModelIsValid_ShouldNotHaveAnyErrors()
    {
        // Arrange
        var model = GenerateValidModel();

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void WhenModelHasMultipleInvalidFields_ShouldHaveMultipleErrors()
    {
        // Arrange
        var model = new UserRequestModel
        {
            UserName = "",
            Email = "invalidemail",
            PhoneNumber = "abc123",
            Password = "password",
            Role = "User",
            CountryId = Guid.NewGuid(),
            BirthDate = DateTime.UtcNow.AddYears(-20)
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserName);
        result.ShouldHaveValidationErrorFor(x => x.Email);
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }

    private UserRequestModel GenerateValidModel()
    {
        return new UserRequestModel
        {
            UserName = "John Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "+1234567890",
            Password = "StrongPassword123!",
            Role = "User",
            CountryId = Guid.NewGuid(),
            BirthDate = DateTime.UtcNow.AddYears(-20)
        };
    }
}
