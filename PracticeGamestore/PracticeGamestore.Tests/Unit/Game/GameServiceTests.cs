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
using GameFilter = PracticeGamestore.Business.DataTransferObjects.Filtering.GameFilter;

namespace PracticeGamestore.Tests.Unit.Game;

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

    private void AssertThatResultListOfGamesIsEqualToExpectedList(List<GameResponseDto> result, List<GameResponseDto> expected)
    {
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        Assert.That(result.Count, Is.EqualTo(expected.Count));
        var elementsAreTheSame = expected.Zip(result, GameResponseDtosAreTheSame).All(equal => equal);
        Assert.That(elementsAreTheSame, Is.True);
    }
    
    [Test]
    public async Task GetAllAsync_WhenUserIsAdult_ShouldReturnAllGames()
    {
        //Arrange
        var hideAdultContent = false;
        var games = TestData.Game.GenerateGameEntities();
        _gameRepository.Setup(x => x.GetAllAsync(hideAdultContent)).ReturnsAsync(games);
        var expected = games.Select(p => p.MapToGameDto()).ToList();
        
        //Act
        var result = (await _gameService.GetAllAsync()).ToList();
        
        //Assert
        AssertThatResultListOfGamesIsEqualToExpectedList(result, expected);
    }
    
    [Test]
    public async Task GetAllAsync_WhenUserIsUnderage_ShouldReturnOnlyGamesWithAgeRatingLessThan18()
    {
        //Arrange
        var hideAdultContent = true;
        var games = TestData.Game.GenerateGameEntitiesWithAgeRatingLessThan18();
        _gameRepository.Setup(x => x.GetAllAsync(hideAdultContent)).ReturnsAsync(games);
        var expected = games.Select(p => p.MapToGameDto()).ToList();
        
        
        //Act
        var result = (await _gameService.GetAllAsync(hideAdultContent)).ToList();
        
        //Assert
        AssertThatResultListOfGamesIsEqualToExpectedList(result, expected);
    }

    [Test]
    public async Task GetByIdAsync_WhenGameExists_ShouldReturnGameDto()
    {
        //Arrange
        var publishers = TestData.Publisher.GeneratePublisherEntities();
        var platforms = TestData.Platform.GeneratePlatformEntities();
        var genres = TestData.Genre.GenerateGenreEntities();
        var game = TestData.Game.GenerateGameEntity(publishers, genres, platforms);
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
    public async Task GetByIdAsync_WhenGameDoesNotExist_ShouldReturnNull()
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
    public async Task UpdateAsync_WhenGameExistsAndSpecifiedRelationsExistAndChangesSavedSuccessfully_ShouldReturnTrue()
    {
        //Arrange
        var publishers = TestData.Publisher.GeneratePublisherEntities();
        var platforms = TestData.Platform.GeneratePlatformEntities();
        var genres = TestData.Genre.GenerateGenreEntities();
        var game = TestData.Game.GenerateGameEntity(publishers, genres, platforms);  _gameRepository.Setup(x => x.GetByIdAsync(game.Id))
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
    public async Task UpdateAsync_WhenGameDoesNotExist_ShouldReturnFalse()
    {
        //Arrange
        var publishers = TestData.Publisher.GeneratePublisherEntities();
        var platforms = TestData.Platform.GeneratePlatformEntities();
        var genres = TestData.Genre.GenerateGenreEntities();
        var game = TestData.Game.GenerateGameEntity(publishers, genres, platforms);   
        _gameRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as DataAccess.Entities.Game);

        //Act
        var result = await _gameService.UpdateAsync(game.Id, TestData.Game.GenerateGameRequestModel(game));
        
        //Assert
        Assert.That(result, Is.False);
    }
    
    [Test]
    public void UpdateAsync_WhenNameAlreadyExists_ShouldThrowArgumentException()
    {
        // Arrange
        var publishers = TestData.Publisher.GeneratePublisherEntities();
        var platforms = TestData.Platform.GeneratePlatformEntities();
        var genres = TestData.Genre.GenerateGenreEntities();
        var oldGame = TestData.Game.GenerateGameEntity(publishers, genres, platforms);
        var newGame = TestData.Game.GenerateGameEntity(publishers, genres, platforms);
        newGame.Name = "New Game";
        
        _gameRepository.Setup(g => g.GetByIdAsync(newGame.Id)).ReturnsAsync(oldGame);
        _gameRepository.Setup(g => g.ExistsByNameAsync(newGame.Name)).ReturnsAsync(true);
        
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() =>
            _gameService.UpdateAsync(newGame.Id, TestData.Game.GenerateGameRequestModel(newGame)));
    }
    
    [Test]
    public void UpdateAsync_WhenKeyAlreadyExists_ShouldThrowArgumentException()
    {
        // Arrange
        var publishers = TestData.Publisher.GeneratePublisherEntities();
        var platforms = TestData.Platform.GeneratePlatformEntities();
        var genres = TestData.Genre.GenerateGenreEntities();
        var oldGame = TestData.Game.GenerateGameEntity(publishers, genres, platforms);
        var newGame = TestData.Game.GenerateGameEntity(publishers, genres, platforms);
        newGame.Key = "New Key";
        
        _gameRepository.Setup(g => g.GetByIdAsync(newGame.Id)).ReturnsAsync(oldGame);
        _gameRepository.Setup(g => g.ExistsByKeyAsync(newGame.Key)).ReturnsAsync(true);
        
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() =>
            _gameService.UpdateAsync(newGame.Id, TestData.Game.GenerateGameRequestModel(newGame)));
    }
    
    [Test]
    public async Task UpdateAsync_WhenSpecifiedPublisherDoesNotExist_ShouldReturnFalse()
    {
        //Arrange
        var publishers = TestData.Publisher.GeneratePublisherEntities();
        var platforms = TestData.Platform.GeneratePlatformEntities();
        var genres = TestData.Genre.GenerateGenreEntities();
        var game = TestData.Game.GenerateGameEntity(publishers, genres, platforms); game.PublisherId = Guid.NewGuid();
        _gameRepository.Setup(x => x.GetByIdAsync(game.Id))
            .ReturnsAsync(game);
        SetUpMocksWhenPublisherDoesNotExist(genres, platforms, game.PublisherId);

        //Act
        var result = await _gameService.UpdateAsync(game.Id, TestData.Game.GenerateGameRequestModel(game));
        
        //Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task UpdateAsync_WhenSpecifiedGenreDoesNotExist_ShouldReturnFalse()
    {
        //Arrange
        var publishers = TestData.Publisher.GeneratePublisherEntities();
        var platforms = TestData.Platform.GeneratePlatformEntities();
        var genres = TestData.Genre.GenerateGenreEntities();
        var game = TestData.Game.GenerateGameEntity(publishers, genres, platforms);
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
    public async Task UpdateAsync_WhenSpecifiedPlatformDoesNotExist_ShouldReturnFalse()
    {
        //Arrange
        var publishers = TestData.Publisher.GeneratePublisherEntities();
        var platforms = TestData.Platform.GeneratePlatformEntities();
        var genres = TestData.Genre.GenerateGenreEntities();
        var game = TestData.Game.GenerateGameEntity(publishers, genres, platforms);
        _gameRepository.Setup(x => x.GetByIdAsync(game.Id))
            .ReturnsAsync(game);
        SetUpDefaultMocks(genres, platforms, game);
        
        //Act
        var result = await _gameService.UpdateAsync(game.Id, TestData.Game.GenerateGameRequestModel(game));
        
        //Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task UpdateAsync_WhenChangesNotSaved_ShouldReturnFalse()
    {
        // Arrange
        var publishers = TestData.Publisher.GeneratePublisherEntities();
        var platforms = TestData.Platform.GeneratePlatformEntities();
        var genres = TestData.Genre.GenerateGenreEntities();
        var game = TestData.Game.GenerateGameEntity(publishers, genres, platforms);
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
    public async Task CreateAsync_WhenChangesSavedSuccessfullyAndSpecifiedRelationsAreCorrect_ShouldAddGame()
    {
        //Arrange
        var publishers = TestData.Publisher.GeneratePublisherEntities();
        var platforms = TestData.Platform.GeneratePlatformEntities();
        var genres = TestData.Genre.GenerateGenreEntities();
        var game = TestData.Game.GenerateGameEntity(publishers, genres, platforms);
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
    public void CreateAsync_WhenNameAlreadyExists_ShouldThrowArgumentException()
    {
        // Arrange
        var publishers = TestData.Publisher.GeneratePublisherEntities();
        var platforms = TestData.Platform.GeneratePlatformEntities();
        var genres = TestData.Genre.GenerateGenreEntities();
        var game = TestData.Game.GenerateGameEntity(publishers, genres, platforms);
        
        _gameRepository.Setup(g => g.ExistsByNameAsync(game.Name)).ReturnsAsync(true);
    
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() => _gameService.CreateAsync(TestData.Game.GenerateGameRequestModel(game)));
    }
    
    [Test]
    public void CreateAsync_WhenKeyAlreadyExists_ShouldThrowArgumentException()
    {
        // Arrange
        var publishers = TestData.Publisher.GeneratePublisherEntities();
        var platforms = TestData.Platform.GeneratePlatformEntities();
        var genres = TestData.Genre.GenerateGenreEntities();
        var game = TestData.Game.GenerateGameEntity(publishers, genres, platforms);
        
        _gameRepository.Setup(g => g.ExistsByKeyAsync(game.Key)).ReturnsAsync(true);
    
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() => _gameService.CreateAsync(TestData.Game.GenerateGameRequestModel(game)));
    }

    [Test]
    public async Task CreateAsync_WhenSpecifiedPublisherDoesNotExist_ShouldReturnNull()
    {
        //Arrange
        var publishers = TestData.Publisher.GeneratePublisherEntities();
        var platforms = TestData.Platform.GeneratePlatformEntities();
        var genres = TestData.Genre.GenerateGenreEntities();
        var game = TestData.Game.GenerateGameEntity(publishers, genres, platforms);
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
    public async Task CreateAsync_WhenSpecifiedGenreDoesNotExist_ShouldReturnNull()
    {
        //Arrange
        var publishers = TestData.Publisher.GeneratePublisherEntities();
        var platforms = TestData.Platform.GeneratePlatformEntities();
        var genres = TestData.Genre.GenerateGenreEntities();
        var game = TestData.Game.GenerateGameEntity(publishers, genres, platforms);
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
    public async Task CreateAsync_WhenSpecifiedPlatformDoesNotExist_ShouldReturnNull()
    {
        //Arrange
        var publishers = TestData.Publisher.GeneratePublisherEntities();
        var platforms = TestData.Platform.GeneratePlatformEntities();
        var genres = TestData.Genre.GenerateGenreEntities();
        var game = TestData.Game.GenerateGameEntity(publishers, genres, platforms); game.GamePlatforms[0].PlatformId = Guid.NewGuid();
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
    public async Task CreateAsync_WhenChangesNotSaved_ShouldReturnNull()
    {
        //Arrange
        var publishers = TestData.Publisher.GeneratePublisherEntities();
        var platforms = TestData.Platform.GeneratePlatformEntities();
        var genres = TestData.Genre.GenerateGenreEntities();
        var game = TestData.Game.GenerateGameEntity(publishers, genres, platforms);
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
    public async Task DeleteAsync_WhenGameIsDeleted_ShouldCallDeleteAndSaveChanges()
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


    [Test]
    public async Task GetFilteredAsync_WhenNoQueryParamsProvided_ShouldReturnUpToTenGamesOrderedByNameAscOnFirstPage()
    {
        //Arrange
        var games = TestData.Game.GenerateGameEntities().OrderBy(g => g.Name).ToList();
        var paginated = games.Take(10).ToList();
        var gameFilter = new GameFilter();
        _gameRepository.Setup(x => x.GetFiltered(It.IsAny<DataAccess.Entities.Filtering.GameFilter>(), false))
            .ReturnsAsync((paginated, games.Count));
        var expected = paginated.Select(p => p.MapToGameDto()).ToList();
        
        //Act
        var (resultGames, resultCount) = await _gameService.GetFilteredAsync(gameFilter);
        
        //Assert
        Assert.That(resultCount, Is.EqualTo(games.Count));
        AssertThatResultListOfGamesIsEqualToExpectedList(resultGames.ToList(), expected);
    }
    
    [Test]
    public async Task GetFilteredAsync_WhenNameProvided_ShouldReturnGamesFilteredByName()
    {
        //Arrange
        var searchName = "cyber";
        var games = TestData.Game.GenerateGameEntities().Where(g => g.Name.ToLowerInvariant().Contains(searchName)).ToList();
        var paginated = games.Take(10).ToList();
        var gameFilter = new GameFilter { Name = searchName };
        _gameRepository.Setup(x => x.GetFiltered(It.IsAny<DataAccess.Entities.Filtering.GameFilter>(), false))
            .ReturnsAsync((paginated, games.Count));
        var expected = paginated.Select(p => p.MapToGameDto()).ToList();
        
        //Act
        var (resultGames, resultCount) = await _gameService.GetFilteredAsync(gameFilter);
        
        //Assert
        Assert.That(resultCount, Is.EqualTo(games.Count));
        AssertThatResultListOfGamesIsEqualToExpectedList(resultGames.ToList(), expected);
    }

    [Test]
    public async Task GetFilteredAsync_WhenNameNotFound_ShouldReturnEmptyList()
    {
        //Arrange
        var gameFilter = new GameFilter { Name = "NonExistingName" };
        _gameRepository.Setup(x => x.GetFiltered(It.IsAny<DataAccess.Entities.Filtering.GameFilter>(), false))
            .ReturnsAsync(([], 0));
        
        //Act
        var (resultGames, resultCount) = await _gameService.GetFilteredAsync(gameFilter);
        
        //Assert
        Assert.That(resultGames.ToList(), Is.Not.Null);
        Assert.That(resultCount, Is.EqualTo(0));
    }
    
    [Test]
    public async Task GetFilteredAsync_WhenMinAndMaxPriceProvided_ShouldReturnGamesWithinPriceRange()
    {
        //Arrange
        var minPrice = 30;
        var maxPrice = 60;
        var games = TestData.Game.GenerateGameEntities().Where(g => g.Price >= minPrice && g.Price <= maxPrice).ToList();
        var paginated = games.Take(10).ToList();
        var gameFilter = new GameFilter { MinPrice = minPrice, MaxPrice = maxPrice };
        _gameRepository.Setup(x => x.GetFiltered(It.IsAny<DataAccess.Entities.Filtering.GameFilter>(), false))
            .ReturnsAsync((paginated, games.Count));
        var expected = paginated.Select(p => p.MapToGameDto()).ToList();
        
        //Act
        var (resultGames, resultCount) = await _gameService.GetFilteredAsync(gameFilter);
        
        //Assert
        Assert.That(resultCount, Is.EqualTo(games.Count));
        AssertThatResultListOfGamesIsEqualToExpectedList(resultGames.ToList(), expected);
    }

    [Test]
    public async Task GetFilteredAsync_WhenOnlyMinPriceProvided_ShouldReturnGamesAboveMinPrice()
    {
        //Arrange
        var minPrice = 50;
        var games = TestData.Game.GenerateGameEntities().Where(g => g.Price >= minPrice).ToList();
        var gameFilter = new GameFilter { MinPrice = minPrice };
        var paginated = games.Take(10).ToList();
        _gameRepository.Setup(x => x.GetFiltered(It.IsAny<DataAccess.Entities.Filtering.GameFilter>(), false))
            .ReturnsAsync((paginated, games.Count));
        var expected = paginated.Select(p => p.MapToGameDto()).ToList();
        
        //Act
        var (resultGames, resultCount) = await _gameService.GetFilteredAsync(gameFilter);
        
        //Assert
        Assert.That(resultCount, Is.EqualTo(games.Count));
        AssertThatResultListOfGamesIsEqualToExpectedList(resultGames.ToList(), expected);
    }

    [Test]
    public async Task GetFilteredAsync_WhenOnlyMaxPriceProvided_ShouldReturnGamesBelowMaxPrice()
    {
        //Arrange
        var maxPrice = 40;
        var games = TestData.Game.GenerateGameEntities().Where(g => g.Price <= maxPrice).ToList();
        var paginated = games.Take(10).ToList();
        var gameFilter = new GameFilter { MaxPrice = maxPrice };
        _gameRepository.Setup(x => x.GetFiltered(It.IsAny<DataAccess.Entities.Filtering.GameFilter>(), false))
            .ReturnsAsync((paginated, games.Count));
        var expected = paginated.Select(p => p.MapToGameDto()).ToList();
        
        //Act
        var (resultGames, resultCount) = await _gameService.GetFilteredAsync(gameFilter);
        
        //Assert
        Assert.That(resultCount, Is.EqualTo(games.Count));
        AssertThatResultListOfGamesIsEqualToExpectedList(resultGames.ToList(), expected);
    }

    [Test]
    public async Task GetFilteredAsync_WhenRatingRangeProvided_ShouldReturnGamesWithinRatingRange()
    {
        //Arrange
        var minRating = 4.0;
        var maxRating = 4.8;
        var games = TestData.Game.GenerateGameEntities().Where(g => g.Rating >= minRating && g.Rating <= maxRating).ToList();
        var paginated = games.Take(10).ToList();
        var gameFilter = new GameFilter { RatingFrom = minRating, RatingTo = maxRating };
        _gameRepository.Setup(x => x.GetFiltered(It.IsAny<DataAccess.Entities.Filtering.GameFilter>(), false))
            .ReturnsAsync((paginated, games.Count));
        var expected = paginated.Select(p => p.MapToGameDto()).ToList();
        
        //Act
        var (resultGames, resultCount) = await _gameService.GetFilteredAsync(gameFilter);
        
        //Assert
        Assert.That(resultCount, Is.EqualTo(games.Count));
        AssertThatResultListOfGamesIsEqualToExpectedList(resultGames.ToList(), expected);
    }

    [Test]
    public async Task GetFilteredAsync_WhenAgeRatingsProvided_ShouldReturnGamesWithSpecificAgeRatings()
    {
        // Arrange
        List<AgeRating> ageRatings = [AgeRating.TwelvePlus, AgeRating.SixteenPlus];
        var games = TestData.Game.GenerateGameEntities().Where(g => ageRatings.Contains(g.AgeRating)).ToList();
        var gameFilter = new GameFilter { Age = [12, 16] };
        var paginated = games.Take(10).ToList();
        _gameRepository.Setup(x => x.GetFiltered(It.IsAny<DataAccess.Entities.Filtering.GameFilter>(), false))
            .ReturnsAsync((paginated, games.Count));
        var expected = paginated.Select(p => p.MapToGameDto()).ToList();
        
        // Act
        var (resultGames, resultCount) = await _gameService.GetFilteredAsync(gameFilter);
        
        // Assert
        Assert.That(resultCount, Is.EqualTo(games.Count));
        AssertThatResultListOfGamesIsEqualToExpectedList(resultGames.ToList(), expected);
    }

    [Test]
    public async Task GetFilteredAsync_WhenThreePlusAgeRatingProvided_ShouldReturnGamesForThreePlusAge()
    {
        // Arrange
        var ageRatings = new List<AgeRating> { AgeRating.ThreePlus };
        var games = TestData.Game.GenerateGameEntities().Where(g => ageRatings.Contains(g.AgeRating)).ToList();
        var gameFilter = new GameFilter { Age = [3] };
        var paginated = games.Take(10).ToList();
        _gameRepository.Setup(x => x.GetFiltered(It.IsAny<DataAccess.Entities.Filtering.GameFilter>(), false))
            .ReturnsAsync((paginated, games.Count));
        var expected = paginated.Select(p => p.MapToGameDto()).ToList();
        
        // Act
        var (resultGames, resultCount) = await _gameService.GetFilteredAsync(gameFilter);
        
        // Assert
        Assert.That(resultCount, Is.EqualTo(games.Count));
        AssertThatResultListOfGamesIsEqualToExpectedList(resultGames.ToList(), expected);
    }

    [Test]
    public async Task GetFilteredAsync_WhenReleaseDateRangeProvided_ShouldReturnGamesWithinDateRange()
    {
        // Arrange
        var startDate = new DateTime(2024, 1, 1);
        var endDate = new DateTime(2024, 6, 30);
        var games = TestData.Game.GenerateGameEntities().Where(g => g.ReleaseDate >= startDate && g.ReleaseDate <= endDate).ToList();
        var paginated = games.Take(10).ToList();
        var gameFilter = new GameFilter { ReleaseDateStart = startDate, ReleaseDateEnd = endDate };
        _gameRepository.Setup(x => x.GetFiltered(It.IsAny<DataAccess.Entities.Filtering.GameFilter>(), false))
            .ReturnsAsync((paginated, games.Count));
        var expected = paginated.Select(p => p.MapToGameDto()).ToList();
        
        // Act
        var (resultGames, resultCount) = await _gameService.GetFilteredAsync(gameFilter);
        
        // Assert
        Assert.That(resultCount, Is.EqualTo(games.Count));
        AssertThatResultListOfGamesIsEqualToExpectedList(resultGames.ToList(), expected);
    }

    [Test]
    public async Task GetFilteredAsync_WhenOnlyStartDateProvided_ShouldReturnGamesAfterStartDate()
    {
        // Arrange
        var startDate = new DateTime(2024, 5, 1);
        var games = TestData.Game.GenerateGameEntities().Where(g => g.ReleaseDate >= startDate).ToList();
        var paginated = games.Take(10).ToList();
        var gameFilter = new GameFilter { ReleaseDateStart = startDate };
        _gameRepository.Setup(x => x.GetFiltered(It.IsAny<DataAccess.Entities.Filtering.GameFilter>(), false))
            .ReturnsAsync((paginated, games.Count));
        var expected = paginated.Select(p => p.MapToGameDto()).ToList();   
        
        // Act
        var (resultGames, resultCount) = await _gameService.GetFilteredAsync(gameFilter);
        
        // Assert
        Assert.That(resultCount, Is.EqualTo(games.Count));
        AssertThatResultListOfGamesIsEqualToExpectedList(resultGames.ToList(), expected);
    }
    
    [Test]
    public async Task GetFilteredAsync_WhenPageNumberIsTwo_ShouldReturnSecondPageOfResults()
    {
        // Arrange
        var games = TestData.Game.GenerateGameEntities().OrderBy(g => g.Name).ToList();
        var paginated = games.Skip(10).Take(10).ToList();
        var gameFilter = new GameFilter { Page = 2, PageSize = 10 };
        _gameRepository.Setup(x => x.GetFiltered(It.IsAny<DataAccess.Entities.Filtering.GameFilter>(), false))
            .ReturnsAsync((paginated, games.Count));
        var expected = paginated.Select(p => p.MapToGameDto()).ToList();
        
        // Act
        var (resultGames, resultCount) = await _gameService.GetFilteredAsync(gameFilter);
        
        // Assert
        Assert.That(resultCount, Is.EqualTo(games.Count));
        AssertThatResultListOfGamesIsEqualToExpectedList(resultGames.ToList(), expected);
    }

    [Test]
    public async Task GetFilteredAsync_WhenPageSizeIsFive_ShouldReturnFiveGames()
    {
        // Arrange
        var games = TestData.Game.GenerateGameEntities();
        var paginated = games.Take(5).ToList();
        var gameFilter = new GameFilter { PageSize = 5 };
        _gameRepository.Setup(x => x.GetFiltered(It.IsAny<DataAccess.Entities.Filtering.GameFilter>(), false))
            .ReturnsAsync((paginated, games.Count));
        var expected = paginated.Select(p => p.MapToGameDto()).ToList();
        
        // Act
        var (resultGames, resultCount) = await _gameService.GetFilteredAsync(gameFilter);
        
        // Assert
        Assert.That(resultCount, Is.EqualTo(games.Count));
        AssertThatResultListOfGamesIsEqualToExpectedList(resultGames.ToList(), expected);
    }
    
    [Test]
    public async Task GetFilteredAsync_WhenSortByPriceAsc_ShouldReturnGamesOrderedByPriceAsc()
    {
        // Arrange
        var games = TestData.Game.GenerateGameEntities().OrderBy(g => g.Price).ToList();
        var paginated = games.Take(10).ToList();
        var gameFilter = new GameFilter { OrderBy = ["price"], Order = "asc" };
        _gameRepository.Setup(x => x.GetFiltered(It.IsAny<DataAccess.Entities.Filtering.GameFilter>(), false))
            .ReturnsAsync((paginated, games.Count));
        var expected = paginated.Select(p => p.MapToGameDto()).ToList();
        
        // Act
        var (resultGames, resultCount) = await _gameService.GetFilteredAsync(gameFilter);
        
        // Assert
        Assert.That(resultCount, Is.EqualTo(games.Count));
        AssertThatResultListOfGamesIsEqualToExpectedList(resultGames.ToList(), expected);
    }

    [Test]
    public async Task GetFilteredAsync_WhenSortByRatingDesc_ShouldReturnGamesOrderedByRatingDesc()
    {
        // Arrange
        var games = TestData.Game.GenerateGameEntities().OrderByDescending(g => g.Rating).ToList();
        var paginated = games.Take(10).ToList();
        var gameFilter = new GameFilter { OrderBy = ["rating"], Order = "desc" };
        _gameRepository.Setup(x => x.GetFiltered(It.IsAny<DataAccess.Entities.Filtering.GameFilter>(), false))
            .ReturnsAsync((paginated, games.Count));
        var expected = paginated.Select(p => p.MapToGameDto()).ToList();
        
        // Act
        var (resultGames, resultCount) = await _gameService.GetFilteredAsync(gameFilter);
        
        // Assert
        Assert.That(resultCount, Is.EqualTo(games.Count));
        AssertThatResultListOfGamesIsEqualToExpectedList(resultGames.ToList(), expected);
    }

    [Test]
    public async Task GetFilteredAsync_WhenSortByReleaseDateDesc_ShouldReturnGamesOrderedByReleaseDateDesc()
    {
        // Arrange
        var games = TestData.Game.GenerateGameEntities().OrderByDescending(g => g.ReleaseDate).ToList();
        var paginated = games.Take(10).ToList();
        var gameFilter = new GameFilter { OrderBy = ["release-date"], Order = "desc" };
        _gameRepository.Setup(x => x.GetFiltered(It.IsAny<DataAccess.Entities.Filtering.GameFilter>(), false))
            .ReturnsAsync((paginated, games.Count));
        var expected = paginated.Select(p => p.MapToGameDto()).ToList();
        
        // Act
        var (resultGames, resultCount) = await _gameService.GetFilteredAsync(gameFilter);
        
        // Assert
        Assert.That(resultCount, Is.EqualTo(games.Count));
        AssertThatResultListOfGamesIsEqualToExpectedList(resultGames.ToList(), expected);
    }
    
    [Test]
    public async Task GetFilteredAsync_WhenMultipleFiltersApplied_ShouldReturnFilteredAndSortedGames()
    {
        // Arrange
        var games = TestData.Game.GenerateGameEntities()
            .Where(g => g.Price is >= 30 and <= 60)
            .Where(g => g.Rating >= 4.0)
            .Where(g => g.AgeRating == AgeRating.TwelvePlus)
            .OrderByDescending(g => g.Rating)
            .ToList();
        var paginated = games.Take(5).ToList();

        
        var gameFilter = new GameFilter 
        { 
            MinPrice = 30m,
            MaxPrice = 60m,
            RatingFrom = 4.0,
            Age = [12],
            OrderBy =["rating"],
            Order = "desc",
            PageSize = 5
        };
        
        _gameRepository.Setup(x => x.GetFiltered(It.IsAny<DataAccess.Entities.Filtering.GameFilter>(), false))
            .ReturnsAsync((paginated, games.Count));
        var expected = paginated.Select(p => p.MapToGameDto()).ToList();
        
        // Act
        var (resultGames, resultCount) = await _gameService.GetFilteredAsync(gameFilter);
        
        // Assert
        Assert.That(resultCount, Is.EqualTo(games.Count));
        AssertThatResultListOfGamesIsEqualToExpectedList(resultGames.ToList(), expected);
    }
    
    [Test]
    public async Task GetFilteredAsync_WhenNoGamesMatchFilter_ShouldReturnEmptyList()
    {
        // Arrange
        var gameFilter = new GameFilter { MinPrice = 10000000 };
        _gameRepository.Setup(x => x.GetFiltered(It.IsAny<DataAccess.Entities.Filtering.GameFilter>(), false))
            .ReturnsAsync(([], 0));
        
        // Act
        var (resultGames, resultCount) = await _gameService.GetFilteredAsync(gameFilter);
        
        // Assert
        Assert.That(resultGames.ToList(), Is.Not.Null);
        Assert.That(resultCount, Is.EqualTo(0));
    }

    [Test]
    public async Task GetFilteredAsync_WhenUserIsUnderageAndNoQueryParams_ShouldReturnGamesFileteredByDefaultWithAgeRatingLessThan18()
    {
        // Arrange
        var hideAdultContent = true;
        var games = TestData.Game.GenerateGameEntitiesWithAgeRatingLessThan18().OrderBy(g => g.Name).Take(10).ToList();
        var paginated = games.ToList();
        var gameFilter = new GameFilter();
        
        _gameRepository.Setup(x => x.GetFiltered(It.IsAny<DataAccess.Entities.Filtering.GameFilter>(), hideAdultContent))
            .ReturnsAsync((paginated, games.Count));
        
        var expected = paginated.Select(p => p.MapToGameDto()).ToList();
        
        // Act
        var (resultGames, resultCount) = await _gameService.GetFilteredAsync(gameFilter, hideAdultContent);
        
        // Assert
        Assert.That(resultCount, Is.EqualTo(games.Count));
        AssertThatResultListOfGamesIsEqualToExpectedList(resultGames.ToList(), expected);
    }
    
    [Test]
    public async Task GetFilteredAsync_WhenUserIsUnderage_ShouldReturnEmptyListIfAskedForAgeRatingOf18()
    {
        // Arrange
        var hideAdultContent = true;
        var gameFilter = new GameFilter
        {
            Age = [18]
        };
        
        _gameRepository.Setup(x => x.GetFiltered(It.IsAny<DataAccess.Entities.Filtering.GameFilter>(), hideAdultContent))
            .ReturnsAsync(([], 0));
        
        // Act
        var (resultGames, resultCount) = await _gameService.GetFilteredAsync(gameFilter, hideAdultContent);
        
        // Assert
        Assert.That(resultCount, Is.EqualTo(0));
        Assert.That(resultGames.ToList(), Is.Empty);
    }
    
    [Test]
    public async Task GetFilteredAsync_WhenUserIsUnderageWithComplexFilter_ShouldReturnFilteredGamesExcludingAdultContent()
    {
        // Arrange
        var hideAdultContent = true;
        var minPrice = 25m;
        var maxPrice = 55m;
        var minRating = 4.0;
        var searchName = "space";
        var startDate = new DateTime(2024, 1, 1);
        var endDate = new DateTime(2024, 12, 31);
        
        var games = TestData.Game.GenerateGameEntitiesWithAgeRatingLessThan18()
            .Where(g => g.Price >= minPrice && g.Price <= maxPrice)
            .Where(g => g.Rating >= minRating)
            .Where(g => g.Name.ToLowerInvariant().Contains(searchName))
            .Where(g => g.ReleaseDate >= startDate && g.ReleaseDate <= endDate)
            .OrderBy(g => g.Name)
            .Take(10)
            .ToList();
        
        var gameFilter = new GameFilter
        {
            Name = searchName,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            RatingFrom = minRating,
            ReleaseDateStart = startDate,
            ReleaseDateEnd = endDate,
            Age = [3, 7, 12, 16, 18],
            OrderBy = ["name"],
            Order = "asc",
            PageSize = 10,
            Page = 1
        };
        
        _gameRepository.Setup(x => x.GetFiltered(It.IsAny<DataAccess.Entities.Filtering.GameFilter>(), hideAdultContent))
            .ReturnsAsync((games, games.Count));
        
        var expected = games.Select(p => p.MapToGameDto()).ToList();
        
        // Act
        var (resultGames, resultCount) = await _gameService.GetFilteredAsync(gameFilter, hideAdultContent);
        
        // Assert
        Assert.That(resultCount, Is.EqualTo(games.Count));
        AssertThatResultListOfGamesIsEqualToExpectedList(resultGames.ToList(), expected);
    }
}