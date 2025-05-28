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

    private static (List<DataAccess.Entities.Publisher>, List<DataAccess.Entities.Genre>, List<DataAccess.Entities.Platform>) GenerateRelations()
    {
        var publishers = new List<DataAccess.Entities.Publisher>
        {
            new() { Id = Guid.NewGuid(), Name = "Electronic Arts", Description = "American video game company", PageUrl = "https://www.ea.com" },
            new() { Id = Guid.NewGuid(), Name = "Ubisoft", Description = "French video game company", PageUrl = "https://www.ubisoft.com" },
            new() { Id = Guid.NewGuid(), Name = "Activision Blizzard", Description = "American video game holding company", PageUrl = "https://www.activisionblizzard.com" },
            new() { Id = Guid.NewGuid(), Name = "CD Projekt", Description = "Polish video game developer", PageUrl = "https://www.cdprojekt.com" },
            new() { Id = Guid.NewGuid(), Name = "Rockstar Games", Description = "American video game publisher", PageUrl = "https://www.rockstargames.com" },
            new() { Id = Guid.NewGuid(), Name = "Nintendo", Description = "Japanese multinational video game company", PageUrl = "https://www.nintendo.com" },
            new() { Id = Guid.NewGuid(), Name = "Valve Corporation", Description = "American video game developer", PageUrl = "https://www.valvesoftware.com" },
            new() { Id = Guid.NewGuid(), Name = "Bethesda Game Studios", Description = "American video game developer", PageUrl = "https://bethesdagamestudios.com" }
        };
        
        var genres = new List<DataAccess.Entities.Genre>
        {
            new() { Id = Guid.NewGuid(), Name = "Action" },
            new() { Id = Guid.NewGuid(), Name = "FPS" },
            new() { Id = Guid.NewGuid(), Name = "RPG" },
            new() { Id = Guid.NewGuid(), Name = "Strategy" },
            new() { Id = Guid.NewGuid(), Name = "Adventure" },
            new() { Id = Guid.NewGuid(), Name = "Racing" },
            new() { Id = Guid.NewGuid(), Name = "Sports" },
            new() { Id = Guid.NewGuid(), Name = "Simulation" },
            new() { Id = Guid.NewGuid(), Name = "Horror" },
            new() { Id = Guid.NewGuid(), Name = "Puzzle" }
        };
        
        var platforms = new List<DataAccess.Entities.Platform>
        {
            new() {Id = Guid.NewGuid(), Name = "PC", Description = "Personal Computer"},
            new() {Id = Guid.NewGuid(), Name = "PS5", Description = "PlayStation 5"},
            new() {Id = Guid.NewGuid(), Name = "Xbox Series X", Description = "Microsoft Xbox Series X"},
            new() {Id = Guid.NewGuid(), Name = "Nintendo Switch", Description = "Nintendo Switch Console"},
            new() {Id = Guid.NewGuid(), Name = "Steam Deck", Description = "Valve Steam Deck"}
        };
        
        return (publishers, genres, platforms);
    }

    private static List<DataAccess.Entities.Game> GenerateGameEntities()
    {
        var (publishers, genres, platforms) = GenerateRelations();
        
        var cyberWarriorsId = Guid.NewGuid();
        var mysticForestId = Guid.NewGuid();
        var spaceColonyId = Guid.NewGuid();
        var dragonsLegacyId = Guid.NewGuid();
        var streetRacerId = Guid.NewGuid();
        var fifaChampionsId = Guid.NewGuid();
        var cityArchitectId = Guid.NewGuid();
        var hauntedMansionId = Guid.NewGuid();
        var puzzleMasterId = Guid.NewGuid();
        var galacticWarfareId = Guid.NewGuid();
        var wildWestId = Guid.NewGuid();
        var cyberShooterId = Guid.NewGuid();
        
        var games = new List<DataAccess.Entities.Game>
        {
            new ()
            {
                Id = cyberWarriorsId,
                Name = "Cyber Warriors 2077",
                Key = "4uiru78rh6x84",
                Price = 59.99m,
                Picture = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A],
                Description = "A futuristic action RPG set in a dystopian cyberpunk world where you fight against corporate overlords.",
                Rating = 4.5,
                AgeRating = AgeRating.EighteenPlus,
                ReleaseDate = new DateTime(2023, 11, 15),
                PublisherId = publishers[0].Id,
                Publisher = publishers[0], 
                GamePlatforms =
                [
                    new() { GameId = cyberWarriorsId, PlatformId = platforms[0].Id, Platform = platforms[0] },
                    new() { GameId = cyberWarriorsId, PlatformId = platforms[1].Id, Platform = platforms[1] }
                ],
                GameGenres = [new() { GameId = cyberWarriorsId, GenreId = genres[0].Id, Genre = genres[0] }]
            },
            new ()
            {
                Id = mysticForestId,
                Name = "Mystic Forest Adventure",
                Key = "kuy32fe7367636872ey",
                Price = 29.99m,
                Description = "A magical journey through enchanted forests where you solve puzzles and befriend mystical creatures.",
                Rating = 4.2,
                AgeRating = AgeRating.ThreePlus,
                ReleaseDate = new DateTime(2024, 3, 8),
                PublisherId = publishers[1].Id,
                Publisher = publishers[1],
                GamePlatforms = [new() { GameId = mysticForestId, PlatformId = platforms[0].Id, Platform = platforms[0] }],
                GameGenres = [new() { GameId = mysticForestId, GenreId = genres[1].Id, Genre = genres[1] }]
            },
            new ()
            {
                Id = spaceColonyId,
                Name = "Space Colony Builder",
                Key = "35467568467987809807",
                Price = 39.99m,
                Description = "Build and manage your own space colony on distant planets while dealing with resource management and alien threats.",
                Rating = 4.7,
                AgeRating = AgeRating.TwelvePlus,
                ReleaseDate = new DateTime(2024, 1, 22),
                PublisherId = publishers[2].Id,
                Publisher = publishers[2],
                GamePlatforms = [new() { GameId = spaceColonyId, PlatformId = platforms[1].Id, Platform = platforms[1] }],
                GameGenres =
                [
                    new() { GameId = spaceColonyId, GenreId = genres[0].Id, Genre = genres[0] },
                    new() { GameId = spaceColonyId, GenreId = genres[1].Id, Genre = genres[1] }
                ]
            },
            new ()
            {
                Id = dragonsLegacyId,
                Name = "Dragon's Legacy",
                Key = "dragon-legacy-2024",
                Price = 69.99m,
                Picture = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0B],
                Description = "An epic fantasy RPG where you embark on a quest to save the realm from an ancient dragon threat.",
                Rating = 4.8,
                AgeRating = AgeRating.SixteenPlus,
                ReleaseDate = new DateTime(2024, 5, 12),
                PublisherId = publishers[3].Id,
                Publisher = publishers[3],
                GamePlatforms =
                [
                    new() { GameId = dragonsLegacyId, PlatformId = platforms[0].Id, Platform = platforms[0] },
                    new() { GameId = dragonsLegacyId, PlatformId = platforms[1].Id, Platform = platforms[1] },
                    new() { GameId = dragonsLegacyId, PlatformId = platforms[2].Id, Platform = platforms[2] }
                ],
                GameGenres = 
                [
                    new() { GameId = dragonsLegacyId, GenreId = genres[2].Id, Genre = genres[2] },
                    new() { GameId = dragonsLegacyId, GenreId = genres[4].Id, Genre = genres[4] }
                ]
            },
            new ()
            {
                Id = streetRacerId,
                Name = "Street Racer Ultimate",
                Key = "street-racer-ultimate",
                Price = 49.99m,
                Description = "High-octane street racing with customizable cars and underground tournaments.",
                Rating = 4.3,
                AgeRating = AgeRating.TwelvePlus,
                ReleaseDate = new DateTime(2024, 2, 14),
                PublisherId = publishers[0].Id,
                Publisher = publishers[0],
                GamePlatforms =
                [
                    new() { GameId = streetRacerId, PlatformId = platforms[0].Id, Platform = platforms[0] },
                    new() { GameId = streetRacerId, PlatformId = platforms[2].Id, Platform = platforms[2] }
                ],
                GameGenres = [new() { GameId = streetRacerId, GenreId = genres[5].Id, Genre = genres[5] }]
            },
            new ()
            {
                Id = fifaChampionsId,
                Name = "FIFA Champions 2025",
                Key = "fifa-champions-2025",
                Price = 59.99m,
                Picture = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0C],
                Description = "The ultimate football simulation with realistic gameplay and all your favorite teams.",
                Rating = 4.1,
                AgeRating = AgeRating.ThreePlus,
                ReleaseDate = new DateTime(2024, 9, 22),
                PublisherId = publishers[0].Id,
                Publisher = publishers[0],
                GamePlatforms =
                [
                    new() { GameId = fifaChampionsId, PlatformId = platforms[0].Id, Platform = platforms[0] },
                    new() { GameId = fifaChampionsId, PlatformId = platforms[1].Id, Platform = platforms[1] },
                    new() { GameId = fifaChampionsId, PlatformId = platforms[2].Id, Platform = platforms[2] }
                ],
                GameGenres = [new() { GameId = fifaChampionsId, GenreId = genres[6].Id, Genre = genres[6] }]
            },
            new ()
            {
                Id = cityArchitectId,
                Name = "City Architect Simulator",
                Key = "city-architect-sim",
                Price = 34.99m,
                Description = "Design and build the city of your dreams with advanced urban planning tools.",
                Rating = 4.6,
                AgeRating = AgeRating.SevenPlus,
                ReleaseDate = new DateTime(2024, 4, 18),
                PublisherId = publishers[1].Id,
                Publisher = publishers[1],
                GamePlatforms = [new() { GameId = cityArchitectId, PlatformId = platforms[0].Id, Platform = platforms[0] }],
                GameGenres = 
                [
                    new() { GameId = cityArchitectId, GenreId = genres[3].Id, Genre = genres[3] },
                    new() { GameId = cityArchitectId, GenreId = genres[7].Id, Genre = genres[7] }
                ]
            },
            new ()
            {
                Id = hauntedMansionId,
                Name = "Haunted Mansion Mystery",
                Key = "haunted-mansion-mystery",
                Price = 24.99m,
                Picture = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0D],
                Description = "Explore a cursed mansion and uncover dark secrets in this spine-chilling horror adventure.",
                Rating = 4.0,
                AgeRating = AgeRating.EighteenPlus,
                ReleaseDate = new DateTime(2024, 10, 31),
                PublisherId = publishers[7].Id,
                Publisher = publishers[7],
                GamePlatforms =
                [
                    new() { GameId = hauntedMansionId, PlatformId = platforms[0].Id, Platform = platforms[0] },
                    new() { GameId = hauntedMansionId, PlatformId = platforms[4].Id, Platform = platforms[4] }
                ],
                GameGenres = 
                [
                    new() { GameId = hauntedMansionId, GenreId = genres[8].Id, Genre = genres[8] },
                    new() { GameId = hauntedMansionId, GenreId = genres[4].Id, Genre = genres[4] }
                ]
            },
            new ()
            {
                Id = puzzleMasterId,
                Name = "Puzzle Master Collection",
                Key = "puzzle-master-collection",
                Price = 19.99m,
                Description = "A collection of challenging puzzles that will test your logical thinking and problem-solving skills.",
                Rating = 4.4,
                AgeRating = AgeRating.ThreePlus,
                ReleaseDate = new DateTime(2024, 6, 5),
                PublisherId = publishers[5].Id,
                Publisher = publishers[5],
                GamePlatforms =
                [
                    new() { GameId = puzzleMasterId, PlatformId = platforms[3].Id, Platform = platforms[3] },
                    new() { GameId = puzzleMasterId, PlatformId = platforms[0].Id, Platform = platforms[0] }
                ],
                GameGenres = [new() { GameId = puzzleMasterId, GenreId = genres[9].Id, Genre = genres[9] }]
            },

            new ()
            {
                Id = galacticWarfareId,
                Name = "Galactic Warfare",
                Key = "galactic-warfare-2024",
                Price = 54.99m,
                Picture = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0E],
                Description = "Command vast space fleets in epic battles across the galaxy in this real-time strategy masterpiece.",
                Rating = 4.7,
                AgeRating = AgeRating.TwelvePlus,
                ReleaseDate = new DateTime(2024, 7, 20),
                PublisherId = publishers[2].Id,
                Publisher = publishers[2],
                GamePlatforms =
                [
                    new() { GameId = galacticWarfareId, PlatformId = platforms[0].Id, Platform = platforms[0] },
                    new() { GameId = galacticWarfareId, PlatformId = platforms[4].Id, Platform = platforms[4] }
                ],
                GameGenres = 
                [
                    new() { GameId = galacticWarfareId, GenreId = genres[3].Id, Genre = genres[3] },
                    new() { GameId = galacticWarfareId, GenreId = genres[0].Id, Genre = genres[0] }
                ]
            },
            new ()
            {
                Id = wildWestId,
                Name = "Wild West Outlaws",
                Key = "wild-west-outlaws",
                Price = 59.99m,
                Description = "Live the life of an outlaw in the American frontier with horse riding, gunfights, and moral choices.",
                Rating = 4.9,
                AgeRating = AgeRating.EighteenPlus,
                ReleaseDate = new DateTime(2024, 8, 15),
                PublisherId = publishers[4].Id,
                Publisher = publishers[4],
                GamePlatforms =
                [
                    new() { GameId = wildWestId, PlatformId = platforms[0].Id, Platform = platforms[0] },
                    new() { GameId = wildWestId, PlatformId = platforms[1].Id, Platform = platforms[1] },
                    new() { GameId = wildWestId, PlatformId = platforms[2].Id, Platform = platforms[2] }
                ],
                GameGenres = 
                [
                    new() { GameId = wildWestId, GenreId = genres[0].Id, Genre = genres[0] },
                    new() { GameId = wildWestId, GenreId = genres[4].Id, Genre = genres[4] }
                ]
            },
            new ()
            {
                Id = cyberShooterId,
                Name = "Cyber Shooter Arena",
                Key = "cyber-shooter-arena",
                Price = 29.99m,
                Picture = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0F],
                Description = "Fast-paced multiplayer FPS set in a neon-lit cyberpunk world with advanced weaponry.",
                Rating = 4.2,
                AgeRating = AgeRating.SixteenPlus,
                ReleaseDate = new DateTime(2024, 12, 1),
                PublisherId = publishers[6].Id,
                Publisher = publishers[6],
                GamePlatforms =
                [
                    new() { GameId = cyberShooterId, PlatformId = platforms[0].Id, Platform = platforms[0] },
                    new() { GameId = cyberShooterId, PlatformId = platforms[4].Id, Platform = platforms[4] }
                ],
                GameGenres = 
                [
                    new() { GameId = cyberShooterId, GenreId = genres[1].Id, Genre = genres[1] },
                    new() { GameId = cyberShooterId, GenreId = genres[0].Id, Genre = genres[0] }
                ]
            }
        };
        return games;
    }

    private static DataAccess.Entities.Game GenerateSingleGameEntity(List<DataAccess.Entities.Publisher> publishers, List<DataAccess.Entities.Genre> genres, List<DataAccess.Entities.Platform> platforms, Guid? id = null)
    {
        var realId = id ?? Guid.NewGuid();
        return new()
        {
            Id = realId,
            Name = "Cyber Warriors 2077",
            Key = "4uiru78rh6x84",
            Price = 59.99m,
            Picture = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A],
            Description =
                "A futuristic action RPG set in a dystopian cyberpunk world where you fight against corporate overlords.",
            Rating = 4.5,
            AgeRating = AgeRating.EighteenPlus,
            ReleaseDate = new DateTime(2023, 11, 15),
            PublisherId = publishers[0].Id,
            Publisher = publishers[0],
            GamePlatforms =
            [
                new() { GameId = realId, PlatformId = platforms[0].Id, Platform = platforms[0] },
                new() { GameId = realId, PlatformId = platforms[1].Id, Platform = platforms[1] }
            ],
            GameGenres = [new() { GameId = realId, GenreId = genres[0].Id, Genre = genres[0] }]
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
            (int)game.AgeRating,
            game.ReleaseDate,
            game.PublisherId,
            game.GameGenres.Select(gg => gg.GenreId).ToList(),
            game.GamePlatforms.Select(gp => gp.PlatformId).ToList()
        );
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