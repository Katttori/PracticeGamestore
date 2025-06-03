using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Services.Blacklist;
using PracticeGamestore.DataAccess.Repositories.Blacklist;
using PracticeGamestore.DataAccess.UnitOfWork;

namespace PracticeGamestore.Tests.Unit.Blacklist;

public class BlacklistServiceTests
{
    private Mock<IBlacklistRepository> _blacklistRepositoryMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private BlacklistService _service;

    [SetUp]
    public void Setup()
    {
        _blacklistRepositoryMock = new Mock<IBlacklistRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _service = new BlacklistService(_blacklistRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task GetAllAsync_ReturnsAllBlacklists()
    {
        //Arrange
        var entities = new List<DataAccess.Entities.Blacklist>
        {
            new() { Id = Guid.NewGuid(), UserEmail = "user@example.com"},
            new() { Id = Guid.NewGuid(), UserEmail = "user@example.com" },
        };
        
        _blacklistRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(entities);
        
        // Act
        var result = (await _service.GetAllAsync()).ToList();
        
        // Assert
        Assert.That(result.Count, Is.EqualTo(entities.Count));
        Assert.That(result.First().Id, Is.EqualTo(entities.First().Id));
        Assert.That(result.First().UserEmail, Is.EqualTo(entities.First().UserEmail));
    }

    [Test]
    public async Task GetByIdAsync_WhenBlacklistExists_ReturnsBlacklistDto()
    {
        //Arrange
        var entity = new DataAccess.Entities.Blacklist { Id = Guid.NewGuid(), UserEmail = "user@example.com" };
        
        _blacklistRepositoryMock.Setup(x => x.GetByIdAsync(entity.Id)).ReturnsAsync(entity);
        
        // Act
        var result = await _service.GetByIdAsync(entity.Id);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result?.Id, Is.EqualTo(entity.Id));
        Assert.That(result?.UserEmail, Is.EqualTo(entity.UserEmail));
    }
    
    [Test]
    public async Task GetByIdAsync_WhenBlacklistDoesNotExist_ReturnsNull()
    {
        //Arrange
        _blacklistRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as DataAccess.Entities.Blacklist);
        
        // Act
        var result = await _service.GetByIdAsync(Guid.NewGuid());
        
        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task CreateAsync_WhenChangesSavedSuccessfully_ReturnsCreatedId()
    {
        //Arrange
        var dto = new BlacklistDto(Guid.NewGuid(), "user@example.com", Guid.NewGuid());
        
        _blacklistRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<DataAccess.Entities.Blacklist>()))
            .ReturnsAsync(dto.Id!.Value);
        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        
        // Act
        var result = await _service.CreateAsync(dto);
        
        // Assert
        Assert.That(result, Is.EqualTo(dto.Id));
    }
    
    [Test]
    public async Task CreateAsync_WhenSaveChangesFailed_ReturnsNull()
    {
        //Arrange
        var dto = new BlacklistDto(Guid.NewGuid(), "user@example.com", Guid.NewGuid());
        
        _blacklistRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<DataAccess.Entities.Blacklist>()))
            .ReturnsAsync(dto.Id!.Value);
        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);
        
        // Act
        var result = await _service.CreateAsync(dto);
        
        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task UpdateAsync_WhenEntityExistsAndChangesSavedSuccessfully_ReturnsTrue()
    {
        //Arrange
        var id = Guid.NewGuid();
        var dto = new BlacklistDto(id, "user@example.com", Guid.NewGuid());
        var entity = new DataAccess.Entities.Blacklist { Id = id, UserEmail = "user@example.com" };
        
        _blacklistRepositoryMock
            .Setup(x => x.GetByIdAsync(entity.Id))
            .ReturnsAsync(entity);
        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        
        // Act
        var result = await _service.UpdateAsync(id, dto);
        
        // Assert
        Assert.That(result, Is.True);
    }
    
    [Test]
    public async Task UpdateAsync_WhenEntityDoesNotExist_ReturnsFalse()
    {
        //Arrange
        var id = Guid.NewGuid();
        var dto = new BlacklistDto(id, "user@example.com", Guid.NewGuid());
        
        _blacklistRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as DataAccess.Entities.Blacklist);
        
        // Act
        var result = await _service.UpdateAsync(id, dto);
        
        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task DeleteAsync_CallsDeleteAndSaveChanges()
    {
        //Arrange
        var id = Guid.NewGuid();
        
        // Act
        await _service.DeleteAsync(id);
         
        // Assert
        _blacklistRepositoryMock.Verify(x => x.DeleteAsync(id), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}