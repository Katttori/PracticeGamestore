using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PracticeGamestore.Filters;

namespace PracticeGamestore.Tests.Unit.Filters;

[TestFixture]
public class RequestModelValidationFilterTests
{
    private Mock<ILogger<RequestModelValidationFilter>> _logger;
    private ActionExecutingContext _context;
    private RequestModelValidationFilter _filter;

    [SetUp]
    public void SetUp()
    {
        _logger = new Mock<ILogger<RequestModelValidationFilter>>();
        _context = new ActionExecutingContext(
            new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor()),
            new List<IFilterMetadata>(),
            new Dictionary<string, object?>(),
            controller: new object());
        _filter = new RequestModelValidationFilter(_logger.Object);
    }
    

[Test]
    public async Task OnActionExecuting_ShouldInterceptAndReturnBadRequestWithErrorsWhenRequestModelIsInvalidAndLogIt()
    {
        // Arrange
        _context.ModelState.AddModelError("error", "message");

        // Act
        _filter.OnActionExecuting(_context);

        // Assert
        Assert.That(_context.Result, Is.InstanceOf<BadRequestObjectResult>());
        var badRequestResult = _context.Result as BadRequestObjectResult;
        Assert.That(badRequestResult, Is.Not.Null);
        Assert.That(badRequestResult!.Value, Is.InstanceOf<SerializableError>());
        var serializableError = badRequestResult.Value as SerializableError;
        Assert.That(serializableError!.ContainsKey("error"), Is.True);
        var errorValues = serializableError["error"] as string[];
        Assert.That(errorValues, Is.Not.Null);
        Assert.That(errorValues, Contains.Item("message"));

        _logger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Model validation failed")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Test]
    public async Task OnActionExecuting_ShouldContinueWhenRequestModelIsValid()
    {
        // Arrange
        
        // Act
        _filter.OnActionExecuting(_context);
        
        // Assert
        Assert.That(_context.Result, Is.Null); 
        
        _logger.Verify(
            x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Never);
    }
}