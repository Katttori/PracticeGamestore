using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.DataTransferObjects.Order;
using PracticeGamestore.Business.Services.HeaderHandle;
using PracticeGamestore.Business.Services.Order;
using PracticeGamestore.Controllers;
using PracticeGamestore.Models.Order;

namespace PracticeGamestore.Tests.Unit.Order;

[TestFixture]
public class OrderControllerTests
{
    private Mock<IOrderService> _orderServiceMock;
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
        _headerHandleServiceMock = new Mock<IHeaderHandleService>();
        _loggerMock = new Mock<ILogger<OrderController>>();
        _orderController = new OrderController(_orderServiceMock.Object, _headerHandleServiceMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task GetAll_WhenOrdersExist_ShouldReturnOkWithOrders()
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
    public async Task Create_WhenOperationFailed_ShouldReturnBadRequest()
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
    public async Task Create_WhenOperationSuccessful_ShouldReturnCreatedWithId()
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
    public async Task Update_WhenOperationSuccessful_ShouldReturnNoContent()
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
    public async Task Update_WhenOperationFailed_ShouldReturnBadRequest()
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
    public async Task Delete_WhenOrderIsDeleted_ShouldReturnNoContent()
    {
        // Arrange
        _orderServiceMock.Setup(x => x.DeleteAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);
        
        // Act
        var result = await _orderController.Delete(Guid.NewGuid());
        
        // Assert
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }
}