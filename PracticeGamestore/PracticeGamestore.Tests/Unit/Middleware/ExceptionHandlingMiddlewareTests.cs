using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PracticeGamestore.Middlewares;

namespace PracticeGamestore.Tests.Unit.Middleware;

public class ExceptionHandlingMiddlewareTests
{
    private Mock<ILogger<ExceptionHandlingMiddleware>> _loggerMock;
    private DefaultHttpContext _httpContext;
    private ExceptionHandlingMiddleware _middleware;

    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<ExceptionHandlingMiddleware>>();
        _httpContext = new DefaultHttpContext();
    }

    [Test]
    public async Task InvokeAsync_WhenExceptionIsThrown_Returns500AndLogs()
    {
        // Arrange
        _middleware = new ExceptionHandlingMiddleware(_ => throw new Exception("Test exception"), _loggerMock.Object);
        await using var responseBodyStream = new MemoryStream();
        _httpContext.Response.Body = responseBodyStream;
        
        // Act
        await _middleware.InvokeAsync(_httpContext);
        responseBodyStream.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();
        
        // Assert
        Assert.That(_httpContext.Response.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        
        var json = JsonSerializer.Deserialize<Dictionary<string, string>>(responseBody);
        Assert.That(json!["message"], Is.EqualTo("something happened?"));

        _loggerMock.Verify(logger =>
                logger.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Unhandled exception occured.")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ), Times.Once()
        );

    }
    
    [Test]
    public async Task InvokeAsync_WhenNoExceptionIsThrown_CallsNextMiddleware()
    {
        // Arrange
        _middleware = new ExceptionHandlingMiddleware(_ => Task.CompletedTask, _loggerMock.Object);
        
        // Act
        await _middleware.InvokeAsync(_httpContext);
        
        // Assert
        Assert.That(_httpContext.Response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    }
}