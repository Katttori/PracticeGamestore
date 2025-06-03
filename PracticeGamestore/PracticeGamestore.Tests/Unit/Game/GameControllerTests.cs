using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Filtering;
using PracticeGamestore.Business.Services.Game;
using PracticeGamestore.Controllers;
using PracticeGamestore.DataAccess.Enums;
using PracticeGamestore.Mappers;
using PracticeGamestore.Models.Game;

namespace PracticeGamestore.Tests.Unit.Game;

[TestFixture]
public class GameControllerTests
{
    private Mock<IGameService> _gameService;
    private GameController _gameController;

    [SetUp]
    public void SetUp()
    {
        _gameService = new Mock<IGameService>();
        _gameController = new GameController(_gameService.Object);
    }

    private static List<GameResponseDto> GenerateGameResponseDtos()
    {
        var gameResponseDtos = new List<GameResponseDto>
        {
            new (
                Guid.NewGuid(),
                "Cyber Warriors 2077",
                "4uiru78rh6x84",
                59.99m,
                [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A],
                "A futuristic action RPG set in a dystopian cyberpunk world where you fight against corporate overlords.",
                4.5,
                AgeRating.EighteenPlus,
                new DateTime(2023, 11, 15),
                new(Guid.NewGuid(), "Electronic Arts", "American video game company", "https://www.ea.com"),
                [new(Guid.NewGuid(), "PC", "Personal Computer"), new(Guid.NewGuid(), "PS5", "PlayStation 5")],
                [new(Guid.NewGuid(), "Action")]
            ),
            new (
                Guid.NewGuid(),
                "Mystic Forest Adventure",
                "kuy32fe7367636872ey",
                29.99m,
                null,
                "A magical journey through enchanted forests where you solve puzzles and befriend mystical creatures.",
                4.2,
                AgeRating.ThreePlus,
                new DateTime(2024, 3, 8),
                new(Guid.NewGuid(), "Ubisoft", "French video game company", "https://www.ubisoft.com"),
                [new(Guid.NewGuid(), "PC", "Personal Computer")],
                [new(Guid.NewGuid(), "FPS")]
            ),
            new (
                Guid.NewGuid(),
                "Space Colony Builder",
                "35467568467987809807",
                39.99m,
                null,
                "Build and manage your own space colony on distant planets while dealing with resource management and alien threats.",
                4.7,
                AgeRating.TwelvePlus,
                new DateTime(2024, 1, 22),
                new(Guid.NewGuid(), "Activision Blizzard", "American video game holding company", "https://www.activisionblizzard.com"),
                [new(Guid.NewGuid(), "PS5", "PlayStation 5")],
                [new(Guid.NewGuid(), "Action"), new(Guid.NewGuid(), "FPS")]
            ),
            new (
                Guid.NewGuid(),
                "Dragon's Legacy",
                "dragon-legacy-2024",
                69.99m,
                [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0B],
                "An epic fantasy RPG where you embark on a quest to save the realm from an ancient dragon threat.",
                4.8,
                AgeRating.SixteenPlus,
                new DateTime(2024, 5, 12),
                new(Guid.NewGuid(), "CD Projekt", "Polish video game developer", "https://www.cdprojekt.com"),
                [new(Guid.NewGuid(), "PC", "Personal Computer"), new(Guid.NewGuid(), "PS5", "PlayStation 5"), new(Guid.NewGuid(), "Xbox Series X", "Microsoft Xbox Series X")],
                [new(Guid.NewGuid(), "RPG"), new(Guid.NewGuid(), "Adventure")]
            ),
            new (
                Guid.NewGuid(),
                "Street Racer Ultimate",
                "street-racer-ultimate",
                49.99m,
                null,
                "High-octane street racing with customizable cars and underground tournaments.",
                4.3,
                AgeRating.TwelvePlus,
                new DateTime(2024, 2, 14),
                new(Guid.NewGuid(), "Electronic Arts", "American video game company", "https://www.ea.com"),
                [new(Guid.NewGuid(), "PC", "Personal Computer"), new(Guid.NewGuid(), "Xbox Series X", "Microsoft Xbox Series X")],
                [new(Guid.NewGuid(), "Racing")]
            ),
            new (
                Guid.NewGuid(),
                "FIFA Champions 2025",
                "fifa-champions-2025",
                59.99m,
                [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0C],
                "The ultimate football simulation with realistic gameplay and all your favorite teams.",
                4.1,
                AgeRating.ThreePlus,
                new DateTime(2024, 9, 22),
                new(Guid.NewGuid(), "Electronic Arts", "American video game company", "https://www.ea.com"),
                [new(Guid.NewGuid(), "PC", "Personal Computer"), new(Guid.NewGuid(), "PS5", "PlayStation 5"), new(Guid.NewGuid(), "Xbox Series X", "Microsoft Xbox Series X")],
                [new(Guid.NewGuid(), "Sports")]
            ),
            new (
                Guid.NewGuid(),
                "City Architect Simulator",
                "city-architect-sim",
                34.99m,
                null,
                "Design and build the city of your dreams with advanced urban planning tools.",
                4.6,
                AgeRating.SevenPlus,
                new DateTime(2024, 4, 18),
                new(Guid.NewGuid(), "Ubisoft", "French video game company", "https://www.ubisoft.com"),
                [new(Guid.NewGuid(), "PC", "Personal Computer")],
                [new(Guid.NewGuid(), "Strategy"), new(Guid.NewGuid(), "Simulation")]
            ),
            new (
                Guid.NewGuid(),
                "Haunted Mansion Mystery",
                "haunted-mansion-mystery",
                24.99m,
                [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0D],
                "Explore a cursed mansion and uncover dark secrets in this spine-chilling horror adventure.",
                4.0,
                AgeRating.EighteenPlus,
                new DateTime(2024, 10, 31),
                new(Guid.NewGuid(), "Bethesda Game Studios", "American video game developer", "https://bethesdagamestudios.com"),
                [new(Guid.NewGuid(), "PC", "Personal Computer"), new(Guid.NewGuid(), "Steam Deck", "Valve Steam Deck")],
                [new(Guid.NewGuid(), "Horror"), new(Guid.NewGuid(), "Adventure")]
            ),
            new (
                Guid.NewGuid(),
                "Puzzle Master Collection",
                "puzzle-master-collection",
                19.99m,
                null,
                "A collection of challenging puzzles that will test your logical thinking and problem-solving skills.",
                4.4,
                AgeRating.ThreePlus,
                new DateTime(2024, 6, 5),
                new(Guid.NewGuid(), "Nintendo", "Japanese multinational video game company", "https://www.nintendo.com"),
                [new(Guid.NewGuid(), "Nintendo Switch", "Nintendo Switch Console"), new(Guid.NewGuid(), "PC", "Personal Computer")],
                [new(Guid.NewGuid(), "Puzzle")]
            ),
            new (
                Guid.NewGuid(),
                "Galactic Warfare",
                "galactic-warfare-2024",
                54.99m,
                [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0E],
                "Command vast space fleets in epic battles across the galaxy in this real-time strategy masterpiece.",
                4.7,
                AgeRating.TwelvePlus,
                new DateTime(2024, 7, 20),
                new(Guid.NewGuid(), "Activision Blizzard", "American video game holding company", "https://www.activisionblizzard.com"),
                [new(Guid.NewGuid(), "PC", "Personal Computer"), new(Guid.NewGuid(), "Steam Deck", "Valve Steam Deck")],
                [new(Guid.NewGuid(), "Strategy"), new(Guid.NewGuid(), "Action")]
            ),
            new (
                Guid.NewGuid(),
                "Wild West Outlaws",
                "wild-west-outlaws",
                59.99m,
                null,
                "Live the life of an outlaw in the American frontier with horse riding, gunfights, and moral choices.",
                4.9,
                AgeRating.EighteenPlus,
                new DateTime(2024, 8, 15),
                new(Guid.NewGuid(), "Rockstar Games", "American video game publisher", "https://www.rockstargames.com"),
                [new(Guid.NewGuid(), "PC", "Personal Computer"), new(Guid.NewGuid(), "PS5", "PlayStation 5"), new(Guid.NewGuid(), "Xbox Series X", "Microsoft Xbox Series X")],
                [new(Guid.NewGuid(), "Action"), new(Guid.NewGuid(), "Adventure")]
            ),
            new (
                Guid.NewGuid(),
                "Cyber Shooter Arena",
                "cyber-shooter-arena",
                29.99m,
                [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0F],
                "Fast-paced multiplayer FPS set in a neon-lit cyberpunk world with advanced weaponry.",
                4.2,
                AgeRating.SixteenPlus,
                new DateTime(2024, 12, 1),
                new(Guid.NewGuid(), "Valve Corporation", "American video game developer", "https://www.valvesoftware.com"),
                [new(Guid.NewGuid(), "PC", "Personal Computer"), new(Guid.NewGuid(), "Steam Deck", "Valve Steam Deck")],
                [new(Guid.NewGuid(), "FPS"), new(Guid.NewGuid(), "Action")]
            )
        };
        return gameResponseDtos;
    }
    
