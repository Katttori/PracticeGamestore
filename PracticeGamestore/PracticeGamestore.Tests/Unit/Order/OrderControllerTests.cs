using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.DataTransferObjects.Order;
using PracticeGamestore.Business.Enums;
using PracticeGamestore.Business.Services.HeaderHandle;
using PracticeGamestore.Business.Services.Order;
using PracticeGamestore.Business.Services.Payment;
using PracticeGamestore.Controllers;
using PracticeGamestore.DataAccess.Enums;
using PracticeGamestore.Models.Order;
using PracticeGamestore.Models.Payment;

namespace PracticeGamestore.Tests.Unit.Order;

[TestFixture]
public class OrderControllerTests
{
    private Mock<IOrderService> _orderServiceMock;
    private Mock<IPaymentService> _paymentServiceMock;
    private Mock<IHeaderHandleService> _headerHandleServiceMock;
    private Mock<ILogger<OrderController>> _loggerMock;
    private OrderController _orderController;
    
    private readonly List<OrderResponseDto> _orderDtos = TestData.Order.GenerateOrderResponseDtos();
    private readonly OrderRequestModel _orderRequestModel = TestData.Order.GenerateOrderRequestModel();
    
    private const string CountryHeader = "Ukraine";
    private const string UserEmailHeader = "test@gmail.com";

    [SetUp]
    public void Setup()
    {
        _orderServiceMock = new Mock<IOrderService>();
        _paymentServiceMock = new Mock<IPaymentService>();
        _headerHandleServiceMock = new Mock<IHeaderHandleService>();
        _loggerMock = new Mock<ILogger<OrderController>>();
        _orderController = new OrderController(_orderServiceMock.Object, _paymentServiceMock.Object,
            _headerHandleServiceMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task GetAll_ReturnsOkWithOrders()
    {
        // Arrange
        _orderServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(_orderDtos);
        
        // Act
        var result = await _orderController.GetAll();
        
        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        var responseModels = 
            (okResult?.Value as IEnumerable<OrderResponseModel> ?? Array.Empty<OrderResponseModel>()).ToList();
        for (var i = 0; i < responseModels.Count; i++)
        {
            Assert.That(responseModels[i].Id, Is.EqualTo(_orderDtos[i].Id));
            Assert.That(responseModels[i].Status, Is.EqualTo(_orderDtos[i].Status.ToString()));
            Assert.That(responseModels[i].UserEmail, Is.EqualTo(_orderDtos[i].UserEmail));
            Assert.That(responseModels[i].Total, Is.EqualTo(_orderDtos[i].Total));
            Assert.That(responseModels[i].Games.Count, Is.EqualTo(_orderDtos[i].Games.Count));
        }
    }
    
    [Test]
    public async Task GetById_WhenOrderIsNull_ReturnsNotFound()
    {
        // Arrange
        _orderServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as OrderResponseDto);
        
        // Act
        var result = await _orderController.GetById(Guid.NewGuid());
        
        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }
    
    [Test]
    public async Task GetById_WhenOrderFound_ReturnsOkWithOrder()
    {
        // Arrange
        _orderServiceMock.Setup(x => x.GetByIdAsync(_orderDtos[0].Id!.Value)).ReturnsAsync(_orderDtos[0]);
        
        // Act
        var result = await _orderController.GetById(_orderDtos[0].Id!.Value);
        
        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var receivedOrder = (result as OkObjectResult)?.Value as OrderResponseModel;
        Assert.That(receivedOrder, Is.Not.Null);
        Assert.That(receivedOrder, Has.Property("Id").EqualTo(_orderDtos[0].Id));
        Assert.That(receivedOrder, Has.Property("Status").EqualTo(_orderDtos[0].Status.ToString()));
        Assert.That(receivedOrder, Has.Property("UserEmail").EqualTo(_orderDtos[0].UserEmail));
        Assert.That(receivedOrder, Has.Property("Total").EqualTo(_orderDtos[0].Total));
        Assert.That(receivedOrder, Has.Property("Games").Count.EqualTo(_orderDtos[0].Games.Count));
    }
    
    [Test]
    public async Task Create_WhenOperationFailed_ReturnsBadRequest()
    {
        // Arrange
        _orderServiceMock
            .Setup(x => x.CreateAsync(It.IsAny<OrderRequestDto>()))
            .ReturnsAsync(null as Guid?);
        
        // Act
        var result = await _orderController.Create(CountryHeader, UserEmailHeader, _orderRequestModel);
        
        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }
    
    [Test]
    public async Task Create_WhenOperationSuccessful_ReturnsCreatedWithId()
    {
        // Arrange
        var newId = Guid.NewGuid();
        
        _orderServiceMock.Setup(x => x.CreateAsync(It.IsAny<OrderRequestDto>())).ReturnsAsync(newId);
        
        // Act
        var result = await _orderController.Create(CountryHeader, UserEmailHeader, _orderRequestModel);
        
        // Assert
        Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
        var createdResult = result as CreatedAtActionResult;
        Assert.That(createdResult?.Value, Is.EqualTo(newId));
    }
    
    [Test]
    public async Task Update_WhenOperationSuccessful_ReturnsNoContent()
    {
        // Arrange
        _orderServiceMock
            .Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<OrderRequestDto>()))
            .ReturnsAsync(true);
        
