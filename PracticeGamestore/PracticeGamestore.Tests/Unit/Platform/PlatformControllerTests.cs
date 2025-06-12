using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Services.Game;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.Business.Services.Platform;
using PracticeGamestore.Controllers;
using PracticeGamestore.Models.Game;
using PracticeGamestore.Models.Platform;

namespace PracticeGamestore.Tests.Unit.Platform;

[TestFixture]
public class PlatformControllerTests
{
    private Mock<IPlatformService> _platformService;
    private Mock<IGameService> _gameService;
    private Mock<ILogger<PlatformController>> _loggerMock;
    private PlatformController _platformController;
    
    [SetUp]
    public void Setup()
    {
        _platformService = new Mock<IPlatformService>();
        _gameService = new Mock<IGameService>();
        _loggerMock = new Mock<ILogger<PlatformController>>();
        _platformController = new PlatformController(_platformService.Object, _gameService.Object, _loggerMock.Object)   {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() 
            }
        };;
    }
    
    [Test]
    public async Task GetAllPlatforms_ShouldReturnOkResult_WhenPlatformsExist()
    {
        // Arrange
        var platforms = TestData.Platform.GeneratePlatformDtos();
        
        _platformService.Setup(service => service.GetAllAsync()).ReturnsAsync(platforms);
        
        // Act
        var result = await _platformController.GetAllPlatforms();
        
        // Assert
        var okResult = result as OkObjectResult;
        
        var response = (okResult?.Value as IEnumerable<PlatformResponseModel> 
                        ?? []).ToList();
        
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult?.StatusCode, Is.EqualTo(200));
        Assert.That(response.Count, Is.EqualTo(platforms.Count));
        Assert.That(response[0].Id, Is.EqualTo(platforms[0].Id));
        Assert.That(response[0].Name, Is.EqualTo(platforms[0].Name));
    }
    
    [Test]
    public async Task GetPlatformById_ShouldReturnOkResult_WhenPlatformExists()
    {
        // Arrange
        var platform = TestData.Platform.GeneratePlatformDto();
        
        _platformService.Setup(service => service.GetByIdAsync(platform.MapToPlatformEntity().Id)).ReturnsAsync(platform);
        
        // Act
        var result = await _platformController.GetPlatformById(platform.MapToPlatformEntity().Id);
        
        // Assert
        var okResult = result as OkObjectResult;
        
        var response = okResult?.Value as PlatformResponseModel;
        
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult?.StatusCode, Is.EqualTo(200));
        Assert.That(response?.Id, Is.EqualTo(platform.Id));
        Assert.That(response?.Name, Is.EqualTo(platform.Name));
    }
    
    [Test]
    public async Task GetPlatformById_ShouldReturnNotFound_WhenPlatformDoesNotExist()
    {
        // Arrange
        var platformId = Guid.NewGuid();
        
        _platformService.Setup(service => service.GetByIdAsync(platformId)).ReturnsAsync((PlatformDto?)null);
        
        // Act
        var result = await _platformController.GetPlatformById(platformId);
        
        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }
    
    [Test]
    public async Task GetGamesByPlatformAsync_WhenUserIsAdult_ShouldReturnOkWithAllGames()
    {
        // Arrange
        var platformId = Guid.NewGuid();
        var hideAdultContent = false;
        var mockGames = TestData.Game.GenerateGameResponseDtos();
        _platformService.Setup(s => s.GetGamesAsync(platformId, hideAdultContent)).ReturnsAsync(mockGames);
        _platformController.ControllerContext.HttpContext.Items["Underage"] = hideAdultContent;

        // Act
        var result = await _platformController.GetGamesByPlatform(platformId);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        var responseModels = (okResult?.Value as IEnumerable<GameResponseModel> ?? []).ToList();
        Assert.That(responseModels.Count, Is.EqualTo(mockGames.Count));
    }
    
    [Test]
    public async Task GetGamesByPlatformAsync_WhenUserIsUnderage_ShouldReturnOkWithGamesOfAgeRatingLessThan18()
    {
        // Arrange
        var platformId = Guid.NewGuid();
        var hideAdultContent = true;
        var mockGames = TestData.Game.GenerateGameResponseDtosWithAgeRatingLessThan18();
        _platformService.Setup(s => s.GetGamesAsync(platformId, hideAdultContent)).ReturnsAsync(mockGames);
        _platformController.ControllerContext.HttpContext.Items["Underage"] = hideAdultContent;
        // Act
        var result = await _platformController.GetGamesByPlatform(platformId);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        var responseModels = (okResult?.Value as IEnumerable<GameResponseModel> ?? []).ToList();
        Assert.That(responseModels.Count, Is.EqualTo(mockGames.Count));
    }
    
    [Test]
    public async Task GetGamesByPlatformAsync_WhenNoGames_ShouldReturnNotFound()
    {
        // Arrange
        var platformId = Guid.NewGuid();
        _platformService.Setup(s => s.GetGamesAsync(platformId, It.IsAny<bool>()))
            .ReturnsAsync(null as IEnumerable<GameResponseDto>);

        // Act
        var result = await _platformController.GetGamesByPlatform(platformId);

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }
    
    [Test]
    public async Task CreatePlatform_ShouldReturnCreatedResult_WhenPlatformIsCreated()
    {
        // Arrange
        var platformRequest = TestData.Platform.GeneratePlatformRequestModel();
        var platformDto = TestData.Platform.GeneratePlatformDto();
        
        _platformService.Setup(service => service.CreateAsync(It.IsAny<PlatformDto>()))
            .ReturnsAsync(platformDto.Id);
        
        // Act
        var result = await _platformController.CreatePlatform(platformRequest);
        
        // Assert
        var createdResult = result as CreatedAtActionResult;
        
        Assert.That(createdResult, Is.Not.Null);
        Assert.That(createdResult?.StatusCode, Is.EqualTo(201));
    }
    
    [Test]
    public async Task CreatePlatform_ShouldReturnBadRequest_WhenCreationFails()
    {
        // Arrange
        var platformRequest = TestData.Platform.GeneratePlatformRequestModel();
        
        _platformService.Setup(service => service.CreateAsync(It.IsAny<PlatformDto>()))
            .ReturnsAsync((Guid?)null);
        
        // Act
        var result = await _platformController.CreatePlatform(platformRequest);
        
        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }
    
    [Test]
    public async Task Update_ShouldReturnNoContent_WhenPlatformIsUpdated()
    {
        // Arrange
        _platformService.Setup(s => s.UpdateAsync(It.IsAny<PlatformDto>())).ReturnsAsync(true);
        
        // Act
        var result = await _platformController
            .UpdatePlatform(Guid.NewGuid(), TestData.Platform.GeneratePlatformRequestModel());
        
        // Assert
        var noContent = result as NoContentResult;
        Assert.That(noContent, Is.Not.Null);
    }

    [Test]
    public async Task Update_ShouldReturnBadRequest_WhenUpdateFails()
    {
        // Arrange
        _platformService.Setup(s => s.UpdateAsync(It.IsAny<PlatformDto>())).ReturnsAsync(false);
        
        // Act
        var result = await _platformController
            .UpdatePlatform(Guid.NewGuid(), TestData.Platform.GeneratePlatformRequestModel());
        
        // Assert
        var badRequest = result as BadRequestObjectResult;
        
        Assert.That(badRequest, Is.Not.Null);
    }
    
    [Test]
    public async Task Delete_ShouldReturnNoContent_WhenPlatformIsDeleted()
    {
        // Arrange
        var platformId = Guid.NewGuid();
        
        _platformService.Setup(s => s.DeleteAsync(platformId));
        
        // Act
        var result = await _platformController.DeletePlatform(platformId);
        
        // Assert
        var noContentResult = result as NoContentResult;
        
        Assert.That(noContentResult, Is.Not.Null);
    }
}