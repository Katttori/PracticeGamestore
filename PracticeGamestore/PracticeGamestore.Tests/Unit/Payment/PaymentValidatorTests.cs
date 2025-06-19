using FluentValidation.TestHelper;
using NUnit.Framework;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Enums;
using PracticeGamestore.Models.Payment;
using PracticeGamestore.Validators;

namespace PracticeGamestore.Tests.Unit.Payment;

[TestFixture]
public class PaymentValidatorTests
{
    private PaymentValidator _validator;
    
    [SetUp]
    public void SetUp()
    {
        _validator = new PaymentValidator();
    }

    [Test]
    public void WhenPaymentTypeIsInvalid_ShouldHaveError()
    {
        // Arrange
        var model = new PaymentRequestModel { Type = (PaymentMethod)999 };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Type).WithErrorMessage(ErrorMessages.InvalidPaymentType);
    }

    [Test]
    public void WhenIbanIsValid_ShouldNotHaveError()
    {
        // Arrange
        var model = TestData.Payment.GenerateIbanPaymentRequestModel();

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Iban);
    }

    [TestCase(null, TestName = "IBAN is null")]
    [TestCase("", TestName = "IBAN is empty")]
    [TestCase("UA12", TestName = "IBAN too short")]
    [TestCase("UA123INVALID45678901234567890", TestName = "IBAN invalid format")]
    public void WhenIbanIsInvalid_ShouldHaveError(string? iban)
    {
        // Arrange
        var model = TestData.Payment.GenerateIbanPaymentRequestModel();
        model.Iban = iban;

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Iban);
    }

    [TestCase("123", "12/30", "123", TestName = "Card number too short")]
    [TestCase("4111111111111111", "01/20", "123", TestName = "Card expired")]
    [TestCase("4111111111111111", "12/30", "1", TestName = "CVC too short")]
    public void WhenCardFieldsAreInvalid_ShouldHaveError(string number, string expiration, string cvc)
    {
        // Arrange
        var model = TestData.Payment.GenerateCardPaymentRequestModel();
        model.Card!.Number = number;
        model.Card.ExpirationDate = expiration;
        model.Card.Cvc = cvc;

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveAnyValidationError();
    }

    [Test]
    public void WhenCardIsValid_ShouldNotHaveError()
    {
        // Arrange
        var model = TestData.Payment.GenerateCardPaymentRequestModel();

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor("Card.Number");
        result.ShouldNotHaveValidationErrorFor("Card.ExpirationDate");
        result.ShouldNotHaveValidationErrorFor("Card.Cvc");
    }

    [Test]
    public void WhenIboxIsEmpty_ShouldHaveError()
    {
        // Arrange
        var model = TestData.Payment.GenerateIboxPaymentRequestModel();
        model.Ibox = Guid.Empty;

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Ibox).WithErrorMessage(ErrorMessages.InvalidIbox);
    }

    [Test]
    public void WhenIboxIsValid_ShouldNotHaveError()
    {
        // Arrange
        var model = TestData.Payment.GenerateIboxPaymentRequestModel();

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Ibox);
    }
}