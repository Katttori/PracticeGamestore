using FluentValidation.TestHelper;
using NUnit.Framework;
using PracticeGamestore.Business.Constants;
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
    public void WhenIbanIsValid_ShouldNotHaveError()
    {
        // Arrange
        var model = TestData.Payment.GeneratePaymentRequestModel(iban: true);

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
        var model = TestData.Payment.GeneratePaymentRequestModel(iban: true);
        model.Iban = new IbanModel { Iban = iban! };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Iban!.Iban);
    }

    [TestCase("123", "12/30", "123", TestName = "Card number too short")]
    [TestCase("4111111111111111", "01/20", "123", TestName = "Card expired")]
    [TestCase("4111111111111111", "12/30", "1", TestName = "CVC too short")]
    public void WhenCardFieldsAreInvalid_ShouldHaveError(string number, string expiration, string cvc)
    {
        // Arrange
        var model = TestData.Payment.GeneratePaymentRequestModel(creditCard: true);
        model.CreditCard!.Number = number;
        model.CreditCard.ExpirationDate = expiration;
        model.CreditCard.Cvc = cvc;

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveAnyValidationError();
    }

    [Test]
    public void WhenCardIsValid_ShouldNotHaveError()
    {
        // Arrange
        var model = TestData.Payment.GeneratePaymentRequestModel(creditCard: true);

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
        var model = TestData.Payment.GeneratePaymentRequestModel(ibox: true);
        model.Ibox = new IboxModel { TransactionId = Guid.Empty };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Ibox!.TransactionId).WithErrorMessage(ErrorMessages.InvalidIbox);
    }

    [Test]
    public void WhenIboxIsValid_ShouldNotHaveError()
    {
        // Arrange
        var model = TestData.Payment.GeneratePaymentRequestModel(ibox: true);

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Ibox);
    }
    
    [Test]
    public void WhenNoPaymentMethodsProvided_ShouldHaveError()
    {
        // Arrange
        var model = TestData.Payment.GeneratePaymentRequestModel();

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x);
    }
    
    [Test]
    public void WhenCardExpirationDateIsInvalid_ShouldHaveError()
    {
        // Arrange
        var model = TestData.Payment.GeneratePaymentRequestModel(creditCard: true);
        model.CreditCard!.ExpirationDate = "12/21";

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CreditCard!.ExpirationDate)
            .WithErrorMessage(ErrorMessages.InvalidExpirationDate);
    }
}