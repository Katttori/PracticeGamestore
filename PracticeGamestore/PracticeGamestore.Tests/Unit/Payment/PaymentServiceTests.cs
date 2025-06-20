using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.DataTransferObjects.Payment;
using PracticeGamestore.Business.Enums;
using PracticeGamestore.Business.Options;
using PracticeGamestore.Business.Services.Payment;
using RichardSzalay.MockHttp;

namespace PracticeGamestore.Tests.Unit.Payment;

[TestFixture]
public class PaymentServiceTests
{
    private HttpClient httpClient;
    private Mock<ILogger<PaymentService>> logger;
    private IPaymentService paymentService;
    private MockHttpMessageHandler mockHttp;

    [SetUp]
    public void SetUp()
    {
        mockHttp = new MockHttpMessageHandler();
        httpClient = mockHttp.ToHttpClient();
        httpClient.BaseAddress = new Uri("https://localhost:5001");
        var testPaymentOptions = new Mock<IOptions<PaymentOptions>>();
        testPaymentOptions.Setup(x => x.Value).Returns(TestData.Payment.PaymentOptions);
        logger = new Mock<ILogger<PaymentService>>();
        paymentService = new PaymentService(httpClient, testPaymentOptions.Object, logger.Object);
    }

    
    
    private async Task<bool> ExecutePaymentMethod(PaymentMethod paymentMethod, object paymentDto)
    {
        return paymentMethod switch
        {
            PaymentMethod.Iban => await paymentService.PayIbanAsync((IbanDto)paymentDto),
            PaymentMethod.CreditCard => await paymentService.PayCardAsync((CreditCardDto)paymentDto),
            PaymentMethod.Ibox => await paymentService.PayIboxAsync((IboxDto)paymentDto),
            _ => throw new ArgumentException($"Unknown payment method: {paymentMethod}")
        };
    }

    private void VerifyLoggerWasCalledWithLevelAndMessage(LogLevel logLevel, string message)
    {
        logger.Verify(
            x => x.Log(
                logLevel,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(message)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [TestCaseSource(typeof(TestData.Payment), nameof(TestData.Payment.PaymentServiceTestData))]
    public async Task PayAsync_WhenPaymentWasSuccessful_ShouldReturnTrue(PaymentMethod method, object paymentDto)
    {
        // Arrange
        var endpoint = TestData.Payment.PaymentOptions.Endpoints.GetValueOrDefault(method);
        mockHttp.When(HttpMethod.Post, endpoint!)
            .Respond(HttpStatusCode.OK);
        
        // Act
        var result = await ExecutePaymentMethod(method, paymentDto);
        
        // Assert
        Assert.That(result, Is.True);
        VerifyLoggerWasCalledWithLevelAndMessage(LogLevel.Information, $"Successfully processed {method} payment");
    }
    
    [TestCaseSource(typeof(TestData.Payment), nameof(TestData.Payment.PaymentServiceTestData))]
    public async Task PayAsync_WhenPaymentFailed_ShouldReturnFalse(PaymentMethod method, object paymentDto)
    {
        // Arrange
        var endpoint = TestData.Payment.PaymentOptions.Endpoints.GetValueOrDefault(method);
        mockHttp.When(HttpMethod.Post, endpoint!)
            .Respond(HttpStatusCode.BadRequest);
        
        // Act
        var result = await ExecutePaymentMethod(method, paymentDto);
        
        // Assert
        Assert.That(result, Is.False);
        VerifyLoggerWasCalledWithLevelAndMessage(LogLevel.Error, $"{method} payment failed with status {HttpStatusCode.BadRequest}");
    }

    [TestCaseSource(typeof(TestData.Payment), nameof(TestData.Payment.PaymentServiceTestData))]
    public async Task PayAsync_WhenNetworkErrorOccurs_ShouldReturnFalse(PaymentMethod method, object paymentDto)
    {
        // Arrange
        var endpoint = TestData.Payment.PaymentOptions.Endpoints.GetValueOrDefault(method);
        mockHttp.When(HttpMethod.Post, endpoint!)
            .Throw(new HttpRequestException("Network error!"));
        
        // Act
        var result = await ExecutePaymentMethod(method, paymentDto);

        // Assert
        Assert.That(result, Is.False); 
        VerifyLoggerWasCalledWithLevelAndMessage(LogLevel.Error, $"{method} payment failed after all retries");
    }
    
    [TestCaseSource(typeof(TestData.Payment), nameof(TestData.Payment.PaymentServiceTestData))]
    public async Task PayAsync_WhenTimeoutOccurs_ShouldReturnFalse(PaymentMethod method, object paymentDto)
    {
        // Arrange
        var endpoint = TestData.Payment.PaymentOptions.Endpoints.GetValueOrDefault(method);
        mockHttp.When(HttpMethod.Post, endpoint!)
            .Throw(new TaskCanceledException("Request timeout"));

        // Act
        var result = await ExecutePaymentMethod(method, paymentDto);

        // Assert
        Assert.That(result, Is.False);
        VerifyLoggerWasCalledWithLevelAndMessage(LogLevel.Error, $"{method} payment timed out");
    }

    [TearDown]
    public void TearDown()
    {
        httpClient.Dispose();
        mockHttp.Dispose();
    }
}