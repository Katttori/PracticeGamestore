using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.Constants;
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
        // Arrange
        var entities = TestData.Blacklist.GenerateBlacklistEntities();
        
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
        // Arrange
        var entity = TestData.Blacklist.GenerateBlacklistEntity();
        
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
        // Arrange
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
        // Arrange
        var dto = TestData.Blacklist.GenerateBlacklistDto();
        
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
        // Arrange
        var dto = TestData.Blacklist.GenerateBlacklistDto();
        
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
    public void CreateAsync_WhenEmailAlreadyExists_ThrowsArgumentException()
    {
        // Arrange
        var dto = TestData.Blacklist.GenerateBlacklistDto();
        
        _blacklistRepositoryMock.Setup(b => b.ExistsByUserEmailAsync(dto.UserEmail)).ReturnsAsync(true);
    
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(dto));
    }

    [Test]
    public async Task UpdateAsync_WhenEntityExistsAndChangesSavedSuccessfully_ReturnsTrue()
    {
        // Arrange
        var dto = TestData.Blacklist.GenerateBlacklistDto();
        var entity = TestData.Blacklist.GenerateBlacklistEntity();
        
        _blacklistRepositoryMock
            .Setup(x => x.GetByIdAsync(dto.Id))
            .ReturnsAsync(entity);
        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        
        // Act
        var result = await _service.UpdateAsync(dto.Id!.Value, dto);
        
        // Assert
        Assert.That(result, Is.True);
    }
    
    [Test]
    public async Task UpdateAsync_WhenEntityDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = TestData.Blacklist.GenerateBlacklistDto();
        
        _blacklistRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as DataAccess.Entities.Blacklist);
        
        // Act
        var result = await _service.UpdateAsync(id, dto);
        
        // Assert
        Assert.That(result, Is.False);
    }
    
    [Test]
    public void UpdateAsync_WhenEmailAlreadyExists_ThrowsArgumentException()
    {
        // Arrange
        var entity = TestData.Blacklist.GenerateBlacklistEntity();
        var dto = TestData.Blacklist.GenerateBlacklistDto();
        
        _blacklistRepositoryMock.Setup(b => b.GetByIdAsync(dto.Id)).ReturnsAsync(entity);
        _blacklistRepositoryMock.Setup(b => b.ExistsByUserEmailAsync(dto.UserEmail)).ReturnsAsync(true);
    
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateAsync(dto.Id!.Value, dto));
    }

    [Test]
    public async Task DeleteAsync_CallsDeleteAndSaveChanges()
    {
        // Arrange
        var id = Guid.NewGuid();
        
        // Act
        await _service.DeleteAsync(id);
         
        // Assert
        _blacklistRepositoryMock.Verify(x => x.DeleteAsync(id), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Test]
    public void HandleUserEmailAccessAsync_WhenEmailHeaderIsMissing_ThrowsArgumentException()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(() => _service.HandleUserEmailAccessAsync(""));
        Assert.That(ex?.Message, Is.EqualTo(ErrorMessages.MissingEmailHeader));
    }
    
    [Test]
    public void HandleUserEmailAccessAsync_WhenUserIsBlacklisted_ThrowsUnauthorized()
    {
        // Arrange
        const string userEmail = "banned@gmail.com";
        
        _blacklistRepositoryMock.Setup(s => s.ExistsByUserEmailAsync(userEmail)).ReturnsAsync(true);

        // Act & Assert
        var ex = Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.HandleUserEmailAccessAsync(userEmail));
        
        Assert.That(ex?.Message, Is.EqualTo(ErrorMessages.BlacklistedUser));
    }
}