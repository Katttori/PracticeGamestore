using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.DataTransferObjects.Filtering;
using PracticeGamestore.Business.Services.Game;
using PracticeGamestore.Business.Services.HeaderHandle;
using PracticeGamestore.Controllers;
using PracticeGamestore.DataAccess.Enums;
using PracticeGamestore.Mappers;
using PracticeGamestore.Models.Game;

namespace PracticeGamestore.Tests.Unit.Game;

public class GameControllerTests
{
    private Mock<IGameService> _gameService;
    private Mock<IHeaderHandleService> _headerHandleService;
    private Mock<ILogger<GameController>> _loggerMock;
    private GameController _gameController;
    
    private const string CountryHeader = "Ukraine";
    private const string UserEmailHeader = "test@gmail.com";

    [SetUp]
    public void SetUp()
    {
        _gameService = new Mock<IGameService>();
        _headerHandleService = new Mock<IHeaderHandleService>();
        _loggerMock = new Mock<ILogger<GameController>>();
        _gameController = new GameController(_gameService.Object,  _headerHandleService.Object, _loggerMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext() 
                }
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

    private static OkObjectResult AssertThatStatusCodeIsOk(IActionResult result)
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
    public async Task GetAll_ShouldReturnOkWithAllGamesIfUserIsAdult()
    {
        //Arrange
        var hideAdultContent = false;
        var gameDtos = TestData.Game.GenerateGameResponseDtos();
        _gameService.Setup(x => x.GetAllAsync(hideAdultContent))
            .ReturnsAsync(gameDtos);
        var expected = gameDtos.Select(dto => dto.MapToGameModel()).ToList();
        _gameController.ControllerContext.HttpContext.Items[HttpContextCustomItems.UnderageIndicator] = hideAdultContent;
        
        //Act
        var result = await _gameController.GetAll(CountryHeader, UserEmailHeader);

        //Assert
        var okResult = AssertThatStatusCodeIsOk(result);
        AssertThatResultListOfGamesIsEqualToExpectedList(okResult, expected);
    }
    
    [Test]
    public async Task GetAll_ShouldReturnOkWithGamesOfAgeRatingLessThan18IfUserIsUnderage()
    {
        //Arrange
        var hideAdultContent = true;
        var gameDtos = TestData.Game.GenerateGameResponseDtosWithAgeRatingLessThan18();
        _gameService.Setup(x => x.GetAllAsync(hideAdultContent))
            .ReturnsAsync(gameDtos);
        var expected = gameDtos.Select(dto => dto.MapToGameModel()).ToList();
        _gameController.ControllerContext.HttpContext.Items[HttpContextCustomItems.UnderageIndicator] = hideAdultContent;
        
        //Act
        var result = await _gameController.GetAll(CountryHeader, UserEmailHeader);

        //Assert
        var okResult = AssertThatStatusCodeIsOk(result);
        AssertThatResultListOfGamesIsEqualToExpectedList(okResult, expected);
    }

    [Test]
    public async Task GetGameById_WhenGameExists_ShouldReturnOkResult()
    {
        //Arrange
        var id = Guid.NewGuid();
        var gameResponseDto = TestData.Game.GenerateGameResponseDto();
        _gameService.Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(gameResponseDto);
        var expected = gameResponseDto.MapToGameModel();

        //Act
        var result = await _gameController.GetById(CountryHeader, UserEmailHeader, id);

        //Assert
        var okResult = AssertThatStatusCodeIsOk(result);
        var response = okResult.Value as GameResponseModel;
        Assert.That(response, Is.Not.Null);
        Assert.That(GameResponseModelsAreTheSame(response!, expected), Is.True);
    }
    
    [Test]
    public async Task GetGameById_WhenGameDoesNotExist_ShouldReturnNotFound()
    {
        //Arrange
        _gameService.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as GameResponseDto);

        //Act
        var result = await _gameController.GetById(CountryHeader, UserEmailHeader, Guid.NewGuid());

        //Assert
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }
    
