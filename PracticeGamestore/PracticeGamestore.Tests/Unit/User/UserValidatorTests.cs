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
    public void ShouldHaveError_WhenUserNameIsInvalid(string? userName)
    {
        var model = GenerateValidModel();
        model.UserName = userName!;

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.UserName);
    }

    [TestCase("John Doe", TestName = "Valid normal name")]
    [TestCase("O'Connor", TestName = "Valid name with apostrophe")]
    [TestCase("Jean-Luc Picard", TestName = "Valid name with dash")]
    public void ShouldNotHaveError_WhenUserNameIsValid(string userName)
    {
        var model = GenerateValidModel();
        model.UserName = userName;

        var result = _validator.TestValidate(model);

        result.ShouldNotHaveValidationErrorFor(x => x.UserName);
    }

    [TestCase(null, TestName = "Email is null")]
    [TestCase("", TestName = "Email is empty")]
    [TestCase("plainaddress", TestName = "Email missing @")]
    [TestCase("@domain.com", TestName = "Email missing local part")]
    [TestCase("user@", TestName = "Email missing domain")]
    [TestCase("user@domain", TestName = "Email missing TLD")]
    [TestCase("user name@domain.com", TestName = "Email with space")]
    public void ShouldHaveError_WhenEmailIsInvalid(string? email)
    {
        var model = GenerateValidModel();
        model.Email = email!;

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [TestCase("valid@example.com")]
    [TestCase("user.name+alias@sub.domain.com")]
    public void ShouldNotHaveError_WhenEmailIsValid(string email)
    {
        var model = GenerateValidModel();
        model.Email = email;

        var result = _validator.TestValidate(model);

        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }

    [TestCase(null, TestName = "Phone is null")]
    [TestCase("", TestName = "Phone is empty")]
    [TestCase("123", TestName = "Phone too short")]
    [TestCase("VeryVeryLongPhoneNumberThatIsTooLong", TestName = "Phone too long")]
    [TestCase("123abc", TestName = "Phone contains letters")]
    public void ShouldHaveError_WhenPhoneNumberIsInvalid(string? phone)
    {
        var model = GenerateValidModel();
        model.PhoneNumber = phone!;

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [TestCase("+1234567890")]
    [TestCase("00380501234567")]
    [TestCase("+1 234 567 890")]
    public void ShouldNotHaveError_WhenPhoneNumberIsValid(string phone)
    {
        var model = GenerateValidModel();
        model.PhoneNumber = phone;

        var result = _validator.TestValidate(model);

        result.ShouldNotHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [Test]
    public void ShouldNotHaveAnyErrors_WhenModelIsValid()
    {
        var model = GenerateValidModel();

        var result = _validator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void ShouldHaveMultipleErrors_WhenModelHasMultipleInvalidFields()
    {
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

        var result = _validator.TestValidate(model);

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
