using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Enums;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.Business.Services.Game;
using PracticeGamestore.DataAccess.Entities;
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

    private static (List<Publisher>, List<DataAccess.Entities.Genre>, List<Platform>) GenerateRelations()
    {
        var publishers = new List<Publisher>
        {
            new() { Id = Guid.NewGuid(), Name = "Electronic Arts", Description = "American video game company", PageUrl = "https://www.ea.com" },
            new() { Id = Guid.NewGuid(), Name = "Ubisoft", Description = "French video game company", PageUrl = "https://www.ubisoft.com" },
            new() { Id = Guid.NewGuid(), Name = "Activision Blizzard", Description = "American video game holding company", PageUrl = "https://www.activisionblizzard.com" },
        };
        var genres = new List<DataAccess.Entities.Genre>
        {
            new() { Id = Guid.NewGuid(), Name = "Action" },
            new() { Id = Guid.NewGuid(), Name = "FPS" },
        };
        var platforms = new List<Platform>
        {
            new() {Id = Guid.NewGuid(), Name = "PC", Description = "Personal Computer"},
            new() {Id = Guid.NewGuid(), Name = "PS5", Description = "PlayStation 5"}
        };
        return (publishers, genres, platforms);
    }

    private static List<DataAccess.Entities.Game> GenerateGameEntities()
    {
        var (publishers, genres, platforms) = GenerateRelations();
        var games = new List<DataAccess.Entities.Game>
        {
            new ()
            {
                Id = Guid.NewGuid(),
                Name = "Cyber Warriors 2077",
                Key = "4uiru78rh6x84",
                Price = 59.99m,
                Picture = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A],
                Description = "A futuristic action RPG set in a dystopian cyberpunk world where you fight against corporate overlords.",
                Rating = 4.5,
                AgeRating = (int)AgeRating.EighteenPlus,
                ReleaseDate = new DateTime(2023, 11, 15),
                PublisherId = publishers[0].Id,
                Publisher = publishers[0], 
                GamePlatforms =
                [
                    new() { GameId = Guid.NewGuid(), PlatformId = platforms[0].Id, Platform = platforms[0] },
                    new() { GameId = Guid.NewGuid(), PlatformId = platforms[1].Id, Platform = platforms[1] }
                ],
                GameGenres = [new() { GameId = Guid.NewGuid(), GenreId = genres[0].Id, Genre = genres[0] }]
            },
            new ()
            {
                Id = Guid.NewGuid(),
                Name = "Mystic Forest Adventure",
                Key = "kuy32fe7367636872ey",
                Price = 29.99m,
                Description = "A magical journey through enchanted forests where you solve puzzles and befriend mystical creatures.",
                Rating = 4.2,
                AgeRating = (int)AgeRating.ThreePlus,
                ReleaseDate = new DateTime(2024, 3, 8),
                PublisherId = publishers[1].Id,
                Publisher = publishers[1],
                GamePlatforms = [new() { GameId = Guid.NewGuid(), PlatformId = platforms[0].Id, Platform = platforms[0] }],
                GameGenres = [new() { GameId = Guid.NewGuid(), GenreId = genres[1].Id, Genre = genres[1] }]
            },
            new ()
            {
                Id = Guid.NewGuid(),
                Name = "Space Colony Builder",
                Key = "35467568467987809807",
                Price = 39.99m,
                Description = "Build and manage your own space colony on distant planets while dealing with resource management and alien threats.",
                Rating = 4.7,
                AgeRating = (int)AgeRating.TwelvePlus,
                ReleaseDate = new DateTime(2024, 1, 22),
                PublisherId = publishers[2].Id,
                Publisher = publishers[2],
                GamePlatforms = [new() { GameId = Guid.NewGuid(), PlatformId = platforms[1].Id, Platform = platforms[1] }],
                GameGenres =
                [
                    new() { GameId = Guid.NewGuid(), GenreId = genres[0].Id, Genre = genres[0] },
                    new() { GameId = Guid.NewGuid(), GenreId = genres[1].Id, Genre = genres[1] }
                ]
            }
        };
        return games;
    }

    private static DataAccess.Entities.Game GenerateSingleGameEntity(List<Publisher> publishers, List<DataAccess.Entities.Genre> genres, List<Platform> platforms, Guid? id = null)
    { 
        return new()
        {
            Id = id ?? Guid.NewGuid(),
            Name = "Cyber Warriors 2077",
            Key = "4uiru78rh6x84",
            Price = 59.99m,
            Picture = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A],
            Description =
                "A futuristic action RPG set in a dystopian cyberpunk world where you fight against corporate overlords.",
            Rating = 4.5,
            AgeRating = (int)AgeRating.EighteenPlus,
            ReleaseDate = new DateTime(2023, 11, 15),
            PublisherId = publishers[0].Id,
            Publisher = publishers[0],
            GamePlatforms =
            [
                new() { GameId = Guid.NewGuid(), PlatformId = platforms[0].Id, Platform = platforms[0] },
                new() { GameId = Guid.NewGuid(), PlatformId = platforms[1].Id, Platform = platforms[1] }
            ],
            GameGenres = [new() { GameId = Guid.NewGuid(), GenreId = genres[0].Id, Genre = genres[0] }]
        };
    }
    
    private static GameRequestDto GenerateGameRequestModel(DataAccess.Entities.Game game)
    { 
        return new GameRequestDto(
            game.Id,
            game.Name,
            game.Key,
            game.Price,
            game.Picture,
            game.Description,
            game.Rating,
            (Business.Enums.AgeRating)game.AgeRating,
            game.ReleaseDate,
            game.PublisherId,
            game.GameGenres.Select(gg => gg.GenreId).ToList(),
            game.GamePlatforms.Select(gp => gp.PlatformId).ToList()
        );
    }

    private void SetUpDefaultMocks(IEnumerable<DataAccess.Entities.Genre> genres, IEnumerable<Platform> platforms, DataAccess.Entities.Game game)
    {
        _publisherRepository.Setup(x => x.GetByIdAsync(game.PublisherId))
            .ReturnsAsync(game.Publisher);
        _genreRepository.Setup(x => x.GetAllAsync())
            .ReturnsAsync(genres);
        _platformRepository.Setup(x => x.GetAllAsync())
            .ReturnsAsync(platforms);
    }
    
    private void SetUpMocksWhenPublisherDoesNotExist(IEnumerable<DataAccess.Entities.Genre> genres, IEnumerable<Platform> platforms, Guid publisherId)
    {
        _publisherRepository.Setup(x => x.GetByIdAsync(publisherId))
            .ReturnsAsync(null as Publisher);
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
        var games = GenerateGameEntities();
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
        var (publishers, genres, platforms) = GenerateRelations();
        var game = GenerateSingleGameEntity(publishers, genres, platforms);
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
        var (publishers, genres, platforms) = GenerateRelations();
        var game = GenerateSingleGameEntity(publishers, genres, platforms);
        _gameRepository.Setup(x => x.GetByIdAsync(game.Id))
            .ReturnsAsync(game);
        SetUpDefaultMocks(genres, platforms, game);
        _unitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        
        //Act
        var result = await _gameService.UpdateAsync(GenerateGameRequestModel(game));
        
        //Assert
        Assert.That(result, Is.True);
    }
    
    [Test]
    public async Task UpdateAsync_WhenGameDoesNotExist_ReturnsFalse()
    {
        //Arrange
        var (publishers, genres, platforms) = GenerateRelations();
        var game = GenerateSingleGameEntity(publishers, genres, platforms);   
        _gameRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as DataAccess.Entities.Game);

        //Act
        var result = await _gameService.UpdateAsync(GenerateGameRequestModel(game));
        
        //Assert
        Assert.That(result, Is.False);
    }
    
    [Test]
    public async Task UpdateAsync_WhenSpecifiedPublisherDoesNotExist_ReturnsFalse()
    {
        //Arrange
        var (publishers, genres, platforms) = GenerateRelations();
        var game = GenerateSingleGameEntity(publishers, genres, platforms);
        game.PublisherId = Guid.NewGuid();
        _gameRepository.Setup(x => x.GetByIdAsync(game.Id))
            .ReturnsAsync(game);
        SetUpMocksWhenPublisherDoesNotExist(genres, platforms, game.PublisherId);

        //Act
        var result = await _gameService.UpdateAsync(GenerateGameRequestModel(game));
        
        //Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task UpdateAsync_WhenSpecifiedGenreDoesNotExist_ReturnsFalse()
    {
        //Arrange
        var (publishers, genres, platforms) = GenerateRelations();
        var game = GenerateSingleGameEntity(publishers, genres, platforms);
        game.GameGenres[0].GenreId = Guid.NewGuid();
        _gameRepository.Setup(x => x.GetByIdAsync(game.Id))
            .ReturnsAsync(game);
        SetUpDefaultMocks(genres, platforms, game);

        //Act
        var result = await _gameService.UpdateAsync(GenerateGameRequestModel(game));
        
        //Assert
        Assert.That(result, Is.False);
    }
    
    [Test]
    public async Task UpdateAsync_WhenSpecifiedPlatformDoesNotExist_ReturnsFalse()
    {
        //Arrange
        var (publishers, genres, platforms) = GenerateRelations();
        var game = GenerateSingleGameEntity(publishers, genres, platforms);
        _gameRepository.Setup(x => x.GetByIdAsync(game.Id))
            .ReturnsAsync(game);
        SetUpDefaultMocks(genres, platforms, game);
        
        //Act
        var result = await _gameService.UpdateAsync(GenerateGameRequestModel(game));
        
        //Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task UpdateAsync_ShouldReturnFalse_WhenChangesNotSaved()
    {
        // Arrange
        var (publishers, genres, platforms) = GenerateRelations();
        var game = GenerateSingleGameEntity(publishers, genres, platforms);
        _gameRepository.Setup(x => x.GetByIdAsync(game.Id))
            .ReturnsAsync(game);
        SetUpDefaultMocks(genres, platforms, game);
        _unitOfWork
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);
        
        // Act
        var result = await _gameService.UpdateAsync(GenerateGameRequestModel(game));
        
        // Assert
        Assert.That(result, Is.False);
    }
    
    [Test]
    public async Task CreateAsync_ShouldAddGame_WhenChangesSavedSuccessfullyAndSpecifiedRealtionsAreCorrect()
    {
        //Arrange
        var (publishers, genres, platforms) = GenerateRelations();
        var game = GenerateSingleGameEntity(publishers, genres, platforms);
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
        var result = await _gameService.CreateAsync(GenerateGameRequestModel(game));

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(game.Id));
    }

    [Test]
    public async Task CreateAsync_ShouldReturnNull_WhenSpecifiedPublisherDoesNotExist()
    {
        //Arrange
        var (publishers, genres, platforms) = GenerateRelations();
        var game = GenerateSingleGameEntity(publishers, genres, platforms);
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
        var result = await _gameService.CreateAsync(GenerateGameRequestModel(game));

        // Assert
        Assert.That(result, Is.Null);
    }
    
    [Test]
    public async Task CreateAsync_ShouldReturnNull_WhenSpecifiedGenreDoesNotExist()
    {
        //Arrange
        var (publishers, genres, platforms) = GenerateRelations();
        var game = GenerateSingleGameEntity(publishers, genres, platforms);
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
        var result = await _gameService.CreateAsync(GenerateGameRequestModel(game));

        // Assert
        Assert.That(result, Is.Null);
    }
    
    [Test]
    public async Task CreateAsync_ShouldReturnNull_WhenSpecifiedPlatformDoesNotExist()
    {
        //Arrange
        var (publishers, genres, platforms) = GenerateRelations();
        var game = GenerateSingleGameEntity(publishers, genres, platforms);
        game.GamePlatforms[0].PlatformId = Guid.NewGuid();
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
        var result = await _gameService.CreateAsync(GenerateGameRequestModel(game));

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task CreateAsync_ShouldReturnNull_WhenChangesNotSaved()
    {
        //Arrange
        var (publishers, genres, platforms) = GenerateRelations();
        var game = GenerateSingleGameEntity(publishers, genres, platforms);
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
        var result = await _gameService.CreateAsync(GenerateGameRequestModel(game));
        
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