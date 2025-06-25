using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Business.DataTransferObjects.Order;
using PracticeGamestore.Business.Services.Order;
using PracticeGamestore.Business.Services.Payment;
using PracticeGamestore.DataAccess.Enums;
using PracticeGamestore.DataAccess.Repositories.Game;
using PracticeGamestore.DataAccess.Repositories.Order;
using PracticeGamestore.DataAccess.UnitOfWork;

namespace PracticeGamestore.Tests.Unit.Order;

[TestFixture]
public class OrderServiceTests
{
    private Mock<IPaymentService> _paymentServiceMock;
    private Mock<IOrderRepository> _orderRepositoryMock;
    private Mock<IGameRepository> _gameRepositoryMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private OrderService _orderService;

    private readonly List<DataAccess.Entities.Order> _orderEntities = TestData.Order.GenerateOrderEntities();
    private readonly OrderRequestDto _orderRequestDto = TestData.Order.GenerateOrderRequestDto();
    private readonly OrderResponseDto _orderResponseDto = TestData.Order.GenerateOrderResponseDto();

    [SetUp]
    public void Setup()
    {
        _paymentServiceMock = new Mock<IPaymentService>();
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _gameRepositoryMock = new Mock<IGameRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _orderService = new OrderService(_paymentServiceMock.Object, _orderRepositoryMock.Object,
            _gameRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task GetAllAsync__WhenOrdersExist_ShouldReturnAllOrders()
    {
        //Arrange
        _orderRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(_orderEntities);
        
        // Act
        var result = (await _orderService.GetAllAsync()).ToList();
        
        // Assert
        Assert.That(result.Count, Is.EqualTo(_orderEntities.Count));
        for (var i = 0; i < result.Count; i++)
        {
            Assert.That(result[i].Id, Is.EqualTo(_orderEntities[i].Id));
            Assert.That(result[i].Status, Is.EqualTo(_orderEntities[i].Status));
            Assert.That(result[i].UserEmail, Is.EqualTo(_orderEntities[i].UserEmail));
            Assert.That(result[i].Total, Is.EqualTo(_orderEntities[i].Total));
            Assert.That(result[i].Games.Count, Is.EqualTo(_orderEntities[i].GameOrders.Count));
        }
    }

    [Test]
    public async Task GetByIdAsync_WhenOrderExists_ShouldReturnOrderDto()
    {
        //Arrange
        _orderRepositoryMock
            .Setup(x => x.GetByIdAsync(_orderEntities[0].Id))
            .ReturnsAsync(_orderEntities[0]);
        
        // Act
        var result = await _orderService.GetByIdAsync(_orderEntities[0].Id);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Property("Id").EqualTo(_orderEntities[0].Id));
        Assert.That(result, Has.Property("Status").EqualTo(_orderEntities[0].Status));
        Assert.That(result, Has.Property("UserEmail").EqualTo(_orderEntities[0].UserEmail));
        Assert.That(result, Has.Property("Total").EqualTo(_orderEntities[0].Total));
        Assert.That(result, Has.Property("Games").Count.EqualTo(_orderEntities[0].GameOrders.Count));
    }
    
    [Test]
    public async Task GetByIdAsync_WhenOrderDoesNotExist_ShouldReturnNull()
    {
        //Arrange
        _orderRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as DataAccess.Entities.Order);
        
        // Act
        var result = await _orderService.GetByIdAsync(Guid.NewGuid());
        
        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task CreateAsync_WhenChangesSavedSuccessfully_ShouldReturnCreatedId()
    {
        //Arrange
        _gameRepositoryMock
            .Setup(x => x.GetExistingIdsAsync(It.IsAny<List<Guid>>()))
            .ReturnsAsync((List<Guid> ids) => ids);
        _orderRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<DataAccess.Entities.Order>()))
            .ReturnsAsync(_orderResponseDto.Id!.Value);
        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        
        // Act
        var result = await _orderService.CreateAsync(_orderRequestDto);
        