    [Test]
    public async Task GetGameById_WhenGameHasAgeRatingEighteenPlusAndUserIsUnderage_ShouldReturnNotFound()
    {
        //Arrange
        var game = TestData.Game.GenerateGameResponseDto();
        game.AgeRating = AgeRating.EighteenPlus;
        _gameService.Setup(x => x.GetByIdAsync(game.Id))
            .ReturnsAsync(game);
        _gameController.ControllerContext.HttpContext.Items["Underage"] = true;

        //Act
        var result = await _gameController.GetById(CountryHeader, UserEmailHeader, game.Id);

        //Assert
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task CreateGame_WhenGameIsCreated_ShouldReturnCreatedResult()
    {
        //Arrange
        var gameRequestModel = TestData.Game.GenerateGameRequestModel();
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
    public async Task CreateGame_WhenCreationFails_ShouldReturnBadRequest()
    {
        //Arrange
        var gameRequestModel = TestData.Game.GenerateGameRequestModel();
        _gameService.Setup(x => x.CreateAsync(It.IsAny<GameRequestDto>()))
            .ReturnsAsync(null as Guid?);

        //Act
        var result = await _gameController.Create(gameRequestModel);
        
        //Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }
    
    [Test]
    public async Task CreateGame_WhenProvidedAgeRatingIsIncorrect_ShouldReturnBadRequest()
    {
        //Arrange
        var gameRequestModel = TestData.Game.GenerateGameRequestModel();
        gameRequestModel.AgeRating = 88;

        //Act
        var result = await _gameController.Create(gameRequestModel);
        
        //Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task Update_WhenGameIsUpdated_ShouldReturnNoContent()
    {
        //Arrange
        var id = Guid.NewGuid();
        var gameRequestModel = TestData.Game.GenerateGameRequestModel();
        _gameService.Setup(x => x.UpdateAsync(id, It.IsAny<GameRequestDto>()))
            .ReturnsAsync(true);

        //Act
        var result = await _gameController.Update(id, gameRequestModel);
        
        //Assert
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task Update_WhenUpdateFails_ShouldReturnBadRequest()
    {
        //Arrange
        var id = Guid.NewGuid();
        var gameRequestModel = TestData.Game.GenerateGameRequestModel();
        _gameService.Setup(x => x.UpdateAsync(id, It.IsAny<GameRequestDto>()))
            .ReturnsAsync(false);

        //Act
        var result = await _gameController.Update(id, gameRequestModel);
        
        //Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }
    
    [Test]
    public async Task Update_WhenProvidedAgeRatingIsIncorrect_ShouldReturnBadRequest()
    {
        //Arrange
        var gameRequestModel = TestData.Game.GenerateGameRequestModel();
        gameRequestModel.AgeRating = 88;
        
        //Act
        var result = await _gameController.Update(Guid.NewGuid(), gameRequestModel);
        
        //Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task Delete_WhenGameIsDeleted_ShouldReturnNoContent()
    {
        //Arrange
        _gameService.Setup(x => x.DeleteAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);
        
        //Act
        var result = await _gameController.Delete(Guid.NewGuid());
        
        //Assert
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }
    
    [Test]
    public async Task GetFiltered_WhenValidFilterProvided_ShouldReturnOkWithFilteredGames()
    {
        // Arrange
        var gameFilter = new GameFilter 
        { 
            MinPrice = 30,
            MaxPrice = 60,
            Name = "cyber"
        };
        
        var gameDtos = TestData.Game.GenerateGameResponseDtos().Where(g => g.Price is >= 30 and <= 60 && g.Name.Contains("cyber", StringComparison.InvariantCultureIgnoreCase)).ToList();
        var paginated = gameDtos.Take(10).ToList(); 
        _gameService.Setup(x => x.GetFilteredAsync(It.IsAny<GameFilter>(), false))
            .ReturnsAsync((paginated, gameDtos.Count));
        
        var expected = paginated.Select(dto => dto.MapToGameModel()).ToList();

        // Act
        var result = await _gameController.GetFiltered(CountryHeader, UserEmailHeader, gameFilter);

        // Assert
        var okResult = AssertThatStatusCodeIsOk(result);
        AssertThatResultIsEqualToExpected(okResult, expected, 1, 10, gameDtos.Count);
    }

    [Test]
    public async Task GetFiltered_WhenNoGamesMatchFilter_ShouldReturnOkWithEmptyList()
    {
        // Arrange
        var gameFilter = new GameFilter { MinPrice = 1000000 };
        _gameService.Setup(x => x.GetFilteredAsync(It.IsAny<GameFilter>(), false))
            .ReturnsAsync(([], 0));

        // Act
        var result = await _gameController.GetFiltered(CountryHeader, UserEmailHeader, gameFilter);

        // Assert
        var okResult = AssertThatStatusCodeIsOk(result);
        AssertThatResultIsEqualToExpected(okResult, [], 1, 10, 0);
    }

    [Test]
    public async Task GetFiltered_WhenValidOrderDirectionProvided_ShouldReturnOkAndGames()
    {
        // Arrange
        var gameFilter = new GameFilter { Order = "desc" };
        var gameDtos = TestData.Game.GenerateGameResponseDtos().OrderBy(g => g.Name).ToList();
        var paginated = gameDtos.Take(10).ToList(); 
        _gameService.Setup(x => x.GetFilteredAsync(It.IsAny<GameFilter>(), false))
            .ReturnsAsync((paginated, gameDtos.Count));
        
        var expected = paginated.Select(dto => dto.MapToGameModel()).ToList();

        // Act
        var result = await _gameController.GetFiltered(CountryHeader, UserEmailHeader, gameFilter);

        // Assert
        var okResult = AssertThatStatusCodeIsOk(result);
        AssertThatResultIsEqualToExpected(okResult, expected, 1, 10, gameDtos.Count);
    }

    [Test]
    public async Task GetFiltered_WhenValidOrderByFieldsProvided_ShouldReturnOkWithGames()
    {
        // Arrange
        var gameFilter = new GameFilter { OrderBy = ["price", "rating"] };
        var gameDtos = TestData.Game.GenerateGameResponseDtos().OrderByDescending(g => g.Rating).ToList();
        var paginated = gameDtos.Take(10).ToList(); 
        _gameService.Setup(x => x.GetFilteredAsync(It.IsAny<GameFilter>(), false))
            .ReturnsAsync((paginated, gameDtos.Count));
        var expected = paginated.Select(dto => dto.MapToGameModel()).ToList();

        // Act
        var result = await _gameController.GetFiltered(CountryHeader, UserEmailHeader, gameFilter);

        // Assert
        var okResult = AssertThatStatusCodeIsOk(result);
        AssertThatResultIsEqualToExpected(okResult, expected, 1, 10, gameDtos.Count);
    }

    [Test]
    public async Task GetFiltered_WhenValidAgeRatingsProvided_ShouldReturnOkWithGames()
    {
        // Arrange
        var gameFilter = new GameFilter { Age = [12, 16] };
        var gameDtos = TestData.Game.GenerateGameResponseDtos().Where(g => g.AgeRating is AgeRating.TwelvePlus or AgeRating.SixteenPlus).ToList();
        var paginated = gameDtos.Take(10).ToList(); 
        _gameService.Setup(x => x.GetFilteredAsync(It.IsAny<GameFilter>(), false))
            .ReturnsAsync((paginated, gameDtos.Count));
        var expected = paginated.Select(dto => dto.MapToGameModel()).ToList();

        // Act
        var result = await _gameController.GetFiltered(CountryHeader, UserEmailHeader, gameFilter);

        // Assert
        var okResult = AssertThatStatusCodeIsOk(result);
        AssertThatResultIsEqualToExpected(okResult, expected, 1, 10, gameDtos.Count);
    }

    [Test]
    public async Task GetFiltered_WhenValidDateRangeProvided_ShouldReturnOk()
    {
        // Arrange
        var gameFilter = new GameFilter 
        { 
            ReleaseDateStart = new DateTime(2024, 1, 1),
            ReleaseDateEnd = new DateTime(2024, 12, 31)
        };
        var gameDtos = TestData.Game.GenerateGameResponseDtos().Where(g =>
            g.ReleaseDate >= new DateTime(2024, 1, 1) && g.ReleaseDate <= new DateTime(2024, 12, 31)).ToList();
        var paginated = gameDtos.Take(10).ToList(); 
        _gameService.Setup(x => x.GetFilteredAsync(It.IsAny<GameFilter>(), false))
            .ReturnsAsync((paginated, gameDtos.Count));
        var expected = paginated.Select(dto => dto.MapToGameModel()).ToList();

        // Act
        var result = await _gameController.GetFiltered(CountryHeader, UserEmailHeader, gameFilter);

        // Assert
        var okResult = AssertThatStatusCodeIsOk(result);
        AssertThatResultIsEqualToExpected(okResult, expected, 1, 10, gameDtos.Count);
    }

    [Test]
    public async Task GetFiltered_WhenValidPaginationProvided_ShouldReturnOk()
    {
        // Arrange
        var gameFilter = new GameFilter { Page = 2, PageSize = 5 };
        var gameDtos = TestData.Game.GenerateGameResponseDtos().ToList();
        var paginated = gameDtos.Skip(5).Take(5).ToList(); 
        _gameService.Setup(x => x.GetFilteredAsync(It.IsAny<GameFilter>(), false))
            .ReturnsAsync((paginated, gameDtos.Count));
        var expected = paginated.Select(dto => dto.MapToGameModel()).ToList();

        // Act
        var result = await _gameController.GetFiltered(CountryHeader, UserEmailHeader, gameFilter);

        // Assert
        var okResult = AssertThatStatusCodeIsOk(result);
        AssertThatResultIsEqualToExpected(okResult, expected, 2, 5, gameDtos.Count);
    }

    [Test]
    public async Task GetFiltered_WhenComplexValidFilterProvided_ShouldReturnOk()
    {
        // Arrange
        var gameFilter = new GameFilter 
        { 
            Name = "cyber",
            MinPrice = 20,
            MaxPrice = 70,
            RatingFrom = 4.0,
            RatingTo = 5.0,
            Age = [16, 18],
            ReleaseDateStart = new DateTime(2023, 1, 1),
            ReleaseDateEnd = new DateTime(2024, 12, 31),
            OrderBy = ["rating", "price"],
            Order = "desc",
            Page = 1,
            PageSize = 10
        };
        var gameDtos = TestData.Game.GenerateGameResponseDtos()
            .Where(g => g.Name.Contains("cyber", StringComparison.InvariantCultureIgnoreCase) && 
                        g.Price is >= 20 and <= 70 && 
                        g.Rating is >= 4.0 and <= 5.0 &&
                        g.AgeRating is AgeRating.SixteenPlus or AgeRating.EighteenPlus &&
                        g.ReleaseDate >= new DateTime(2023, 1, 1) && g.ReleaseDate <= new DateTime(2024, 12, 31))
            .OrderByDescending(g => g.Rating)
            .ThenByDescending(g => g.Price)
            .ToList();
        var paginated = gameDtos.Take(10).ToList(); 
        _gameService.Setup(x => x.GetFilteredAsync(It.IsAny<GameFilter>(), false))
            .ReturnsAsync((paginated, gameDtos.Count));
        var expected = paginated.Select(dto => dto.MapToGameModel()).ToList();
        
        // Act
        var result = await _gameController.GetFiltered(CountryHeader, UserEmailHeader, gameFilter);

        // Assert
        var okResult = AssertThatStatusCodeIsOk(result);
        AssertThatResultIsEqualToExpected(okResult, expected, 1, 10, gameDtos.Count);
    }

    [Test]
    public async Task GetFiltered_WhenUserIsUnderage_ShouldReturnOkWithFilteredGamesExcludingAdultContent()
    {
        // Arrange
        var hideAdultContent = true;
        var gameFilter = new GameFilter 
        { 
            MinPrice = 25,
            MaxPrice = 55,
            Name = "space"
        };
        
        var gameDtos = TestData.Game.GenerateGameResponseDtosWithAgeRatingLessThan18()
            .Where(g => g.Price is >= 25 and <= 55 && g.Name.Contains("space", StringComparison.InvariantCultureIgnoreCase))
            .ToList();
        var paginated = gameDtos.Take(10).ToList(); 
        _gameService.Setup(x => x.GetFilteredAsync(It.IsAny<GameFilter>(), hideAdultContent))
            .ReturnsAsync((paginated, gameDtos.Count));
        
        var expected = paginated.Select(dto => dto.MapToGameModel()).ToList();
        _gameController.ControllerContext.HttpContext.Items[HttpContextCustomItems.UnderageIndicator] = hideAdultContent;

        // Act
        var result = await _gameController.GetFiltered(CountryHeader, UserEmailHeader, gameFilter);

        // Assert
        var okResult = AssertThatStatusCodeIsOk(result);
        AssertThatResultIsEqualToExpected(okResult, expected, 1, 10, gameDtos.Count);
    }

    [Test]
    public async Task GetFiltered_WhenFilteringForAdultOnlyContentAndUserIsUnderage_ShouldReturnEmptyList()
    {
        // Arrange
        var hideAdultContent = true;
        var gameFilter = new GameFilter 
        { 
            Age = [18]
        };
        
        _gameService.Setup(x => x.GetFilteredAsync(It.IsAny<GameFilter>(), hideAdultContent))
            .ReturnsAsync(([], 0));
        
        _gameController.ControllerContext.HttpContext.Items[HttpContextCustomItems.UnderageIndicator] = hideAdultContent;

        // Act
        var result = await _gameController.GetFiltered(CountryHeader, UserEmailHeader, gameFilter);

        // Assert
        var okResult = AssertThatStatusCodeIsOk(result);
        var response = okResult.Value as PaginatedGameListResponseModel;
        Assert.That(response, Is.Not.Null);
        Assert.That(response, Is.Empty);
    }
    
    [Test]
    public async Task GetFiltered_WhenUserIsUnderageWithComplexFilter_ShouldReturnFilteredGamesSortedByPriceDesc()
    {
        // Arrange
        var hideAdultContent = true;
        var gameFilter = new GameFilter 
        { 
            RatingFrom = 4.0,
            Age = [12, 16, 18],
            OrderBy = ["price"],
            Order = "desc",
            PageSize = 5,
            Page = 1
        };
    
        var gameDtos = TestData.Game.GenerateGameResponseDtosWithAgeRatingLessThan18()
            .Where(g => g.Rating >= 4.0 && (g.AgeRating == AgeRating.TwelvePlus || g.AgeRating == AgeRating.SixteenPlus))
            .OrderByDescending(g => g.Price)
            .ToList();
        var paginated = gameDtos.Take(5).ToList(); 
        _gameService.Setup(x => x.GetFilteredAsync(It.IsAny<GameFilter>(), hideAdultContent))
            .ReturnsAsync((paginated, gameDtos.Count));
    
        var expected = paginated.Select(dto => dto.MapToGameModel()).ToList();
        _gameController.ControllerContext.HttpContext.Items[HttpContextCustomItems.UnderageIndicator] = hideAdultContent;

        // Act
        var result = await _gameController.GetFiltered(CountryHeader, UserEmailHeader, gameFilter);

        // Assert
        var okResult = AssertThatStatusCodeIsOk(result);
        AssertThatResultIsEqualToExpected(okResult, expected, 1, 5, gameDtos.Count);
    }
}