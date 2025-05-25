using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.Business.Services.Game;
using PracticeGamestore.DataAccess.Enums;
using PracticeGamestore.DataAccess.Repositories.Game;
using PracticeGamestore.DataAccess.Repositories.Genre;
using PracticeGamestore.DataAccess.Repositories.Platform;
using PracticeGamestore.DataAccess.Repositories.Publisher;
using PracticeGamestore.DataAccess.UnitOfWork;

namespace PracticeGamestore.Tests.Unit.Game;

[TestFixture]
public class GameServiceTests
{
    private Mock<IGameRepository> _gameRepository;
    private Mock<IPublisherRepository> _publisherRepository;
    private Mock<IGenreRepository> _genreRepository;
    private Mock<IPlatformRepository> _platformRepository;
    private Mock<IUnitOfWork> _unitOfWork;
    private IGameService _gameService;
    
    [SetUp]
    public void SetUp()
    {
        _gameRepository = new Mock<IGameRepository>();
        _publisherRepository = new Mock<IPublisherRepository>();
        _genreRepository = new Mock<IGenreRepository>();
        _platformRepository = new Mock<IPlatformRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _gameService = new GameService(_gameRepository.Object, _publisherRepository.Object, _genreRepository.Object, _platformRepository.Object, _unitOfWork.Object);
    }

    private void SetUpDefaultMocks(IEnumerable<DataAccess.Entities.Genre> genres, IEnumerable<DataAccess.Entities.Platform> platforms, DataAccess.Entities.Game game)
    {
        _publisherRepository.Setup(x => x.GetByIdAsync(game.PublisherId))
            .ReturnsAsync(game.Publisher);
        _genreRepository.Setup(x => x.GetAllAsync())
            .ReturnsAsync(genres);
        _platformRepository.Setup(x => x.GetAllAsync())
            .ReturnsAsync(platforms);
    }
    
    private void SetUpMocksWhenPublisherDoesNotExist(IEnumerable<DataAccess.Entities.Genre> genres, IEnumerable<DataAccess.Entities.Platform> platforms, Guid publisherId)
    {
        _publisherRepository.Setup(x => x.GetByIdAsync(publisherId))
            .ReturnsAsync(null as DataAccess.Entities.Publisher);
        _genreRepository.Setup(x => x.GetAllAsync())
            .ReturnsAsync(genres);
        _platformRepository.Setup(x => x.GetAllAsync())
            .ReturnsAsync(platforms);
    }

    private static bool GameResponseDtosAreTheSame(GameResponseDto dto1, GameResponseDto dto2)
    {
        return dto1.Id.Equals(dto2.Id) && 
               dto1.Name == dto2.Name && 
               dto1.Key == dto2.Key && 
               dto1.Price == dto2.Price && 
               Equals(dto1.Picture, dto2.Picture) &&  
               dto1.Description == dto2.Description && 
               dto1.Rating.Equals(dto2.Rating) && 
               dto1.AgeRating == dto2.AgeRating && 
               dto1.ReleaseDate.Equals(dto2.ReleaseDate) && 
               dto1.Publisher.Id.Equals(dto2.Publisher.Id) && 
               dto1.Publisher.Name.Equals(dto2.Publisher.Name) && 
               dto1.Publisher.PageUrl.Equals(dto2.Publisher.PageUrl) && 
               dto1.Publisher.Description.Equals(dto2.Publisher.Description) && 
               dto1.Platforms.Count == dto2.Platforms.Count &&
               dto1.Platforms.All(ep => dto2.Platforms.Any(rp => rp.Id == ep.Id && rp.Name == ep.Name && rp.Description == ep.Description)) &&
               dto1.Genres.Count == dto2.Genres.Count &&
               dto1.Genres.All(eg => dto2.Genres.Any(rg => rg.Id == eg.Id && rg.Name == eg.Name && rg.Description == eg.Description && rg.ParentId == eg.ParentId));
    }
    