    private static GameResponseDto GenerateSingleGameResponseDto()
    {
        return new(
            Guid.NewGuid(),
            "Cyber Warriors 2077",
            "4uiru78rh6x84",
            59.99m,
            [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A],
            "A futuristic action RPG set in a dystopian cyberpunk world where you fight against corporate overlords.",
            4.5,
            AgeRating.EighteenPlus,
            new DateTime(2023, 11, 15),
            new(Guid.NewGuid(), "Electronic Arts", "American video game company", "https://www.ea.com"),
            [new(Guid.NewGuid(), "PC", "Personal Computer"), new(Guid.NewGuid(), "PS5", "PlayStation 5")],
            [new(Guid.NewGuid(), "Action")]
        );
    }
    
    private static GameRequestModel GenerateSingleGameRequestModel()
    {
        return new()
        {
            Name = "Cyber Warriors 2077",
            Key = "4uiru78rh6x84",
            Price = 59.99m,
            Picture = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A],
            Description = "A futuristic action RPG set in a dystopian cyberpunk world where you fight against corporate overlords.",
            Rating = 4.5,
            AgeRating = 18,
            ReleaseDate = new DateTime(2023, 11, 15),
            PublisherId = Guid.NewGuid(),
            GenreIds = [Guid.NewGuid()],
            PlatformIds = [Guid.NewGuid(), Guid.NewGuid()]
        };
    }
    
    private static bool GameResponseModelsAreTheSame(GameResponseModel dto1, GameResponseModel dto2)
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

    private OkObjectResult AssetThatStatusCodeIsOk(IActionResult result)
    {
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult!.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        return okResult;
    }
    
    private static void AssertThatResultListOfGamesIsEqualToExpectedList(OkObjectResult okResult, List<GameResponseModel> expected)
    {
        var response = (okResult.Value as IEnumerable<GameResponseModel> ?? []).ToList();
        Assert.That(response, Is.Not.Null);
        Assert.That(response, Is.Not.Empty);
        Assert.That(response.Count, Is.EqualTo(expected.Count));
        var elementsAreTheSame = expected.Zip(response, GameResponseModelsAreTheSame).All(equal => equal);
        Assert.That(elementsAreTheSame, Is.True);
    }
    
    private static void AssertThatResultIsEqualToExpected(
        OkObjectResult okResult, 
        List<GameResponseModel> expectedGames,
        int expectedPageNumber,
        int expectedPageSize,
        int expectedCount)
    {
        var response = okResult.Value as PaginatedGameListResponseModel;
        Assert.That(response, Is.Not.Null);
    
        var games = response!.Games;
    
        Assert.That(games.Count, Is.EqualTo(expectedGames.Count));
        Assert.That(response.PageNumber, Is.EqualTo(expectedPageNumber));
        Assert.That(response.PageSize, Is.EqualTo(expectedPageSize));
        Assert.That(response.Count, Is.EqualTo(expectedCount));
    
        var elementsAreTheSame = expectedGames.Zip(games, GameResponseModelsAreTheSame).All(equal => equal);
        Assert.That(elementsAreTheSame, Is.True);
    }
    
    [Test]
    public async Task GetAll_ReturnsOkWithGames()
    {
        //Arrange
        var gameDtos = GenerateGameResponseDtos();
        _gameService.Setup(x => x.GetAllAsync())
            .ReturnsAsync(gameDtos);
        var expected = gameDtos.Select(dto => dto.MapToGameModel()).ToList();

        //Act
        var result = await _gameController.GetAll();

        //Assert
        var okResult = AssetThatStatusCodeIsOk(result);
        AssertThatResultListOfGamesIsEqualToExpectedList(okResult, expected);
    }

    [Test]
    public async Task GetGameById_ShouldReturnOkResult_WhenGameExists()
    {
        //Arrange
        var id = Guid.NewGuid();
        var gameResponseDto = GenerateSingleGameResponseDto();
        _gameService.Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(gameResponseDto);
        var expected = gameResponseDto.MapToGameModel();

        //Act
        var result = await _gameController.GetById(id);

        //Assert
        var okResult = AssetThatStatusCodeIsOk(result);
        var response = okResult.Value as GameResponseModel;
        Assert.That(response, Is.Not.Null);
        Assert.That(GameResponseModelsAreTheSame(response!, expected), Is.True);
    }

    [Test]
    public async Task GetGameById_ShouldReturnNotFound_WhenPublisherDoesNotExist()
    {
        //Arrange
        _gameService.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as GameResponseDto);

        //Act
        var result = await _gameController.GetById(new Guid());

        //Assert
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task CreateGame_ShouldReturnCreatedResult_WhenPublisherIsCreated()
    {
        //Arrange
        var gameRequestModel = GenerateSingleGameRequestModel();
        var id = Guid.NewGuid();
        _gameService.Setup(x => x.CreateAsync(It.IsAny<GameRequestDto>()))
            .ReturnsAsync(id);

        //Act
        var result = await _gameController.Create(gameRequestModel);

        //Assert
        var createdResult = result as CreatedAtActionResult;
        Assert.That(createdResult, Is.Not.Null);
        Assert.That(createdResult!.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
        Assert.That(createdResult.Value, Is.EqualTo(id));
        Assert.That(createdResult.ActionName, Is.EqualTo(nameof(GameController.GetById)));
        Assert.That(createdResult.RouteValues, Is.Not.Null);
        Assert.That(createdResult.RouteValues!["id"], Is.EqualTo(id));
    }

    [Test]
    public async Task CreateGame_ShouldReturnBadRequest_WhenCreationFails()
    {
        //Arrange
        var gameRequestModel = GenerateSingleGameRequestModel();
        _gameService.Setup(x => x.CreateAsync(It.IsAny<GameRequestDto>()))
            .ReturnsAsync(null as Guid?);

        //Act
        var result = await _gameController.Create(gameRequestModel);
        
        //Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }
    
    [Test]
    public async Task CreateGame_ShouldReturnBadRequest_WhenProvidedAgeRatingIsIncorrect()
    {
        //Arrange
        var gameRequestModel = GenerateSingleGameRequestModel();
        gameRequestModel.AgeRating = 88;

        //Act
        var result = await _gameController.Create(gameRequestModel);
        
        //Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task Update_ShouldReturnNoContent_WhenGameIsUpdated()
    {
        //Arrange
        var id = Guid.NewGuid();
        var gameRequestModel = GenerateSingleGameRequestModel();
        _gameService.Setup(x => x.UpdateAsync(id, It.IsAny<GameRequestDto>()))
            .ReturnsAsync(true);

        //Act
        var result = await _gameController.Update(id, gameRequestModel);
        
        //Assert
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task Update_ShouldReturnBadRequest_WhenUpdateFails()
    {
        //Arrange
        var id = Guid.NewGuid();
        var gameRequestModel = GenerateSingleGameRequestModel();
        _gameService.Setup(x => x.UpdateAsync(id, It.IsAny<GameRequestDto>()))
            .ReturnsAsync(false);

        //Act
        var result = await _gameController.Update(id, gameRequestModel);
        
        //Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }
    
    [Test]
    public async Task Update_ShouldReturnBadRequest_WhenProvidedAgeRatingIsIncorrect()
    {
        //Arrange
        var gameRequestModel = GenerateSingleGameRequestModel();
        gameRequestModel.AgeRating = 88;
        
        //Act
        var result = await _gameController.Update(Guid.NewGuid(), gameRequestModel);
        
        //Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task Delete_ShouldReturnNoContent_WhenGameIsDeleted()
    {
        //Arrange
        _gameService.Setup(x => x.DeleteAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);
        
        //Act
        var result = await _gameController.Delete(Guid.NewGuid());
        
        //Assert
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }
    
    [Test]
    public async Task GetFiltered_ShouldReturnOkWithFilteredGames_WhenValidFilterProvided()
    {
        // Arrange
        var gameFilter = new GameFilter 
        { 
            MinPrice = 30,
            MaxPrice = 60,
            Name = "cyber"
        };
        
        var gameDtos = GenerateGameResponseDtos().Where(g => g.Price is >= 30 and <= 60 && g.Name.Contains("cyber", StringComparison.InvariantCultureIgnoreCase)).ToList();
        var paginated = gameDtos.Take(10).ToList(); 
        _gameService.Setup(x => x.GetFilteredAsync(It.IsAny<GameFilter>()))
            .ReturnsAsync((paginated, gameDtos.Count));
        
        var expected = paginated.Select(dto => dto.MapToGameModel()).ToList();

        // Act
        var result = await _gameController.GetFiltered(gameFilter);

        // Assert
        var okResult = AssetThatStatusCodeIsOk(result);
        AssertThatResultIsEqualToExpected(okResult, expected, 1, 10, gameDtos.Count);
    }

    [Test]
    public async Task GetFiltered_ShouldReturnOkWithEmptyList_WhenNoGamesMatchFilter()
    {
        // Arrange
        var gameFilter = new GameFilter { MinPrice = 1000000 };
        _gameService.Setup(x => x.GetFilteredAsync(It.IsAny<GameFilter>()))
            .ReturnsAsync(([], 0));

        // Act
        var result = await _gameController.GetFiltered(gameFilter);

        // Assert
        var okResult = AssetThatStatusCodeIsOk(result);
        AssertThatResultIsEqualToExpected(okResult, [], 1, 10, 0);
    }

    [Test]
    public async Task GetFiltered_ShouldReturnOkAndGames_WhenValidOrderDirectionProvided()
    {
        // Arrange
        var gameFilter = new GameFilter { Order = "desc" };
        var gameDtos = GenerateGameResponseDtos().OrderBy(g => g.Name).ToList();
        var paginated = gameDtos.Take(10).ToList(); 
        _gameService.Setup(x => x.GetFilteredAsync(It.IsAny<GameFilter>()))
            .ReturnsAsync((paginated, gameDtos.Count));
        
        var expected = paginated.Select(dto => dto.MapToGameModel()).ToList();

        // Act
        var result = await _gameController.GetFiltered(gameFilter);

        // Assert
        var okResult = AssetThatStatusCodeIsOk(result);
        AssertThatResultIsEqualToExpected(okResult, expected, 1, 10, gameDtos.Count);
    }

    [Test]
    public async Task GetFiltered_ShouldReturnOkWithGames_WhenValidOrderByFieldsProvided()
    {
        // Arrange
        var gameFilter = new GameFilter { OrderBy = ["price", "rating"] };
        var gameDtos = GenerateGameResponseDtos().OrderByDescending(g => g.Rating).ToList();
        var paginated = gameDtos.Take(10).ToList(); 
        _gameService.Setup(x => x.GetFilteredAsync(It.IsAny<GameFilter>()))
            .ReturnsAsync((paginated, gameDtos.Count));
        var expected = paginated.Select(dto => dto.MapToGameModel()).ToList();

        // Act
        var result = await _gameController.GetFiltered(gameFilter);

        // Assert
        var okResult = AssetThatStatusCodeIsOk(result);
        AssertThatResultIsEqualToExpected(okResult, expected, 1, 10, gameDtos.Count);
    }

    [Test]
    public async Task GetFiltered_ShouldReturnOkWithGames_WhenValidAgeRatingsProvided()
    {
        // Arrange
        var gameFilter = new GameFilter { Age = [AgeRating.TwelvePlus, AgeRating.SixteenPlus] };
        var gameDtos = GenerateGameResponseDtos().Where(g => g.AgeRating is AgeRating.TwelvePlus or AgeRating.SixteenPlus).ToList();
        var paginated = gameDtos.Take(10).ToList(); 
        _gameService.Setup(x => x.GetFilteredAsync(It.IsAny<GameFilter>()))
            .ReturnsAsync((paginated, gameDtos.Count));
        var expected = paginated.Select(dto => dto.MapToGameModel()).ToList();

        // Act
        var result = await _gameController.GetFiltered(gameFilter);

        // Assert
        var okResult = AssetThatStatusCodeIsOk(result);
        AssertThatResultIsEqualToExpected(okResult, expected, 1, 10, gameDtos.Count);
    }

    [Test]
    public async Task GetFiltered_ShouldReturnOk_WhenValidDateRangeProvided()
    {
        // Arrange
        var gameFilter = new GameFilter 
        { 
            ReleaseDateStart = new DateTime(2024, 1, 1),
            ReleaseDateEnd = new DateTime(2024, 12, 31)
        };
        var gameDtos = GenerateGameResponseDtos().Where(g => g.ReleaseDate >= new DateTime(2024, 1, 1) && g.ReleaseDate <= new DateTime(2024, 12, 31)).ToList();
        var paginated = gameDtos.Take(10).ToList(); 
        _gameService.Setup(x => x.GetFilteredAsync(It.IsAny<GameFilter>()))
            .ReturnsAsync((paginated, gameDtos.Count));
        var expected = paginated.Select(dto => dto.MapToGameModel()).ToList();

        // Act
        var result = await _gameController.GetFiltered(gameFilter);

        // Assert
        var okResult = AssetThatStatusCodeIsOk(result);
        AssertThatResultIsEqualToExpected(okResult, expected, 1, 10, gameDtos.Count);
    }

    [Test]
    public async Task GetFiltered_ShouldReturnOk_WhenValidPaginationProvided()
    {
        // Arrange
        var gameFilter = new GameFilter { Page = 2, PageSize = 5 };
        var gameDtos = GenerateGameResponseDtos().ToList();
        var paginated = gameDtos.Skip(5).Take(5).ToList(); 
        _gameService.Setup(x => x.GetFilteredAsync(It.IsAny<GameFilter>()))
            .ReturnsAsync((paginated, gameDtos.Count));
        var expected = paginated.Select(dto => dto.MapToGameModel()).ToList();

        // Act
        var result = await _gameController.GetFiltered(gameFilter);

        // Assert
        var okResult = AssetThatStatusCodeIsOk(result);
        AssertThatResultIsEqualToExpected(okResult, expected, 2, 5, gameDtos.Count);
    }

    [Test]
    public async Task GetFiltered_ShouldReturnOk_WhenComplexValidFilterProvided()
    {
        // Arrange
        var gameFilter = new GameFilter 
        { 
            Name = "cyber",
            MinPrice = 20,
            MaxPrice = 70,
            RatingFrom = 4.0,
            RatingTo = 5.0,
            Age = [AgeRating.SixteenPlus, AgeRating.EighteenPlus],
            ReleaseDateStart = new DateTime(2023, 1, 1),
            ReleaseDateEnd = new DateTime(2024, 12, 31),
            OrderBy = ["rating", "price"],
            Order = "desc",
            Page = 1,
            PageSize = 10
        };
        var gameDtos = GenerateGameResponseDtos()
            .Where(g => g.Name.Contains("cyber", StringComparison.InvariantCultureIgnoreCase) && 
                        g.Price is >= 20 and <= 70 && 
                        g.Rating is >= 4.0 and <= 5.0 &&
                        g.AgeRating is AgeRating.SixteenPlus or AgeRating.EighteenPlus &&
                        g.ReleaseDate >= new DateTime(2023, 1, 1) && g.ReleaseDate <= new DateTime(2024, 12, 31))
            .OrderByDescending(g => g.Rating)
            .ThenByDescending(g => g.Price)
            .ToList();
        var paginated = gameDtos.Take(10).ToList(); 
        _gameService.Setup(x => x.GetFilteredAsync(It.IsAny<GameFilter>()))
            .ReturnsAsync((paginated, gameDtos.Count));
        var expected = paginated.Select(dto => dto.MapToGameModel()).ToList();
        
        // Act
        var result = await _gameController.GetFiltered(gameFilter);

        // Assert
        var okResult = AssetThatStatusCodeIsOk(result);
        AssertThatResultIsEqualToExpected(okResult, expected, 1, 10, gameDtos.Count);
    }
        
    [TestCase(-10, null, TestName = "GetFiltered_ShouldReturnBadRequest_WhenMinPriceIsNegative")]
    [TestCase(null, -5, TestName = "GetFiltered_ShouldReturnBadRequest_WhenMaxPriceIsNegative")]
    [TestCase(100, 50, TestName = "GetFiltered_ShouldReturnBadRequest_WhenMinPriceIsGreaterThanMaxPrice")]
    public async Task GetFiltered_ShouldReturnBadRequest_WhenInvalidPriceRange(decimal? minPrice, decimal? maxPrice)
    {
        // Arrange
        var gameFilter = new GameFilter { MinPrice = minPrice, MaxPrice = maxPrice};

        // Act
        var result = await _gameController.GetFiltered(gameFilter);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [TestCase(-1, null, TestName = "GetFiltered_ShouldReturnBadRequest_WhenRatingFromIsNegative")]
    [TestCase(6, null, TestName = "GetFiltered_ShouldReturnBadRequest_WhenRatingFromIsGreaterThanFive")]
    [TestCase(null, -0.5, TestName = "GetFiltered_ShouldReturnBadRequest_WhenRatingToIsNegative")]
    [TestCase(null, 7.5, TestName = "GetFiltered_ShouldReturnBadRequest_WhenRatingToIsGreaterThanFive")]
    [TestCase(4.5, 3.0, TestName = "GetFiltered_ShouldReturnBadRequest_WhenRatingFromIsGreaterThanRatingTo")]
    public async Task GetFiltered_ShouldReturnBadRequest_WhenInvalidRatingRange(double? ratingFrom, double? ratingTo)
    {
        // Arrange
        var gameFilter = new GameFilter { RatingFrom = ratingFrom, RatingTo = ratingTo};

        // Act
        var result = await _gameController.GetFiltered(gameFilter);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [TestCase("2024-12-31", "2024-01-01", TestName = "GetFiltered_ShouldReturnBadRequest_WhenStartDateAfterEndDate")]
    [TestCase("2025-01-01", "2024-12-31", TestName = "GetFiltered_ShouldReturnBadRequest_WhenStartDateInFuture")]
    public async Task GetFiltered_ShouldReturnBadRequest_WhenInvalidDateRange(string startDateStr, string endDateStr)
    {
        // Arrange
        var gameFilter = new GameFilter 
        { 
            ReleaseDateStart = DateTime.Parse(startDateStr),
            ReleaseDateEnd =  DateTime.Parse(endDateStr)
        };

        // Act
        var result = await _gameController.GetFiltered(gameFilter);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task GetFiltered_ShouldReturnBadRequest_WhenInvalidOrderDirectionProvided()
    {
        // Arrange
        var gameFilter = new GameFilter { Order = "invalid" };

        // Act
        var result = await _gameController.GetFiltered(gameFilter);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task GetFiltered_ShouldReturnBadRequest_WhenInvalidOrderByFields()
    {
        // Arrange
        var gameFilter = new GameFilter { OrderBy = ["invalid-field"] };

        // Act
        var result = await _gameController.GetFiltered(gameFilter);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task CreateGame_ShouldReturnBadRequest_WhenInvalidAgeRating()
    {
        // Arrange
        var gameFilter = new GameFilter { Age = [(AgeRating)999] };

        // Act
        var result = await _gameController.GetFiltered(gameFilter);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [TestCase(-1, null, TestName = "GetFiltered_ShouldReturnBadRequest_WhenPageIsNegative")]
    [TestCase(null, -5, TestName = "GetFiltered_ShouldReturnBadRequest_WhenPageSizeIsNegative")]
    public async Task GetFiltered_ShouldReturnBadRequest_WhenInvalidPagination(int? page, int? pageSize)
    { 
        // Arrange
        var gameFilter = new GameFilter { Page = page, PageSize = pageSize };

        // Act
        var result = await _gameController.GetFiltered(gameFilter);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }
}