        // Assert
        Assert.That(result, Is.EqualTo(_orderResponseDto.Id));
    }
    
    [Test]
    public async Task CreateAsync_WhenSaveChangesFailed_ShouldReturnNull()
    {
        //Arrange
        _gameRepositoryMock
            .Setup(x => x.GetExistingIdsAsync(It.IsAny<List<Guid>>()))
            .ReturnsAsync((List<Guid> ids) => ids);
        _orderRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<DataAccess.Entities.Order>()))
            .ReturnsAsync(_orderResponseDto.Id!.Value);
        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);
        
        // Act
        var result = await _orderService.CreateAsync(_orderRequestDto);
        
        // Assert
        Assert.That(result, Is.Null);
    }
    
    [Test]
    public async Task CreateAsync_WhenGameIdIsMissing_ShouldReturnNull()
    {
        // Arrange
        var existingIds = _orderRequestDto.GameIds.Take(1).ToList();

        _gameRepositoryMock
            .Setup(x => x.GetExistingIdsAsync(It.IsAny<List<Guid>>()))
            .ReturnsAsync(existingIds);

        // Act
        var result = await _orderService.CreateAsync(_orderRequestDto);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task UpdateAsync_WhenEntityExistsAndChangesSavedSuccessfully_ShouldReturnTrue()
    {
        //Arrange
        _gameRepositoryMock
            .Setup(x => x.GetExistingIdsAsync(It.IsAny<List<Guid>>()))
            .ReturnsAsync((List<Guid> ids) => ids);
        _orderRepositoryMock
            .Setup(x => x.GetByIdAsync(_orderEntities[0].Id))
            .ReturnsAsync(_orderEntities[0]);
        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        
        // Act
        var result = await _orderService.UpdateAsync(_orderEntities[0].Id, _orderRequestDto);
        
        // Assert
        Assert.That(result, Is.True);
    }
    
    [Test]
    public async Task UpdateAsync_WhenEntityDoesNotExist_ShouldReturnFalse()
    {
        //Arrange
        _gameRepositoryMock
            .Setup(x => x.GetExistingIdsAsync(It.IsAny<List<Guid>>()))
            .ReturnsAsync((List<Guid> ids) => ids);
        _orderRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as DataAccess.Entities.Order);
        
        // Act
        var result = await _orderService.UpdateAsync(It.IsAny<Guid>(), _orderRequestDto);
        
        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task DeleteAsync_WhenOrderIsDeleted_ShouldCallDeleteAndSaveChanges()
    {
        //Arrange
        var id = Guid.NewGuid();
        
        // Act
        await _orderService.DeleteAsync(id);
         
        // Assert
        _orderRepositoryMock.Verify(x => x.DeleteAsync(id), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Test]
    public void PayOrderAsync_WhenOrderNotFound_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var dto = TestData.Payment.GeneratePaymentDto();
        
        _orderRepositoryMock.Setup(s => s.GetByIdAsync(orderId)).ReturnsAsync(null as DataAccess.Entities.Order);
    
        // Act & Assert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(() => _orderService.PayOrderAsync(orderId, dto));
        Assert.That(ex?.Message, Is.EqualTo(ErrorMessages.NotFound("Order", orderId)));
    }
    
    [Test]
    public void PayOrderAsync_WhenOrderNotInInitiatedStatus_ShouldThrowArgumentException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var dto = TestData.Payment.GeneratePaymentDto();
        var order = TestData.Order.GenerateOrderEntities()[1];
    
        _orderRepositoryMock.Setup(s => s.GetByIdAsync(orderId)).ReturnsAsync(order);
    
        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(() => _orderService.PayOrderAsync(orderId, dto));
        Assert.That(ex?.Message, Is.EqualTo(ErrorMessages.IncorrectOrderStatusForPayment));
    }
    
    [Test]
    public void PayOrderAsync_WhenInvalidPaymentType_ShouldThrowArgumentException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var dto = TestData.Payment.GeneratePaymentDto();
        dto.Iban = null;
        dto.CreditCard = null;
        dto.Ibox = null;

        _orderRepositoryMock.Setup(s => s.GetByIdAsync(orderId))
            .ReturnsAsync(TestData.Order.GenerateOrderEntities()[0]);
    
        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(() => _orderService.PayOrderAsync(orderId, dto));
        Assert.That(ex?.Message, Is.EqualTo(ErrorMessages.InvalidPaymentType));
    }
    
    [Test]
    public async Task PayOrderAsync_WhenPaymentFails_ShouldReturnFalse()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var dto = TestData.Payment.GeneratePaymentDto();
    
        _orderRepositoryMock.Setup(s => s.GetByIdAsync(orderId))
            .ReturnsAsync(TestData.Order.GenerateOrderEntities()[0]);
        _paymentServiceMock.Setup(p => p.PayIbanAsync(dto.Iban!)).ReturnsAsync(false);
    
        // Act
        var result = await _orderService.PayOrderAsync(orderId, dto);
    
        // Assert
        Assert.That(result, Is.False);
    }
    
    [Test]
    public async Task PayOrderAsync_WhenPaymentSucceeds_ShouldReturnTrueAndChangeOrderStatus()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var dto = TestData.Payment.GeneratePaymentDto();
        var order = TestData.Order.GenerateOrderEntities()[0];
    
        _orderRepositoryMock.Setup(s => s.GetByIdAsync(orderId)).ReturnsAsync(order);
        _paymentServiceMock.Setup(p => p.PayIbanAsync(dto.Iban!)).ReturnsAsync(true);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
    
        // Act
        var result = await _orderService.PayOrderAsync(orderId, dto);
    
        // Assert
        Assert.That(result, Is.True);
        Assert.That(order.Status, Is.EqualTo(OrderStatus.Paid));
    }

    [Test]
    public async Task GetOrdersByUserEmailAsync_WhenUserExists_ShouldReturnOrdersDtos()
    {
        // Arrange
        var userEmail = "user@example.com";
        
        _orderRepositoryMock
            .Setup(x => x.GetOrdersByUserEmailAsync(userEmail))
            .ReturnsAsync(_orderEntities.Where(o => o.UserEmail == userEmail).ToList());
        
        // Act
        var result = (await _orderService.GetOrdersByUserEmailAsync(userEmail)).ToList();
        
        // Assert
        Assert.That(result.Count, Is.EqualTo(_orderEntities.Count(o => o.UserEmail == userEmail)));
        for (var i = 0; i < result.Count; i++)
        {
            Assert.That(result[i].Id, Is.EqualTo(_orderEntities[i].Id));
            Assert.That(result[i].Status, Is.EqualTo(_orderEntities[i].Status));
            Assert.That(result[i].UserEmail, Is.EqualTo(_orderEntities[i].UserEmail));
            Assert.That(result[i].Total, Is.EqualTo(_orderEntities[i].Total));
            Assert.That(result[i].Games.Count, Is.EqualTo(_orderEntities[i].GameOrders.Count));
        }
    }

    [Test]
    public async Task GetOrdersByUserEmailAsync_WhenUserDoesNotExist_ShouldReturnEmptyList()
    {
        // Arrange
        var userEmail = "nouser@example.com";

        _orderRepositoryMock
            .Setup(x => x.GetOrdersByUserEmailAsync(userEmail))
            .ReturnsAsync(new List<DataAccess.Entities.Order>());

        // Act
        var result = (await _orderService.GetOrdersByUserEmailAsync(userEmail)).ToList();

        // Assert
        Assert.That(result.Count, Is.EqualTo(0));
        Assert.That(result, Is.Not.Null);
    }
}