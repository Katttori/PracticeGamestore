using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Services.Genre;
using PracticeGamestore.Business.Services.Location;
using PracticeGamestore.Controllers;
using PracticeGamestore.Models.Game;
using PracticeGamestore.Models.Genre;

namespace PracticeGamestore.Tests.Unit.Genre;

[TestFixture]
public class GenreControllerTests
{
    private Mock<IGenreService> _genreService;
    private Mock<ILocationService> _locationService;
    private Mock<IHttpContextAccessor> _httpContextAccessor;
    private Mock<ILogger<GenreController>> _loggerMock;
    private GenreController _genreController;

    [SetUp]
    public void Setup()
    {
        _genreService = new Mock<IGenreService>();
        _locationService = new Mock<ILocationService>();
        _httpContextAccessor = new Mock<IHttpContextAccessor>();
        _loggerMock = new Mock<ILogger<GenreController>>();
        _genreController = new GenreController(_genreService.Object, _locationService.Object, _httpContextAccessor.Object, _loggerMock.Object);
    }

    [Test]
    public async Task GetAll_ReturnsOkWithGenres()
    {
        // Arrange
        var genreDtos = TestData.Genre.GenerateGenreDtos();
        
        _genreService.Setup(x => x.GetAllAsync()).ReturnsAsync(genreDtos);
        
        // Act
        var result = await _genreController.GetAll();
        
        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        var responseModels = 
            (okResult?.Value as IEnumerable<GenreResponseModel> ?? Array.Empty<GenreResponseModel>()).ToList();
        Assert.That(responseModels.Count, Is.EqualTo(genreDtos.Count));
        Assert.That(responseModels.First().Id, Is.EqualTo(genreDtos.First().Id));
        Assert.That(responseModels.First().Name, Is.EqualTo(genreDtos.First().Name));
    }
    
    [Test]
    public async Task GetById_WhenGenreIsNull_ReturnsNotFound()
    {
        // Arrange
        _genreService.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as GenreDto);
        
        // Act
        var result = await _genreController.GetById(Guid.NewGuid());
        
        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }
    
    [Test]
    public async Task GetById_WhenGenreFound_ReturnsOkWithGenre()
    {
        // Arrange
        var genreDto = TestData.Genre.GenerateGenreDto();
        
        _genreService.Setup(x => x.GetByIdAsync(genreDto.Id!.Value)).ReturnsAsync(genreDto);
        
        // Act
        var result = await _genreController.GetById(genreDto.Id!.Value);
        
        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var receivedGenre = (result as OkObjectResult)?.Value as GenreResponseModel;
        Assert.That(receivedGenre, Is.Not.Null);
        Assert.That(receivedGenre?.Id, Is.EqualTo(genreDto.Id));
        Assert.That(receivedGenre?.Name, Is.EqualTo(genreDto.Name));
    }
    
    [Test]
    public async Task Create_WhenOperationFailed_ReturnsBadRequest()
    {
        // Arrange
        var model = TestData.Genre.GenerateGenreRequestModel();
        
        _genreService.Setup(x => x.CreateAsync(It.IsAny<GenreDto>())).ReturnsAsync(null as Guid?);
        
        // Act
        var result = await _genreController.Create(model);
        
        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }
    
    [Test]
    public async Task Create_WhenOperationSuccessful_ReturnsCreatedWithId()
    {
        // Arrange
        var newId = Guid.NewGuid();
        
        _genreService.Setup(x => x.CreateAsync(It.IsAny<GenreDto>())).ReturnsAsync(newId);
        
        // Act
        var result = await _genreController.Create(TestData.Genre.GenerateGenreRequestModel());
        
        // Assert
        Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
        var createdResult = result as CreatedAtActionResult;
        Assert.That(createdResult?.Value, Is.EqualTo(newId));
    }
    
    [Test]
    public async Task Update_WhenOperationSuccessful_ReturnsNoContent()
    {
        // Arrange
        _genreService.Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<GenreDto>())).ReturnsAsync(true);
        
        // Act
        var result = await _genreController.Update(Guid.NewGuid(), TestData.Genre.GenerateGenreRequestModel());
        
        // Assert
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }
    
    [Test]
    public async Task Update_WhenOperationFailed_ReturnsBadRequest()
    {
        // Arrange
        _genreService.Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<GenreDto>())).ReturnsAsync(false);
        
        // Act
        var result = await _genreController.Update(Guid.NewGuid(), TestData.Genre.GenerateGenreRequestModel());
        
        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }
    
    [Test]
    public async Task Delete_ReturnsNoContent()
    {
        // Arrange
        _genreService.Setup(x => x.DeleteAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);
        
        // Act
        var result = await _genreController.Delete(Guid.NewGuid());
        
        // Assert
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task GetGamesByGenre_ShouldReturnNotFound_WhenGenreDoesNotExist()
    {
        // Arrange
        _genreService.Setup(x => x.GetGames(It.IsAny<Guid>()))
            .ReturnsAsync(null as IEnumerable<GameResponseDto>);
        
        // Act
        var result = await _genreController.GetGamesByGenre(Guid.NewGuid());
        
        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }
    
    [Test]
    public async Task GetGamesByGenre_ShouldReturnGamesWithThisGenreOrItsChildren_WhenGenreExist()
    {
        // Arrange
        var actionGenreId = TestData.Genre.GenerateActionGenre().Id;
        var children = TestData.Genre.GenerateGenreChildren(actionGenreId);
        
        var games = TestData.Game.GenerateGameResponseDtos()
            .Where(game => game.Genres.Any(genre => children.Contains(genre.Id!.Value))).ToList();

        _genreService.Setup(x => x.GetGames(actionGenreId))
            .ReturnsAsync(games);
        
        // Act
        var result = await _genreController.GetGamesByGenre(actionGenreId);
        
        // Assert
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult!.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        var response = 
            (okResult.Value as IEnumerable<GameResponseModel> ?? Array.Empty<GameResponseModel>()).ToList();
        Assert.That(response, Is.Not.Null);
        Assert.That(response.All(g => g.Genres.Any(genre => children.Contains(genre.Id))), Is.True);
    }
    
}
