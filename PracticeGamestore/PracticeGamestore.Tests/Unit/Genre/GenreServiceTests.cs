using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Services.Genre;
using PracticeGamestore.DataAccess.Repositories.Game;
using PracticeGamestore.DataAccess.Repositories.Genre;
using PracticeGamestore.DataAccess.UnitOfWork;

namespace PracticeGamestore.Tests.Unit.Genre;

[TestFixture]
public class GenreServiceTests
{
    private Mock<IGenreRepository> _genreRepository;
    private Mock<IUnitOfWork> _unitOfWork;
    private Mock<IGameRepository> _gameRepository;
    private GenreService _service;

    [SetUp]
    public void Setup()
    {
        _genreRepository = new Mock<IGenreRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _gameRepository = new Mock<IGameRepository>();
        _service = new GenreService(_genreRepository.Object, _gameRepository.Object, _unitOfWork.Object); // Correct order
    }
  
    [Test]
    public async Task GetAllAsync_WhenGenresExist_ShouldReturnAllGenres()
    {
        // Arrange
        var entities = TestData.Genre.GenerateGenreEntities();
        
        _genreRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(entities);
        
        // Act
        var result = (await _service.GetAllAsync()).ToList();
        
        // Assert
        Assert.That(result.Count, Is.EqualTo(entities.Count));
        Assert.That(result.First().Id, Is.EqualTo(entities.First().Id));
        Assert.That(result.First().Name, Is.EqualTo(entities.First().Name));
    }

