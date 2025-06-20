using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Services.Game;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.Business.Services.HeaderHandle;
using PracticeGamestore.Business.Services.Platform;
using PracticeGamestore.Controllers;
using PracticeGamestore.Models.Game;
using PracticeGamestore.Models.Platform;

namespace PracticeGamestore.Tests.Unit.Platform;

[TestFixture]
public class PlatformControllerTests
{
    private Mock<IPlatformService> _platformService;
    private Mock<IHeaderHandleService> _headerHandleService;
    private Mock<ILogger<PlatformController>> _loggerMock;
    private PlatformController _platformController;
    
    private const string CountryHeader = "Ukraine";
    private const string UserEmailHeader = "test@gmail.com";
    
    [SetUp]
    public void Setup()
    {
        _platformService = new Mock<IPlatformService>();
        _headerHandleService = new Mock<IHeaderHandleService>();
        _loggerMock = new Mock<ILogger<PlatformController>>();
        _platformController = new PlatformController(_platformService.Object, _headerHandleService.Object, _loggerMock.Object)   {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() 
            }
        };
    }
    
    [Test]
    public async Task GetAllPlatforms_WhenPlatformsExist_ShouldReturnOkResult()
    {
        // Arrange
        var platforms = TestData.Platform.GeneratePlatformDtos();
        
        _platformService.Setup(service => service.GetAllAsync()).ReturnsAsync(platforms);
        
        // Act
        var result = await _platformController.GetAllPlatforms(CountryHeader, UserEmailHeader);
        
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
    public async Task GetPlatformById_WhenPlatformExists_ShouldReturnOkResult()
    {
        // Arrange
        var platform = TestData.Platform.GeneratePlatformDto();
        
        _platformService.Setup(service => service.GetByIdAsync(platform.MapToPlatformEntity().Id)).ReturnsAsync(platform);
        
        // Act
        var result = await _platformController.GetPlatformById(CountryHeader, UserEmailHeader, platform.MapToPlatformEntity().Id);
        
        // Assert
        var okResult = result as OkObjectResult;
        
        var response = okResult?.Value as PlatformResponseModel;
        
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult?.StatusCode, Is.EqualTo(200));
        Assert.That(response?.Id, Is.EqualTo(platform.Id));
        Assert.That(response?.Name, Is.EqualTo(platform.Name));
    }
    
    [Test]
    public async Task GetPlatformById_WhenPlatformDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var platformId = Guid.NewGuid();
        
        _platformService.Setup(service => service.GetByIdAsync(platformId)).ReturnsAsync((PlatformDto?)null);
        
        // Act
        var result = await _platformController.GetPlatformById(CountryHeader, UserEmailHeader, platformId);
        
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
        _platformController.ControllerContext.HttpContext.Items[HttpContextCustomItems.UnderageIndicator] = hideAdultContent;

        // Act
        var result = await _platformController.GetGamesByPlatform(CountryHeader, UserEmailHeader, platformId);

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
        _platformController.ControllerContext.HttpContext.Items[HttpContextCustomItems.UnderageIndicator] = hideAdultContent;
        // Act
        var result = await _platformController.GetGamesByPlatform(CountryHeader, UserEmailHeader, platformId);

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
        var result = await _platformController.GetGamesByPlatform(CountryHeader, UserEmailHeader, platformId);

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }
    
    [Test]
    public async Task CreatePlatform_WhenPlatformIsCreated_ShouldReturnCreatedResult()
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
    public async Task CreatePlatform_WhenCreationFails_ShouldReturnBadRequest()
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
    public async Task Update_WhenPlatformIsUpdated_ShouldReturnNoContent()
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
    public async Task Update_WhenUpdateFails_ShouldReturnBadRequest()
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
    public async Task Delete_WhenPlatformIsDeleted_ShouldReturnNoContent()
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