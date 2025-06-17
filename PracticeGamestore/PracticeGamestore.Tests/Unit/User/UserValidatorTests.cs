using FluentValidation.TestHelper;
using NUnit.Framework;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Business.Enums;
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
        var model = TestData.User.GenerateUserRequestModel();
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
        var model = TestData.User.GenerateUserRequestModel();
        model.UserName = userName;

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.UserName);
    }
    
    [Test]
    public void WhenUserNameIsTooLong_ShouldHaveError()
    {
        // Arrange
        var model = TestData.User.GenerateUserRequestModel();
        model.UserName = TestData.StringConstants.LongerThatShortMaximum;
        
        // Act
        var result = _validator.TestValidate(model);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserName);
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
        var model = TestData.User.GenerateUserRequestModel();
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
        var model = TestData.User.GenerateUserRequestModel();
        model.Email = email;
        
        // Act
        var result = _validator.TestValidate(model);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }
    
    [Test]
    public void WhenEmailIsTooLong_ShouldHaveError()
    {
        // Arrange
        var model = TestData.User.GenerateUserRequestModel();
        model.Email = TestData.StringConstants.LongerThatShortMaximum;
        
        // Act
        var result = _validator.TestValidate(model);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [TestCase(null, TestName = "Phone is null")]
    [TestCase("", TestName = "Phone is empty")]
    [TestCase("123", TestName = "Phone too short")]
    [TestCase("VeryVeryLongPhoneNumberThatIsTooLong", TestName = "Phone too long")]
    [TestCase("123abc", TestName = "Phone contains letters")]
    public void WhenPhoneNumberIsInvalid_ShouldHaveError(string? phone)
    {
        // Arrange
        var model = TestData.User.GenerateUserRequestModel();
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
        var model = TestData.User.GenerateUserRequestModel();
        model.PhoneNumber = phone;
        
        // Act
        var result = _validator.TestValidate(model);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.PhoneNumber);
    }
    
    [Test]
    public void WhenPhoneNumberIsTooLong_ShouldHaveError()
    {
        // Arrange
        var model = TestData.User.GenerateUserRequestModel();
        // Create a phone number longer than max length but still matching the regex
        var longPhoneNumber = new string('1', ValidationConstants.PhoneNumber.MaxLength + 1);
        model.PhoneNumber = longPhoneNumber;
        
        // Act
        var result = _validator.TestValidate(model);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }
    
    [TestCase(null, TestName = "Password is null")]
    [TestCase("", TestName = "Password is empty")]
    [TestCase("1234567", TestName = "Password too short")]
    [TestCase("password", TestName = "Password without uppercase")]
    [TestCase("PASSWORD", TestName = "Password without lowercase")]
    [TestCase("Password", TestName = "Password without digits")]
    [TestCase("Password123", TestName = "Password without special characters")]
    [TestCase("pass", TestName = "Password too short and missing requirements")]
    public void WhenPasswordIsInvalid_ShouldHaveError(string? password)
    {
        // Arrange
        var model = TestData.User.GenerateUserRequestModel();
        model.Password = password!;
        
        // Act
        var result = _validator.TestValidate(model);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
    
    [TestCase("weakpass", TestName = "Weak password without uppercase, digits and special chars")]
    [TestCase("Password1", TestName = "Password missing special characters")]
    [TestCase("password123!", TestName = "Password missing uppercase")]
    [TestCase("PASSWORD123!", TestName = "Password missing lowercase")]
    [TestCase("Password!", TestName = "Password missing digits")]
    public void WhenPasswordDoesNotMeetComplexityRequirements_ShouldHaveCustomError(string password)
    {
        // Arrange
        var model = TestData.User.GenerateUserRequestModel();
        model.Password = password;
        
        // Act
        var result = _validator.TestValidate(model);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage(ErrorMessages.InsecurePassword);
    }
    
    [Test]
    public void WhenPasswordIsTooLong_ShouldHaveError()
    {
        // Arrange
        var model = TestData.User.GenerateUserRequestModel();
        
        var longPassword = new string('A', ValidationConstants.Password.MaxLength - 10) + "1a!";
        longPassword += new string('B', 15);
        model.Password = longPassword;
        
        // Act
        var result = _validator.TestValidate(model);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
    
    [TestCase("Password123!", TestName = "Valid complex password")]
    [TestCase("MySecure@Pass1", TestName = "Valid password with @")]
    [TestCase("ComplexP@ssw0rd", TestName = "Valid password with mixed requirements")]
    [TestCase("StrongPassword123#", TestName = "Valid long password")]
    [TestCase("T3st!ng$", TestName = "Valid minimal complexity password")]
    [TestCase("MyP@ssw0rd2024", TestName = "Valid year-based password")]
    [TestCase("Secure#123Pass", TestName = "Valid password with hash")]
    public void WhenPasswordIsValid_ShouldNotHaveError(string password)
    {
        // Arrange
        var model = TestData.User.GenerateUserRequestModel();
        model.Password = password;
        
        // Act
        var result = _validator.TestValidate(model);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }
    
    [Test]
    public void WhenCountryIdIsEmpty_ShouldHaveError()
    {
        // Arrange
        var model = TestData.User.GenerateUserRequestModel();
        model.CountryId = Guid.Empty;
        
        // Act
        var result = _validator.TestValidate(model);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CountryId);
    }

    [Test]
    public void WhenCountryIdIsValid_ShouldNotHaveError()
    {
        // Arrange
        var model = TestData.User.GenerateUserRequestModel();
        model.CountryId = Guid.NewGuid();
        
        // Act
        var result = _validator.TestValidate(model);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.CountryId);
    }
    
    [Test]
    public void WhenBirthDateIsInFuture_ShouldHaveError()
    {
        // Arrange
        var model = TestData.User.GenerateUserRequestModel();
        model.BirthDate = DateTime.UtcNow.AddDays(1);
        
        // Act
        var result = _validator.TestValidate(model);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.BirthDate);
    }

    [Test]
    public void WhenBirthDateIsToday_ShouldHaveError()
    {
        // Arrange
        var model = TestData.User.GenerateUserRequestModel();
        model.BirthDate = DateTime.UtcNow;
        
        // Act
        var result = _validator.TestValidate(model);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.BirthDate);
    }

    [TestCase(-1, TestName = "Birth date yesterday")]
    [TestCase(-365, TestName = "Birth date one year ago")]
    [TestCase(-7300, TestName = "Birth date 20 years ago")]
    [TestCase(-36500, TestName = "Birth date 100 years ago")]
    public void WhenBirthDateIsInPast_ShouldNotHaveError(int daysInPast)
    {
        // Arrange
        var model = TestData.User.GenerateUserRequestModel();
        model.BirthDate = DateTime.UtcNow.AddDays(daysInPast);
        
        // Act
        var result = _validator.TestValidate(model);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.BirthDate);
    }
    
    [TestCase(UserRole.Admin, TestName = "Valid Admin role")]
    [TestCase(UserRole.User, TestName = "Valid User role")]
    [TestCase(UserRole.Manager, TestName = "Valid Manager role")]
    public void WhenRoleIsValid_ShouldNotHaveError(UserRole role)
    {
        // Arrange
        var model = TestData.User.GenerateUserRequestModel();
        model.Role = role;
        
        // Act
        var result = _validator.TestValidate(model);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Role);
    }

    [Test]
    public void WhenRoleIsInvalidEnum_ShouldHaveCustomError()
    {
        // Arrange
        var model = TestData.User.GenerateUserRequestModel();
        model.Role = (UserRole)999;
        
        // Act
        var result = _validator.TestValidate(model);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Role)
            .WithErrorMessage(ErrorMessages.InvalidRole);
    }

    [Test]
    public void WhenModelIsValid_ShouldNotHaveAnyErrors()
    {
        // Arrange
        var model = TestData.User.GenerateUserRequestModel();

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
            Password = "weak",
            Role = (UserRole)999,
            CountryId = Guid.Empty,
            BirthDate = DateTime.UtcNow.AddYears(1)
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserName);
        result.ShouldHaveValidationErrorFor(x => x.Email);
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
        result.ShouldHaveValidationErrorFor(x => x.Password);
        result.ShouldHaveValidationErrorFor(x => x.CountryId);
        result.ShouldHaveValidationErrorFor(x => x.BirthDate);
        result.ShouldHaveValidationErrorFor(x => x.Role)
            .WithErrorMessage(ErrorMessages.InvalidRole);
    }
}
