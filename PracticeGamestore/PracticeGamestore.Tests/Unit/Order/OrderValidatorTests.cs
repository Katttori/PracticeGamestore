using FluentValidation.TestHelper;
using NUnit.Framework;
using PracticeGamestore.Models.Order;
using PracticeGamestore.Validators;

namespace PracticeGamestore.Tests.Unit.Order;

[TestFixture]
public class OrderValidatorTests
{
    private OrderValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new OrderValidator();
    }

    [TestCase(0, TestName = "Total is zero")]
    [TestCase(-1, TestName = "Total is negative")]
    [TestCase(-99.99, TestName = "Total is a large negative")]
    public void WhenTotalIsZeroOrNegative_ShouldHaveError(decimal total)
    {
        // Arrange
        var order = TestData.Order.GenerateOrderRequestModel();
        order.Total = total;

        // Act
        var result = _validator.TestValidate(order);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Total);
    }

    [TestCase(0.01, TestName = "Total is minimal valid value")]
    [TestCase(100, TestName = "Total is typical value")]
    [TestCase(9999.99, TestName = "Total is high valid value")]
    public void WhenTotalIsPositive_ShouldNotHaveError(decimal total)
    {
        // Arrange
        var order = TestData.Order.GenerateOrderRequestModel();
        order.Total = total;

        // Act
        var result = _validator.TestValidate(order);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Total);
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
    public void WhenEmailIsInvalid_ShouldHaveError(string? email)
    {
        // Arrange
        var order = TestData.Order.GenerateOrderRequestModel();
        order.UserEmail = email!;

        // Act
        var result = _validator.TestValidate(order);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserEmail);
    }

    [TestCase("valid@example.com", TestName = "Valid email")]
    [TestCase("user.name+tag@sub.domain.com", TestName = "Complex valid email")]
    public void WhenEmailIsValid_ShouldNotHaveError(string email)
    {
        // Arrange
        var order = TestData.Order.GenerateOrderRequestModel();
        order.UserEmail = email;

        // Act
        var result = _validator.TestValidate(order);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.UserEmail);
    }

    [Test]
    public void WhenGameIdsIsNull_ShouldHaveError()
    {
        // Arrange
        var order = TestData.Order.GenerateOrderRequestModel();
        order.GameIds = null!;

        // Act
        var result = _validator.TestValidate(order);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GameIds);
    }

    [Test]
    public void WhenGameIdsIsEmpty_ShouldHaveError()
    {
        // Arrange
        var order = TestData.Order.GenerateOrderRequestModel();
        order.GameIds = [];

        // Act
        var result = _validator.TestValidate(order);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GameIds);
    }

    [Test]
    public void WhenGameIdsContainEmptyGuid_ShouldHaveCustomError()
    {
        // Arrange
        var order = TestData.Order.GenerateOrderRequestModel();
        order.GameIds = [Guid.NewGuid(), Guid.Empty];

        // Act
        var result = _validator.TestValidate(order);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GameIds)
            .WithErrorMessage("Game Ids does not contain corrects ids");
    }

    [Test]
    public void WhenGameIdsAreValid_ShouldNotHaveError()
    {
        // Arrange
        var order = TestData.Order.GenerateOrderRequestModel();
        order.GameIds = [Guid.NewGuid(), Guid.NewGuid()];

        // Act
        var result = _validator.TestValidate(order);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.GameIds);
    }

    [Test]
    public void WhenModelIsValid_ShouldNotHaveAnyErrors()
    {
        // Arrange
        var order = TestData.Order.GenerateOrderRequestModel();

        // Act
        var result = _validator.TestValidate(order);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void WhenModelHasMultipleInvalidFields_ShouldHaveMultipleErrors()
    {
        // Arrange
        var order = new OrderRequestModel
        {
            UserEmail = "invalid",
            Total = 0,
            GameIds = [Guid.Empty]
        };

        // Act
        var result = _validator.TestValidate(order);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Total);
        result.ShouldHaveValidationErrorFor(x => x.UserEmail);
        result.ShouldHaveValidationErrorFor(x => x.GameIds);
    }
}
