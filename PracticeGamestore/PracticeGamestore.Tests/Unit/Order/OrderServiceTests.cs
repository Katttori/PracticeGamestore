using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.DataTransferObjects.Order;
using PracticeGamestore.Business.Services.Order;
using PracticeGamestore.DataAccess.Repositories.Game;
using PracticeGamestore.DataAccess.Repositories.Order;
using PracticeGamestore.DataAccess.UnitOfWork;

namespace PracticeGamestore.Tests.Unit.Order;

public class OrderServiceTests
{
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
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _gameRepositoryMock = new Mock<IGameRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _orderService = new OrderService(_orderRepositoryMock.Object, _gameRepositoryMock.Object, _unitOfWorkMock.Object);
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
            Assert.That(result[i].Games.Count, Is.EqualTo(_orderEntities[i].GameOrders.Count));
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
        Assert.That(result, Has.Property("Games").Count.EqualTo(_orderEntities[0].GameOrders.Count));
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
    public async Task CreateAsync_WhenSaveChangesFailed_ReturnsNull()
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
    public async Task CreateAsync_WhenGameIdIsMissing_ReturnsNull()
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
    public async Task UpdateAsync_WhenEntityExistsAndChangesSavedSuccessfully_ReturnsTrue()
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
    public async Task UpdateAsync_WhenEntityDoesNotExist_ReturnsFalse()
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