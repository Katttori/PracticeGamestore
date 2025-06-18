using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Middlewares;

namespace PracticeGamestore.Tests.Unit.Middleware;

[TestFixture]
public class ExceptionHandlingMiddlewareTests
{
    private Mock<ILogger<ExceptionHandlingMiddleware>> _loggerMock;
    private DefaultHttpContext _httpContext;

    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<ExceptionHandlingMiddleware>>();
        _httpContext = new DefaultHttpContext();
    }

    [Test]
    public async Task InvokeAsync_WhenExceptionIsThrown_ShouldReturn500AndLogs()
    {
        // Act
        var (status, message) = await InvokeAndReadAsync(new Exception("Test exception"));
        
        // Assert
        Assert.That(status, Is.EqualTo(StatusCodes.Status500InternalServerError));
        Assert.That(message, Is.EqualTo("something happened?"));
        VerifyLogCalled();
    }
    
    [Test]
    public async Task InvokeAsync_WhenArgumentExceptionIsThrown_ShouldReturn400AndLogs()
    {
        // Act
        var (status, message) = await InvokeAndReadAsync(new ArgumentException("Incorrect argument"));
        
        // Assert
        Assert.That(status, Is.EqualTo(StatusCodes.Status400BadRequest));
        Assert.That(message, Is.EqualTo("Incorrect argument"));
        VerifyLogCalled();
    }
    
    [Test]
    public async Task InvokeAsync_WhenUnauthorizedAccessExceptionIsThrown_ShouldReturn403AndLogs()
    {
        // Act
        var (status, message) = await InvokeAndReadAsync(new UnauthorizedAccessException(ErrorMessages.BlacklistedUser));
        
        // Assert
        Assert.That(status, Is.EqualTo(StatusCodes.Status403Forbidden));
        Assert.That(message, Is.EqualTo(ErrorMessages.BlacklistedUser));
        VerifyLogCalled();
    }
    
    [Test]
    public async Task InvokeAsync_WhenNoExceptionIsThrown_ShouldCallNextMiddleware()
    {
        // Arrange
        var middleware = new ExceptionHandlingMiddleware(_ => Task.CompletedTask, _loggerMock.Object);
        
        // Act
        await middleware.InvokeAsync(_httpContext);
        
        // Assert
        Assert.That(_httpContext.Response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    }
    
    private async Task<(int statusCode, string message)> InvokeAndReadAsync(Exception exception)
    {
        var middleware = new ExceptionHandlingMiddleware(_ => throw exception, _loggerMock.Object);
        await using var responseBodyStream = new MemoryStream();
        _httpContext.Response.Body = responseBodyStream;

        await middleware.InvokeAsync(_httpContext);
    
        responseBodyStream.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();
        var json = JsonSerializer.Deserialize<Dictionary<string, string>>(responseBody);

        return (_httpContext.Response.StatusCode, json?["message"] ?? "");
    }

    private void VerifyLogCalled()
    {
        _loggerMock.Verify(logger =>
            logger.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Unhandled exception occured.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ), Times.Once);
    }
}