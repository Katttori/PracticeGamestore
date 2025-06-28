using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.DataTransferObjects.Order;
using PracticeGamestore.Business.DataTransferObjects.Payment;
using PracticeGamestore.Business.Services.HeaderHandle;
using PracticeGamestore.Business.Services.Order;
using PracticeGamestore.Controllers;
using PracticeGamestore.Mappers;
using PracticeGamestore.Models.Order;

namespace PracticeGamestore.Tests.Unit.Order;

[TestFixture]
public class OrderControllerTests
{
    private Mock<IOrderService> _orderServiceMock;
    private Mock<IHeaderHandleService> _headerHandleServiceMock;
    private Mock<ILogger<OrderController>> _loggerMock;
    private OrderController _orderController;
    
    private const string CountryHeader = "Ukraine";
    private const string UserEmailHeader = "test@gmail.com";

    [SetUp]
    public void Setup()
    {
        _orderServiceMock = new Mock<IOrderService>();
        _headerHandleServiceMock = new Mock<IHeaderHandleService>();
        _loggerMock = new Mock<ILogger<OrderController>>();
        _orderController =
            new OrderController(_orderServiceMock.Object, _headerHandleServiceMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task GetAll_WhenOrdersExist_ShouldReturnOkWithOrders()
    {
        // Arrange
        var orderDtos = TestData.Order.GenerateOrderResponseDtos();
        
        _orderServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(orderDtos);
        
        // Act
        var result = await _orderController.GetAll();
        
        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        var responseModels = 
            (okResult?.Value as IEnumerable<OrderResponseModel> ?? Array.Empty<OrderResponseModel>()).ToList();
        for (var i = 0; i < responseModels.Count; i++)
        {
            Assert.That(responseModels[i].Id, Is.EqualTo(orderDtos[i].Id));
            Assert.That(responseModels[i].Status, Is.EqualTo(orderDtos[i].Status.ToString()));
            Assert.That(responseModels[i].UserEmail, Is.EqualTo(orderDtos[i].UserEmail));
            Assert.That(responseModels[i].Total, Is.EqualTo(orderDtos[i].Total));
            Assert.That(responseModels[i].Games.Count, Is.EqualTo(orderDtos[i].Games.Count));
        }
    }
    
    [Test]
    public async Task GetById_WhenOrderIsNull_ShouldReturnNotFound()
    {
        // Arrange
        _orderServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as OrderResponseDto);
        
        // Act
        var result = await _orderController.GetById(Guid.NewGuid());
        
        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }
    
    [Test]
    public async Task GetById_WhenOrderFound_ShouldReturnOkWithOrder()
    {
        // Arrange
        var orderDtos = TestData.Order.GenerateOrderResponseDtos();
        
        _orderServiceMock.Setup(x => x.GetByIdAsync(orderDtos[0].Id!.Value)).ReturnsAsync(orderDtos[0]);
        
        // Act
        var result = await _orderController.GetById(orderDtos[0].Id!.Value);
        
        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var receivedOrder = (result as OkObjectResult)?.Value as OrderResponseModel;
        Assert.That(receivedOrder, Is.Not.Null);
        Assert.That(receivedOrder, Has.Property("Id").EqualTo(orderDtos[0].Id));
        Assert.That(receivedOrder, Has.Property("Status").EqualTo(orderDtos[0].Status.ToString()));
        Assert.That(receivedOrder, Has.Property("UserEmail").EqualTo(orderDtos[0].UserEmail));
        Assert.That(receivedOrder, Has.Property("Total").EqualTo(orderDtos[0].Total));
        Assert.That(receivedOrder, Has.Property("Games").Count.EqualTo(orderDtos[0].Games.Count));
    }
    
    [Test]
    public async Task Create_WhenOperationFailed_ShouldReturnBadRequest()
    {
        // Arrange
        var orderCreateRequestModel = TestData.Order.GenerateOrderCreateRequestModel();
        
        _orderServiceMock
            .Setup(x => x.CreateAsync(It.IsAny<OrderRequestDto>()))
            .ReturnsAsync(null as Guid?);
        
        // Act
        var result = await _orderController.Create(CountryHeader, UserEmailHeader, orderCreateRequestModel);
        
        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }
    
    [Test]
    public async Task Create_WhenOperationSuccessful_ShouldReturnCreatedWithId()
    {
        // Arrange
        var newId = Guid.NewGuid();
        var orderCreateRequestModel = TestData.Order.GenerateOrderCreateRequestModel();
        
        _orderServiceMock.Setup(x => x.CreateAsync(It.IsAny<OrderRequestDto>())).ReturnsAsync(newId);
        
        // Act
        var result = await _orderController.Create(CountryHeader, UserEmailHeader, orderCreateRequestModel);
        
        // Assert
        Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
        var createdResult = result as CreatedAtActionResult;
        Assert.That(createdResult?.Value, Is.EqualTo(newId));
    }
    
    [Test]
    public async Task Update_WhenOperationSuccessful_ShouldReturnNoContent()
    {
        // Arrange
        _orderServiceMock
            .Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<OrderRequestDto>()))
            .ReturnsAsync(true);
        
