using FluentValidation.TestHelper;
using NUnit.Framework;
using PracticeGamestore.Validators;
using PracticeGamestore.Models.Order;

namespace PracticeGamestore.Tests.Unit.Validators;

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
    public void ShouldHaveError_WhenTotalIsZeroOrNegative(decimal total)
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
    public void ShouldNotHaveError_WhenTotalIsPositive(decimal total)
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
    [TestCase("invalid-email", TestName = "Email format is invalid")]
    public void ShouldHaveError_WhenEmailIsInvalid(string? email)
    {
        // Arrange
        var order = TestData.Order.GenerateOrderRequestModel();
        order.UserEmail = email;

        // Act
        var result = _validator.TestValidate(order);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserEmail);
    }

    [TestCase("valid@example.com", TestName = "Valid email")]
    [TestCase("user.name+tag@sub.domain.com", TestName = "Complex valid email")]
    public void ShouldNotHaveError_WhenEmailIsValid(string email)
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
    public void ShouldHaveError_WhenGameIdsIsNull()
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
    public void ShouldHaveError_WhenGameIdsIsEmpty()
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
    public void ShouldHaveCustomError_WhenGameIdsContainEmptyGuid()
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
    public void ShouldNotHaveError_WhenGameIdsAreValid()
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
    public void ShouldNotHaveAnyErrors_WhenModelIsValid()
    {
        // Arrange
        var order = TestData.Order.GenerateOrderRequestModel();

        // Act
        var result = _validator.TestValidate(order);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void ShouldHaveMultipleErrors_WhenModelHasMultipleInvalidFields()
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
