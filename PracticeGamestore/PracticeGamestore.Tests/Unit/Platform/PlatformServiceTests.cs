using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.Business.Services.Platform;
using PracticeGamestore.DataAccess.Repositories.Platform;
using PracticeGamestore.DataAccess.UnitOfWork;

namespace PracticeGamestore.Tests.Unit.Platform;

public class PlatformServiceTests
{
    private Mock<IPlatformRepository> _platformRepository;
    private Mock<IUnitOfWork> _unitOfWork;
    private IPlatformService _platformService;
    
    [SetUp]
    public void Setup()
    {
        _platformRepository = new Mock<IPlatformRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _platformService = new PlatformService(_platformRepository.Object, _unitOfWork.Object);
    }
    
    [Test]
    public async Task GetAllAsync_ShouldReturnAllPlatforms()
    {
        // Arrange
        var platforms = new List<DataAccess.Entities.Platform>
        {
            new() {Id = Guid.NewGuid(), Name = "PC", Description = "Personal Computer"},
            new() {Id = Guid.NewGuid(), Name = "PS5", Description = "PlayStation 5"}
        };
        
        _platformRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(platforms);
        
        // Act
        var result = (await _platformService.GetAllAsync()).ToList();
        
        // Assert   
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(platforms.Count));
        Assert.That(result[0].Id, Is.EqualTo(platforms[0].Id));
        Assert.That(result[0].Name, Is.EqualTo(platforms[0].Name));
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnPlatformDto_WhenPlatformExists()
    {
        // Arrange
        var p = new DataAccess.Entities.Platform
        {
            Id = Guid.NewGuid(),
            Name = "PC",
            Description = "Personal Computer"
        };
        
        _platformRepository.Setup(repo => repo.GetByIdAsync(p.Id)).ReturnsAsync(p);
        
        // Act
        var result = await _platformService.GetByIdAsync(p.Id);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result?.Id, Is.EqualTo(p.Id));
        Assert.That(result?.Name, Is.EqualTo(p.Name));
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnNull_WhenPlatformDoesNotExists()
    {
        // Arrange
        _platformRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as DataAccess.Entities.Platform);
        
        // Act
        var result = await _platformService.GetByIdAsync(Guid.NewGuid());
        
        // Assert
        Assert.That(result, Is.Null);
    }
    
    [Test]
    public async Task CreateAsync_ShouldAddPlatform_WhenChangesSavedSuccessfully()
    {
        // Arrange
        var platformDto = new PlatformDto(Guid.NewGuid(), "PC", "Personal Computer");

        _platformRepository.Setup(p => p.CreateAsync(It.IsAny<DataAccess.Entities.Platform>()))
            .ReturnsAsync(platformDto.MapToEntity().Id);
        _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        
        // Act
        var result = await _platformService.CreateAsync(platformDto);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(platformDto.MapToEntity().Id));
    }

    [Test]
    public async Task CreateAsync_ShouldReturnNull_WhenChangesNotSaved()
    {
        // Arrange
        var platformDto = new PlatformDto(Guid.NewGuid(), "PC", "Personal Computer");

        _platformRepository
            .Setup(p => p.CreateAsync(It.IsAny<DataAccess.Entities.Platform>()))
            .ReturnsAsync(platformDto.MapToEntity().Id);
        
        _unitOfWork
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        // Act
        var result = await _platformService.CreateAsync(platformDto);
        
        // Assert
        Assert.That(result, Is.Null);
    }
    
    [Test]
    public async Task UpdateAsync_ShouldReturnTrue_WhenPlatformUpdatedSuccessfully()
    {
        // Arrange
        var platformDto = new PlatformDto(Guid.NewGuid(), "PC", "Personal Computer");
        var platform = new DataAccess.Entities.Platform
        {
            Id = platformDto.MapToEntity().Id,
            Name = platformDto.Name,
            Description = platformDto.Description
        };
        
        _platformRepository
            .Setup(p => p.GetByIdAsync(platform.Id))
            .ReturnsAsync(platform);
        
        _unitOfWork
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        
        // Act
        var result = await _platformService.UpdateAsync(platformDto);
        
        // Assert
        Assert.That(result, Is.True);
    }
    
    [Test]
    public async Task UpdateAsync_ShouldReturnFalse_WhenPlatformNotFound()
    {
        // Arrange
        var platformDto = new PlatformDto(Guid.NewGuid(), "PC", "Personal Computer");
        
        _platformRepository
            .Setup(p => p.GetByIdAsync(platformDto.MapToEntity().Id))
            .ReturnsAsync(null as DataAccess.Entities.Platform);
        
        // Act
        var result = await _platformService.UpdateAsync(platformDto);
        
        // Assert
        Assert.That(result, Is.False);
    }
    
    [Test]
    public async Task UpdateAsync_ShouldReturnFalse_WhenChangesNotSaved()
    {
        // Arrange
        var platformDto = new PlatformDto(Guid.NewGuid(), "PC", "Personal Computer");
        var platform = new DataAccess.Entities.Platform
        {
            Id = platformDto.MapToEntity().Id,
            Name = platformDto.Name,
            Description = platformDto.Description
        };
        
        _platformRepository
            .Setup(p => p.GetByIdAsync(platform.Id))
            .ReturnsAsync(platform);
        
        _unitOfWork
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);
        
        // Act
        var result = await _platformService.UpdateAsync(platformDto);
        
        // Assert
        Assert.That(result, Is.False);
    }
    
    [Test]
    public async Task DeleteAsync_ShouldCallDeleteAndSaveChanges()
    {
        // Arrange
        var platformId = Guid.NewGuid();
        
        _platformRepository
            .Setup(p => p.DeleteAsync(platformId))
            .Returns(Task.CompletedTask);
        
        _unitOfWork
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        
        // Act
        await _platformService.DeleteAsync(platformId);
        
        // Assert
        _platformRepository.Verify(p => p.DeleteAsync(platformId), Times.Once);
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}