        // Act
        var result = await _orderController.Update(Guid.NewGuid(), TestData.Order.GenerateOrderUpdateRequestModel());
        
        // Assert
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }
    
    [Test]
    public async Task Update_WhenOperationFailed_ShouldReturnBadRequest()
    {
        // Arrange
        _orderServiceMock
            .Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<OrderRequestDto>()))
            .ReturnsAsync(false);
        
        // Act
        var result = await _orderController.Update(Guid.NewGuid(), TestData.Order.GenerateOrderUpdateRequestModel());
        
        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }
    
    [Test]
    public async Task Delete_WhenOrderIsDeleted_ShouldReturnNoContent()
    {
        // Arrange
        _orderServiceMock.Setup(x => x.DeleteAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);
        
        // Act
        var result = await _orderController.Delete(Guid.NewGuid());
        
        // Assert
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task PayOrder_WhenPaymentFails_ShouldReturnBadRequest()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var model = TestData.Payment.GeneratePaymentRequestModel(true);
        
        _orderServiceMock.Setup(s => s.PayOrderAsync(orderId, model.MapToPaymentDto()))
            .ReturnsAsync(null as Dictionary<string, string>);

        // Act
        var result = await _orderController.PayOrder(orderId, model);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }
    
    [Test]
    public async Task PayOrder_WhenPaymentSuccessful_ShouldReturnOkWithGameKeyMap()
    {
        // Arrange
        var order =TestData.Order.GenerateOrderEntities()[0];
        var gameKeyMap = TestData.Order.GenerateGameKeyMapForOrder(order);
        var model = TestData.Payment.GeneratePaymentRequestModel(true);
        
        _orderServiceMock.Setup(s => s.PayOrderAsync(order.Id, It.IsAny<PaymentDto>())).ReturnsAsync(gameKeyMap);

        // Act
        var result = await _orderController.PayOrder(order.Id, model);

        // Assert
        Assert.That(result, Is.Not.Null);
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult!.StatusCode, Is.EqualTo(200));
        Assert.That(okResult.Value, Is.Not.Null);
        var resultMap = okResult.Value as Dictionary<string, string>;
        Assert.That(resultMap!.Count, Is.EqualTo(gameKeyMap.Count));
        Assert.That(resultMap.Keys, Is.SubsetOf(gameKeyMap.Keys));
        Assert.That(resultMap, Is.EquivalentTo(gameKeyMap));
    }
    
    [Test]
    public async Task GetHistory_WhenUserEmailIsValid_ShouldReturnOkWithOrders()
    {
        // Arrange 
        var orderDtos = TestData.Order.GenerateOrderResponseDtos();
        
        _headerHandleServiceMock.Setup(x => x.CheckAccessAsync(CountryHeader, UserEmailHeader))
            .Returns(Task.CompletedTask);
        _orderServiceMock.Setup(x => x.GetOrdersByUserEmailAsync(UserEmailHeader))
            .ReturnsAsync(orderDtos);
        
        // Act
        var result = await _orderController.GetOrdersByUserEmail(CountryHeader, UserEmailHeader);
        
        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        var responseModels = 
            (okResult?.Value as IEnumerable<OrderResponseModel> ?? Array.Empty<OrderResponseModel>()).ToList();

        for (var i = 0; i < responseModels.Count; i++)
        {
            Assert.That(responseModels[i], Is.InstanceOf<OrderResponseModel>());
            Assert.That(responseModels[i].Id, Is.EqualTo(orderDtos[i].Id));
            Assert.That(responseModels[i].Status, Is.EqualTo(orderDtos[i].Status.ToString()));
            Assert.That(responseModels[i].UserEmail, Is.EqualTo(orderDtos[i].UserEmail));
            Assert.That(responseModels[i].Total, Is.EqualTo(orderDtos[i].Total));
            Assert.That(responseModels[i].Games.Count, Is.EqualTo(orderDtos[i].Games.Count));
        }
    }
}