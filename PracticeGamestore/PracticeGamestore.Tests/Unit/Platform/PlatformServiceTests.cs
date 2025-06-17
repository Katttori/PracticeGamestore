using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.Business.Services.Platform;
using PracticeGamestore.DataAccess.Enums;
using PracticeGamestore.DataAccess.Repositories.Game;
using PracticeGamestore.DataAccess.Repositories.Platform;
using PracticeGamestore.DataAccess.UnitOfWork;

namespace PracticeGamestore.Tests.Unit.Platform;

[TestFixture]
public class PlatformServiceTests
{
    private Mock<IPlatformRepository> _platformRepository;
    private Mock<IGameRepository> _gameRepository;
    private Mock<IUnitOfWork> _unitOfWork;
    private IPlatformService _platformService;
    
    [SetUp]
    public void Setup()
    {
        _platformRepository = new Mock<IPlatformRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _gameRepository = new Mock<IGameRepository>();
        _platformService = new PlatformService(_platformRepository.Object,_gameRepository.Object, _unitOfWork.Object);
    }
    
    [Test]
    public async Task GetAllAsync_ShouldReturnAllPlatforms()
    {
        // Arrange
        var platforms = TestData.Platform.GeneratePlatformEntities();
        
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
        var platform = TestData.Platform.GeneratePlatformEntity();
        
        _platformRepository.Setup(repo => repo.GetByIdAsync(platform.Id)).ReturnsAsync(platform);
        
        // Act
        var result = await _platformService.GetByIdAsync(platform.Id);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result?.Id, Is.EqualTo(platform.Id));
        Assert.That(result?.Name, Is.EqualTo(platform.Name));
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
        var platformDto = TestData.Platform.GeneratePlatformDto();

        _platformRepository.Setup(p => p.CreateAsync(It.IsAny<DataAccess.Entities.Platform>()))
            .ReturnsAsync(platformDto.MapToPlatformEntity().Id);
        _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        
        // Act
        var result = await _platformService.CreateAsync(platformDto);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(platformDto.MapToPlatformEntity().Id));
    }

    [Test]
    public async Task CreateAsync_ShouldReturnNull_WhenChangesNotSaved()
    {
        // Arrange
        var platformDto = TestData.Platform.GeneratePlatformDto();

        _platformRepository
            .Setup(p => p.CreateAsync(It.IsAny<DataAccess.Entities.Platform>()))
            .ReturnsAsync(platformDto.MapToPlatformEntity().Id);
        
        _unitOfWork
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        // Act
        var result = await _platformService.CreateAsync(platformDto);
        
        // Assert
        Assert.That(result, Is.Null);
    }
    
    [Test]
    public void CreateAsync_WhenNameAlreadyExists_ThrowsArgumentException()
    {
        // Arrange
        var platformDto = TestData.Platform.GeneratePlatformDto();
        
        _platformRepository.Setup(p => p.ExistsByNameAsync(platformDto.Name)).ReturnsAsync(true);
    
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() => _platformService.CreateAsync(platformDto));
    }
    
    [Test]
    public async Task UpdateAsync_ShouldReturnTrue_WhenPlatformUpdatedSuccessfully()
    {
        // Arrange
        var platformDto = TestData.Platform.GeneratePlatformDto();
        var platform = TestData.Platform.GeneratePlatformEntity();
        
        _platformRepository
            .Setup(p => p.GetByIdAsync(platformDto.Id!.Value))
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
        var platformDto = TestData.Platform.GeneratePlatformDto();
        
        _platformRepository
            .Setup(p => p.GetByIdAsync(platformDto.MapToPlatformEntity().Id))
            .ReturnsAsync(null as DataAccess.Entities.Platform);
        
        // Act
        var result = await _platformService.UpdateAsync(platformDto);
        
        // Assert
        Assert.That(result, Is.False);
    }
    
    [Test]
    public void UpdateAsync_WhenNameAlreadyExists_ThrowsArgumentException()
    {
        // Arrange
        var platformDto = TestData.Platform.GeneratePlatformDto();
        var platform = TestData.Platform.GeneratePlatformEntity();
        platformDto.Name = "New Name";
        
        _platformRepository.Setup(p => p.GetByIdAsync(platformDto.Id!.Value)).ReturnsAsync(platform);
        _platformRepository.Setup(p => p.ExistsByNameAsync(platformDto.Name)).ReturnsAsync(true);
    
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() => _platformService.UpdateAsync(platformDto));
    }
    
    [Test]
    public async Task UpdateAsync_ShouldReturnFalse_WhenChangesNotSaved()
    {
        // Arrange
        var platformDto = TestData.Platform.GeneratePlatformDto();
        var platform = TestData.Platform.GeneratePlatformEntity();
        
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
    
    [Test]
    public async Task GetByPlatformGamesAsync_WhenPlatformExistsAndUserIsAdult_ShouldReturnAllGames()
    {
        //Arrange
        var hideAdultContent = false;
        var games = TestData.Game.GenerateGameEntities();
        var platformId = games[0].GamePlatforms[0].PlatformId;
        games = games.Where(g => g.GamePlatforms.Any(x => x.PlatformId == platformId)).ToList();
        _platformRepository.Setup(p => p.ExistsByIdAsync(platformId)).ReturnsAsync(true);
        _gameRepository.Setup(g => g.GetByPlatformIdAsync(platformId, hideAdultContent)).ReturnsAsync(games);
        var expected = games.Select(p => p.MapToGameDto()).ToList();

        //Act
        var result = 
            (await _platformService.GetGamesAsync(platformId) ?? Array.Empty<GameResponseDto>())
            .ToList();

        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        Assert.That(result.Count, Is.EqualTo(expected.Count));
        Assert.That(result.All(game => game.Platforms.Any(p => p.Id == platformId)), Is.True);
    }
     
    [Test]
    public async Task GetByPlatformGamesAsync_WhenPlatformExistsAndUserIsUnderage_ShouldReturnGamesWithAgeRatingLessThan18()
    {
        //Arrange
        var hideAdultContent = true;
        var games = TestData.Game.GenerateGameEntitiesWithAgeRatingLessThan18();
        var platformId = games[0].GamePlatforms[0].PlatformId;
        games = games.Where(g => g.GamePlatforms.Any(x => x.PlatformId == platformId)).ToList();
        _platformRepository.Setup(p => p.ExistsByIdAsync(platformId)).ReturnsAsync(true);
        _gameRepository.Setup(g => g.GetByPlatformIdAsync(platformId, hideAdultContent)).ReturnsAsync(games);
        var expected = games.Select(p => p.MapToGameDto()).ToList();

        //Act
        var result = 
            (await _platformService.GetGamesAsync(platformId, hideAdultContent) ?? Array.Empty<GameResponseDto>())
            .ToList();

        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        Assert.That(result.Count, Is.EqualTo(expected.Count));
        Assert.That(result.All(game => game.Platforms.Any(p => p.Id == platformId)), Is.True);
    }
    
    [Test]
    public async Task GetByPlatformGamesAsync_WhenPlatformDoesNotExist_ShouldReturnNull()
    {
        //Arrange
        _platformRepository.Setup(p => p.ExistsByIdAsync(It.IsAny<Guid>())).ReturnsAsync(false);

        //Act
        var result = await _platformService.GetGamesAsync(It.IsAny<Guid>());
        
        //Assert
        Assert.That(result, Is.Null);
    }
}