    [Test]
    public async Task GetByIdAsync_WhenGenreExists_ShouldReturnGenreDto()
    {
        // Arrange
        var entity = TestData.Genre.GenerateActionGenre();
        
        _genreRepository.Setup(x => x.GetByIdAsync(entity.Id)).ReturnsAsync(entity);
        
        // Act
        var result = await _service.GetByIdAsync(entity.Id);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result?.Id, Is.EqualTo(entity.Id));
        Assert.That(result?.Name, Is.EqualTo(entity.Name));
    }
    
    [Test]
    public async Task GetByIdAsync_WhenGenreDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        _genreRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as DataAccess.Entities.Genre);
        
        // Act
        var result = await _service.GetByIdAsync(Guid.NewGuid());
        
        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task CreateAsync_WhenChangesSavedSuccessfully_ShouldReturnCreatedId()
    {
        // Arrange
        var dto = TestData.Genre.GenerateGenreDto();
        
        _genreRepository
            .Setup(x => x.CreateAsync(It.IsAny<DataAccess.Entities.Genre>()))
            .ReturnsAsync(dto.Id!.Value);
        _unitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        
        // Act
        var result = await _service.CreateAsync(dto);
        
        // Assert
        Assert.That(result, Is.EqualTo(dto.Id));
    }
    
    [Test]
    public async Task CreateAsync_WhenSaveChangesFailed_ShouldReturnNull()
    {
        // Arrange
        var dto = TestData.Genre.GenerateGenreDto();
        
        _genreRepository
            .Setup(x => x.CreateAsync(It.IsAny<DataAccess.Entities.Genre>()))
            .ReturnsAsync(dto.Id!.Value);
        _unitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);
        
        // Act
        var result = await _service.CreateAsync(dto);
        
        // Assert
        Assert.That(result, Is.Null);
    }
    
    [Test]
    public void CreateAsync_WhenNameAlreadyExists_ShouldThrowArgumentException()
    {
        // Arrange
        var dto = new GenreDto(Guid.NewGuid(), "Action");
        
        _genreRepository.Setup(g => g.ExistsByNameAsync(dto.Name)).ReturnsAsync(true);
    
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(dto));
    }
    
    [Test]
    public async Task CreateAsync_WhenParentIsInvalid_ShouldReturnNull()
    {
        // Arrange
        var invalidParentId = Guid.NewGuid();
        var dto = TestData.Genre.GenerateGenreDto();
    
        _genreRepository
            .Setup(x => x.GetByIdAsync(invalidParentId))
            .ReturnsAsync(null as DataAccess.Entities.Genre);
    
        // Act
        var result = await _service.CreateAsync(dto);
    
        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task UpdateAsync_WhenEntityExistsAndChangesSavedSuccessfully_ShouldReturnTrue()
    {
        // Arrange
        var dto = TestData.Genre.GenerateGenreDto();
        var entity = TestData.Genre.GenerateActionGenre();
        
        _genreRepository
            .Setup(x => x.GetByIdAsync(dto.Id!.Value))
            .ReturnsAsync(entity);
        _unitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        
        // Act
        var result = await _service.UpdateAsync(dto.Id!.Value, dto);
        
        // Assert
        Assert.That(result, Is.True);
    }
    
    [Test]
    public async Task UpdateAsync_WhenEntityDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = TestData.Genre.GenerateGenreDto();
        
        _genreRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as DataAccess.Entities.Genre);
        
        // Act
        var result = await _service.UpdateAsync(id, dto);
        
        // Assert
        Assert.That(result, Is.False);
    }
    
    [Test]
    public void UpdateAsync_WhenNameAlreadyExists_ShouldThrowArgumentException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = new GenreDto(id, "FPS");
        var entity = new DataAccess.Entities.Genre { Id = id, Name = "Action" };
        
        _genreRepository.Setup(g => g.GetByIdAsync(id)).ReturnsAsync(entity);
        _genreRepository.Setup(g => g.ExistsByNameAsync(dto.Name)).ReturnsAsync(true);
    
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(dto));
    }
    
    [Test]
    public async Task UpdateAsync_WhenParentIsSelf_ShouldReturnFalse()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = TestData.Genre.GenerateGenreDto();
        var entity = TestData.Genre.GenerateActionGenre();
    
        _genreRepository
            .Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(entity);
    
        // Act
        var result = await _service.UpdateAsync(id, dto);
    
        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task DeleteAsync_WhenGenresIsDeleted_ShouldCallDeleteAndSaveChanges()
    {
        // Arrange
        var id = Guid.NewGuid();
        
        // Act
        await _service.DeleteAsync(id);
         
        // Assert
        _genreRepository.Verify(x => x.DeleteAsync(id), Times.Once);
        _unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Test]
    public async Task GetByGenreGamesAsync_WhenGenreExistAndUserIsAdult_ShouldReturnAllGames()
    {
        // Arrange
        var actionGenreId = TestData.Genre.GenerateActionGenre().Id;
        var children = TestData.Genre.GenerateGenreChildren(actionGenreId);
        var hideAdultContent = false;
        var games = TestData.Game.GenerateGameEntities()
            .Where(game => game.GameGenres.Any(gg => children.Contains(gg.GenreId))).ToList();
        _genreRepository.Setup(x => x.ExistsAsync(actionGenreId)).ReturnsAsync(true);
        _genreRepository.Setup(x => x.GetGenreChildrenIdsAsync(actionGenreId)).ReturnsAsync(children);
        _gameRepository.Setup(x => x.GetByGenreAndItsChildrenAsync(children, hideAdultContent)).ReturnsAsync(games);

        // Act
        var result = (await _service.GetGamesAsync(actionGenreId) ?? Array.Empty<GameResponseDto>()).ToList();
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(games.Count));
        Assert.That(result.All(g => g.Genres.Any(genre => children.Contains(genre.Id!.Value))), Is.True);
    }
    
    [Test]
    public async Task GetByGenreGamesAsync_WhenGenreExistAndUserIsUnderage_ShouldReturnGamesWithAgeRatingLessThan18()
    {
        // Arrange
        var actionGenreId = TestData.Genre.GenerateActionGenre().Id;
        var children = TestData.Genre.GenerateGenreChildren(actionGenreId);
        var hideAdultContent = true;
        var games = TestData.Game.GenerateGameEntitiesWithAgeRatingLessThan18()
            .Where(game => game.GameGenres.Any(gg => children.Contains(gg.GenreId))).ToList();
        _genreRepository.Setup(x => x.ExistsAsync(actionGenreId)).ReturnsAsync(true);
        _genreRepository.Setup(x => x.GetGenreChildrenIdsAsync(actionGenreId)).ReturnsAsync(children);
        _gameRepository.Setup(x => x.GetByGenreAndItsChildrenAsync(children, hideAdultContent)).ReturnsAsync(games);

        // Act
        var result = (await _service.GetGamesAsync(actionGenreId) ?? Array.Empty<GameResponseDto>()).ToList();
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(games.Count));
        Assert.That(result.All(g => g.Genres.Any(genre => children.Contains(genre.Id!.Value))), Is.True);
    }
    
    [Test]
    public async Task GetByGenreGamesAsync_WhenGenreDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        _genreRepository.Setup(x => x.ExistsAsync(It.IsAny<Guid>()))
            .ReturnsAsync(false);

        // Act
        var result = await _service.GetGamesAsync(Guid.NewGuid());
        
        // Assert
        Assert.That(result, Is.Null);
    }
}