    [Test]
    public async Task GetAllAsync_ShouldReturnAllGames()
    {
        //Arrange
        var games = TestData.Game.GenerateGameEntities();
        _gameRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(games);
        var expected = games.Select(p => p.MapToGameDto()).ToList();
        
        //Act
        var result = (await _gameService.GetAllAsync()).ToList();
        
        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        Assert.That(result.Count, Is.EqualTo(expected.Count));
        var elementsAreTheSame = expected.Zip(result, GameResponseDtosAreTheSame).All(equal => equal);
        Assert.That(elementsAreTheSame, Is.True);
    }

    [Test]
    public async Task GetByIdAsync_WhenGameExists_ReturnsGameDto()
    {
        //Arrange
        var publishers = TestData.Publisher.GeneratePublisherEntities();
        var platforms = TestData.Platform.GeneratePlatformEntities();
        var genres = TestData.Genre.GenerateGenreEntities();
        var game =  TestData.Game.GenerateGameEntity(publishers, genres, platforms);
        _gameRepository.Setup(x => x.GetByIdAsync(game.Id))
            .ReturnsAsync(game);
        var expected = game.MapToGameDto();

        //Act
        var result = await _gameService.GetByIdAsync(game.Id);

        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(GameResponseDtosAreTheSame(result!, expected), Is.True);
    }

    [Test]
    public async Task GetByIdAsync_WhenGameDoesNotExist_ReturnsNull()
    {
        //Arrange
        _gameRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as DataAccess.Entities.Game);

        //Act
        var result = await _gameService.GetByIdAsync(Guid.NewGuid());
        
        //Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task UpdateAsync_WhenGameExistsAndSpecifiedRelationsExistAndChangesSavedSuccessfully_ReturnsTrue()
    {
        //Arrange
        var publishers = TestData.Publisher.GeneratePublisherEntities();
        var platforms = TestData.Platform.GeneratePlatformEntities();
        var genres = TestData.Genre.GenerateGenreEntities();
        var game =  TestData.Game.GenerateGameEntity(publishers, genres, platforms);  _gameRepository.Setup(x => x.GetByIdAsync(game.Id))
            .ReturnsAsync(game);
        SetUpDefaultMocks(genres, platforms, game);
        _unitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        
        //Act
        var result = await _gameService.UpdateAsync(game.Id, TestData.Game.GenerateGameRequestModel(game));
        
        //Assert
        Assert.That(result, Is.True);
    }
    
    [Test]
    public async Task UpdateAsync_WhenGameDoesNotExist_ReturnsFalse()
    {
        //Arrange
        var publishers = TestData.Publisher.GeneratePublisherEntities();
        var platforms = TestData.Platform.GeneratePlatformEntities();
        var genres = TestData.Genre.GenerateGenreEntities();
        var game =  TestData.Game.GenerateGameEntity(publishers, genres, platforms);   
        _gameRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as DataAccess.Entities.Game);

        //Act
        var result = await _gameService.UpdateAsync(game.Id, TestData.Game.GenerateGameRequestModel(game));
        