        // Act
        var result = await _orderController.Update(Guid.NewGuid(), _orderRequestModel);
        
        // Assert
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }
    
    [Test]
    public async Task Update_WhenOperationFailed_ReturnsBadRequest()
    {
        // Arrange
        _orderServiceMock
            .Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<OrderRequestDto>()))
            .ReturnsAsync(false);
        
        // Act
        var result = await _orderController.Update(Guid.NewGuid(), _orderRequestModel);
        
        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }
    
    [Test]
    public async Task Delete_ReturnsNoContent()
    {
        // Arrange
        _orderServiceMock.Setup(x => x.DeleteAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);
        
        // Act
        var result = await _orderController.Delete(Guid.NewGuid());
        
        // Assert
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task PayOrder_WhenOrderNotFound_ReturnsNotFound()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var model = TestData.Payment.GenerateIbanPaymentRequestModel();
        
        _orderServiceMock.Setup(s => s.GetByIdAsync(orderId)).ReturnsAsync(null as OrderResponseDto);

        // Act
        var result = await _orderController.PayOrder(orderId, model);

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task PayOrder_WhenOrderNotInInitiatedStatus_ReturnsBadRequest()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var model = TestData.Payment.GenerateIbanPaymentRequestModel();
        var orderResponse = TestData.Order.GenerateOrderResponseDto();
        orderResponse.Status = OrderStatus.Created;

        _orderServiceMock.Setup(s => s.GetByIdAsync(orderId)).ReturnsAsync(orderResponse);

        // Act
        var result = await _orderController.PayOrder(orderId, model);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public void PayOrder_WhenInvalidPaymentType_ThrowsArgumentException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var model = new PaymentRequestModel { Type = (PaymentMethod)99 };

        _orderServiceMock.Setup(s => s.GetByIdAsync(orderId)).ReturnsAsync(TestData.Order.GenerateOrderResponseDto());

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() => _orderController.PayOrder(orderId, model));
    }

    [Test]
    public async Task PayOrder_WhenPaymentFails_ReturnsBadRequest()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var model = TestData.Payment.GenerateIbanPaymentRequestModel();

        _orderServiceMock.Setup(s => s.GetByIdAsync(orderId)).ReturnsAsync(TestData.Order.GenerateOrderResponseDto());
        _paymentServiceMock.Setup(p => p.PayIbanAsync(model.Iban!)).ReturnsAsync(false);

        // Act
        var result = await _orderController.PayOrder(orderId, model);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task PayOrder_WhenPaymentSucceeds_ReturnsOkAndChangeOrderStatus()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var model = TestData.Payment.GenerateIbanPaymentRequestModel();
        var order = TestData.Order.GenerateOrderResponseDto();

        _orderServiceMock.Setup(s => s.GetByIdAsync(orderId)).ReturnsAsync(order);
        _paymentServiceMock.Setup(p => p.PayIbanAsync(It.IsAny<string>())).ReturnsAsync(true);

        // Act
        var result = await _orderController.PayOrder(orderId, model);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());

        var okResult = result as OkObjectResult;
        var returnedOrder = okResult?.Value as OrderResponseDto;
        Assert.That(returnedOrder, Is.Not.Null);
        Assert.That(returnedOrder?.Status, Is.EqualTo(OrderStatus.Paid));
    }
}