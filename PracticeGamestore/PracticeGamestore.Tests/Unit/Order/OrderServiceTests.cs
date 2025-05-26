using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Services.Order;
using PracticeGamestore.DataAccess.Entities;
using PracticeGamestore.DataAccess.Enums;
using PracticeGamestore.DataAccess.Repositories.Order;
using PracticeGamestore.DataAccess.UnitOfWork;

namespace PracticeGamestore.Tests.Unit.Order;

public class OrderServiceTests
{
    private Mock<IOrderRepository> _orderRepositoryMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private OrderService _orderService;
    
    private static readonly Guid FirstId = Guid.NewGuid();
    private static readonly Guid SecondId = Guid.NewGuid();
    
    private readonly List<DataAccess.Entities.Order> _orderEntities = new()
    {
        new DataAccess.Entities.Order
        {
            Id = FirstId,
            Status = OrderStatus.Created,
            UserEmail = "test@test.com",
            Total = 100,
            GameOrders =
            [
                new GameOrder { GameId = FirstId, OrderId = FirstId },
                new GameOrder { GameId = SecondId, OrderId = FirstId }
            ]
        },
        new DataAccess.Entities.Order
        {
            Id = SecondId,
            Status = OrderStatus.Paid,
            UserEmail = "test2@test.com",
            Total = 200,
            GameOrders =
            [
                new GameOrder { GameId = SecondId, OrderId = SecondId },
                new GameOrder { GameId = FirstId, OrderId = SecondId }
            ]
        }
    };
    
    private readonly OrderDto _orderDto = new(
        FirstId,
        OrderStatus.Initiated,
        "test@test.com",
        100,
        [
            new() { GameId = FirstId },
            new() { OrderId = FirstId }
        ]
    );

    [SetUp]
    public void Setup()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _orderService = new OrderService(_orderRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task GetAllAsync_ReturnsAllOrders()
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
            Assert.That(result[i].GameOrders.Count, Is.EqualTo(_orderEntities[i].GameOrders.Count));
        }
    }

    [Test]
    public async Task GetByIdAsync_WhenOrderExists_ReturnsOrderDto()
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
        Assert.That(result, Has.Property("GameOrders").Count.EqualTo(_orderEntities[0].GameOrders.Count));
    }
    
    [Test]
    public async Task GetByIdAsync_WhenOrderDoesNotExist_ReturnsNull()
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
    public async Task CreateAsync_WhenChangesSavedSuccessfully_ReturnsCreatedId()
    {
        //Arrange
        _orderRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<DataAccess.Entities.Order>()))
            .ReturnsAsync(_orderDto.Id);
        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        
        // Act
        var result = await _orderService.CreateAsync(_orderDto);
        
        // Assert
        Assert.That(result, Is.EqualTo(_orderDto.Id));
    }
    
    [Test]
    public async Task CreateAsync_WhenSaveChangesFailed_ReturnsNull()
    {
        //Arrange
        _orderRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<DataAccess.Entities.Order>()))
            .ReturnsAsync(_orderDto.Id);
        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);
        
        // Act
        var result = await _orderService.CreateAsync(_orderDto);
        
        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task UpdateAsync_WhenEntityExistsAndChangesSavedSuccessfully_ReturnsTrue()
    {
        //Arrange
        _orderRepositoryMock
            .Setup(x => x.GetByIdAsync(_orderEntities[0].Id))
            .ReturnsAsync(_orderEntities[0]);
        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        
        // Act
        var result = await _orderService.UpdateAsync(_orderDto);
        
        // Assert
        Assert.That(result, Is.True);
    }
    
    [Test]
    public async Task UpdateAsync_WhenEntityDoesNotExist_ReturnsFalse()
    {
        //Arrange
        _orderRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as DataAccess.Entities.Order);
        
        // Act
        var result = await _orderService.UpdateAsync(_orderDto);
        
        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task DeleteAsync_CallsDeleteAndSaveChanges()
    {
        //Arrange
        var id = Guid.NewGuid();
        
        // Act
        await _orderService.DeleteAsync(id);
         
        // Assert
        _orderRepositoryMock.Verify(x => x.DeleteAsync(id), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}