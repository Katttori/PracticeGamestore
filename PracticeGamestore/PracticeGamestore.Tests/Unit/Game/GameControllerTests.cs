using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Services.Game;
using PracticeGamestore.Controllers;
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

    
    [Test]
    public async Task GetAll_ReturnsOkWithGames()
    {
        //Arrange
        var gameDtos = TestData.Game.GenerateGameResponseDtos();
        _gameService.Setup(x => x.GetAllAsync())
            .ReturnsAsync(gameDtos);
        var expected = gameDtos.Select(dto => dto.MapToGameModel()).ToList();

        //Act
        var result = await _gameController.GetAll();

        //Assert
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult!.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        var response = (okResult.Value as IEnumerable<GameResponseModel> ?? []).ToList();
        Assert.That(response, Is.Not.Null);
        Assert.That(response.Count, Is.EqualTo(expected.Count));
        var elementsAreTheSame = response.Zip(expected, GameResponseModelsAreTheSame).All(equal => equal);
        Assert.That(elementsAreTheSame, Is.True);
    }

    [Test]
    public async Task GetGameById_ShouldReturnOkResult_WhenGameExists()
    {
        //Arrange
        var id = Guid.NewGuid();
        var gameResponseDto = TestData.Game.GenerateGameResponseDto();
        _gameService.Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(gameResponseDto);
        var expected = gameResponseDto.MapToGameModel();

        //Act
        var result = await _gameController.GetById(id);

        //Assert
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult!.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
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
    public async Task CreateGame_ShouldReturnBadRequest_WhenCreationFails()
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
    public async Task CreateGame_ShouldReturnBadRequest_WhenProvidedAgeRatingIsIncorrect()
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
    public async Task Update_ShouldReturnNoContent_WhenGameIsUpdated()
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
    public async Task Update_ShouldReturnBadRequest_WhenUpdateFails()
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
    public async Task Update_ShouldReturnBadRequest_WhenProvidedAgeRatingIsIncorrect()
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
    public async Task Delete_ShouldReturnNoContent_WhenGameIsDeleted()
    {
        //Arrange
        _gameService.Setup(x => x.DeleteAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);
        
        //Act
        var result = await _gameController.Delete(Guid.NewGuid());
        
        //Assert
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }
}