        //Assert
        Assert.That(result, Is.False);
    }
    
    [Test]
    public async Task UpdateAsync_WhenSpecifiedPublisherDoesNotExist_ReturnsFalse()
    {
        //Arrange
        var publishers = TestData.Publisher.GeneratePublisherEntities();
        var platforms = TestData.Platform.GeneratePlatformEntities();
        var genres = TestData.Genre.GenerateGenreEntities();
        var game =  TestData.Game.GenerateGameEntity(publishers, genres, platforms); game.PublisherId = Guid.NewGuid();
        _gameRepository.Setup(x => x.GetByIdAsync(game.Id))
            .ReturnsAsync(game);
        SetUpMocksWhenPublisherDoesNotExist(genres, platforms, game.PublisherId);

        //Act
        var result = await _gameService.UpdateAsync(game.Id, TestData.Game.GenerateGameRequestModel(game));
        
        //Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task UpdateAsync_WhenSpecifiedGenreDoesNotExist_ReturnsFalse()
    {
        //Arrange
        var publishers = TestData.Publisher.GeneratePublisherEntities();
        var platforms = TestData.Platform.GeneratePlatformEntities();
        var genres = TestData.Genre.GenerateGenreEntities();
        var game =  TestData.Game.GenerateGameEntity(publishers, genres, platforms);
        game.GameGenres[0].GenreId = Guid.NewGuid();
        _gameRepository.Setup(x => x.GetByIdAsync(game.Id))
            .ReturnsAsync(game);
        SetUpDefaultMocks(genres, platforms, game);

        //Act
        var result = await _gameService.UpdateAsync(game.Id, TestData.Game.GenerateGameRequestModel(game));
        
        //Assert
        Assert.That(result, Is.False);
    }
    
    [Test]
    public async Task UpdateAsync_WhenSpecifiedPlatformDoesNotExist_ReturnsFalse()
    {
        //Arrange
        var publishers = TestData.Publisher.GeneratePublisherEntities();
        var platforms = TestData.Platform.GeneratePlatformEntities();
        var genres = TestData.Genre.GenerateGenreEntities();
        var game =  TestData.Game.GenerateGameEntity(publishers, genres, platforms);
        _gameRepository.Setup(x => x.GetByIdAsync(game.Id))
            .ReturnsAsync(game);
        SetUpDefaultMocks(genres, platforms, game);
        
        //Act
        var result = await _gameService.UpdateAsync(game.Id, TestData.Game.GenerateGameRequestModel(game));
        
        //Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task UpdateAsync_ShouldReturnFalse_WhenChangesNotSaved()
    {
        // Arrange
        var publishers = TestData.Publisher.GeneratePublisherEntities();
        var platforms = TestData.Platform.GeneratePlatformEntities();
        var genres = TestData.Genre.GenerateGenreEntities();
        var game =  TestData.Game.GenerateGameEntity(publishers, genres, platforms);
        _gameRepository.Setup(x => x.GetByIdAsync(game.Id))
            .ReturnsAsync(game);
        SetUpDefaultMocks(genres, platforms, game);
        _unitOfWork
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);
        
        // Act
        var result = await _gameService.UpdateAsync(game.Id, TestData.Game.GenerateGameRequestModel(game));
        
        // Assert
        Assert.That(result, Is.False);
    }
    
    [Test]
    public async Task CreateAsync_ShouldAddGame_WhenChangesSavedSuccessfullyAndSpecifiedRealtionsAreCorrect()
    {
        //Arrange
        var publishers = TestData.Publisher.GeneratePublisherEntities();
        var platforms = TestData.Platform.GeneratePlatformEntities();
        var genres = TestData.Genre.GenerateGenreEntities();
        var game =  TestData.Game.GenerateGameEntity(publishers, genres, platforms);
        _gameRepository.Setup(x => x.CreateAsync(
                It.IsAny<DataAccess.Entities.Game>(), 
                It.IsAny<List<Guid>>(), 
                It.IsAny<List<Guid>>()))
            .ReturnsAsync(game.Id);     
        SetUpDefaultMocks(genres, platforms, game);
        _unitOfWork
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        
        //Act
        var result = await _gameService.CreateAsync(TestData.Game.GenerateGameRequestModel(game));

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(game.Id));
    }

    [Test]
    public async Task CreateAsync_ShouldReturnNull_WhenSpecifiedPublisherDoesNotExist()
    {
        //Arrange
        var publishers = TestData.Publisher.GeneratePublisherEntities();
        var platforms = TestData.Platform.GeneratePlatformEntities();
        var genres = TestData.Genre.GenerateGenreEntities();
        var game =  TestData.Game.GenerateGameEntity(publishers, genres, platforms);
        _gameRepository.Setup(x => x.CreateAsync(
                It.IsAny<DataAccess.Entities.Game>(), 
                It.IsAny<List<Guid>>(), 
                It.IsAny<List<Guid>>()))
            .ReturnsAsync(game.Id);     
        SetUpMocksWhenPublisherDoesNotExist(genres, platforms, game.PublisherId);
        _unitOfWork
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        
        //Act
        var result = await _gameService.CreateAsync(TestData.Game.GenerateGameRequestModel(game));

        // Assert
        Assert.That(result, Is.Null);
    }
    
    [Test]
    public async Task CreateAsync_ShouldReturnNull_WhenSpecifiedGenreDoesNotExist()
    {
        //Arrange
        var publishers = TestData.Publisher.GeneratePublisherEntities();
        var platforms = TestData.Platform.GeneratePlatformEntities();
        var genres = TestData.Genre.GenerateGenreEntities();
        var game =  TestData.Game.GenerateGameEntity(publishers, genres, platforms);
        game.GameGenres[0].GenreId = Guid.NewGuid();
        _gameRepository.Setup(x => x.CreateAsync(
                It.IsAny<DataAccess.Entities.Game>(), 
                It.IsAny<List<Guid>>(), 
                It.IsAny<List<Guid>>()))
            .ReturnsAsync(game.Id);    
        SetUpDefaultMocks(genres, platforms, game);
        _unitOfWork
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        
        //Act
        var result = await _gameService.CreateAsync(TestData.Game.GenerateGameRequestModel(game));

        // Assert
        Assert.That(result, Is.Null);
    }
    
    [Test]
    public async Task CreateAsync_ShouldReturnNull_WhenSpecifiedPlatformDoesNotExist()
    {
        //Arrange
        var publishers = TestData.Publisher.GeneratePublisherEntities();
        var platforms = TestData.Platform.GeneratePlatformEntities();
        var genres = TestData.Genre.GenerateGenreEntities();
        var game =  TestData.Game.GenerateGameEntity(publishers, genres, platforms); game.GamePlatforms[0].PlatformId = Guid.NewGuid();
        _gameRepository.Setup(x => x.CreateAsync(
                It.IsAny<DataAccess.Entities.Game>(), 
                It.IsAny<List<Guid>>(), 
                It.IsAny<List<Guid>>()))
            .ReturnsAsync(game.Id);    
        SetUpDefaultMocks(genres, platforms, game);
        _unitOfWork
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        
        //Act
        var result = await _gameService.CreateAsync(TestData.Game.GenerateGameRequestModel(game));

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task CreateAsync_ShouldReturnNull_WhenChangesNotSaved()
    {
        //Arrange
        var publishers = TestData.Publisher.GeneratePublisherEntities();
        var platforms = TestData.Platform.GeneratePlatformEntities();
        var genres = TestData.Genre.GenerateGenreEntities();
        var game =  TestData.Game.GenerateGameEntity(publishers, genres, platforms);
        _gameRepository.Setup(x => x.CreateAsync(
                It.IsAny<DataAccess.Entities.Game>(), 
                It.IsAny<List<Guid>>(), 
                It.IsAny<List<Guid>>()))
            .ReturnsAsync(game.Id);    
        SetUpDefaultMocks(genres, platforms, game);
        _unitOfWork
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);
        
        //Act
        var result = await _gameService.CreateAsync(TestData.Game.GenerateGameRequestModel(game));
        
        //Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task DeleteAsync_ShouldCallDeleteAndSaveChanges()
    {
        //Arrange
        var id = Guid.NewGuid();
        _gameRepository.Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
            .Returns(Task.CompletedTask);
        _unitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        //Act
        await _gameService.DeleteAsync(id);
        
        //Assert
        _gameRepository.Verify(x => x.DeleteAsync(id), Times.Once);
        _unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}