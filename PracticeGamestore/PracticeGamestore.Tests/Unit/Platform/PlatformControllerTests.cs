using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Services.Platform;
using PracticeGamestore.Controllers;
using PracticeGamestore.DataAccess.Repositories.Platform;
using PracticeGamestore.Models.Platform;

namespace PracticeGamestore.API.Tests.Unit.Platform;

public class PlatformControllerTests
{
    private Mock<IPlatformService> _platformService;
    private PlatformController _platformController;
    
    [SetUp]
    public void Setup()
    {
        _platformService = new Mock<IPlatformService>();
        _platformController = new PlatformController(_platformService.Object);
    }
    
    [Test]
    public async Task GetAllPlatforms_ShouldReturnOkResult_WhenPlatformsExist()
    {
        var platforms = new List<PlatformDto>
        {
            new(Guid.NewGuid(), "PC", "Personal Computer"),
            new(Guid.NewGuid(), "PS5", "PlayStation 5")
        };
        
        _platformService.Setup(service => service.GetAllAsync()).ReturnsAsync(platforms);
        
        var result = await _platformController.GetAllPlatforms();
        
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
        var platform = new PlatformDto(Guid.NewGuid(), "PC", "Personal Computer");
        
        _platformService.Setup(service => service.GetByIdAsync(platform.Id)).ReturnsAsync(platform);
        
        var result = await _platformController.GetPlatformById(platform.Id);
        
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
        var platformId = Guid.NewGuid();
        
        _platformService.Setup(service => service.GetByIdAsync(platformId)).ReturnsAsync((PlatformDto?)null);
        
        var result = await _platformController.GetPlatformById(platformId);
        
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }
    
    [Test]
    public async Task CreatePlatform_ShouldReturnCreatedResult_WhenPlatformIsCreated()
    {
        var platformRequest = new PlatformRequestModel
        {
            Name = "PC",
            Description = "Personal Computer"
        };
        
        var platformDto = new PlatformDto(Guid.NewGuid(), platformRequest.Name, platformRequest.Description);
        
        _platformService.Setup(service => service.CreateAsync(It.IsAny<PlatformDto>()))
            .ReturnsAsync(platformDto.Id);
        
        var result = await _platformController.CreatePlatform(platformRequest);
        
        var createdResult = result as CreatedAtActionResult;
        
        Assert.That(createdResult, Is.Not.Null);
        Assert.That(createdResult?.StatusCode, Is.EqualTo(201));
    }
    
    [Test]
    public async Task CreatePlatform_ShouldReturnBadRequest_WhenCreationFails()
    {
        var platformRequest = new PlatformRequestModel
        {
            Name = "PC",
            Description = "Personal Computer"
        };
        
        _platformService.Setup(service => service.CreateAsync(It.IsAny<PlatformDto>()))
            .ReturnsAsync((Guid?)null);
        
        var result = await _platformController.CreatePlatform(platformRequest);
        
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }
    
    [Test]
    public async Task Update_ShouldReturnNoContent_WhenPlatformIsUpdated()
    {
        _platformService.Setup(s => s.UpdateAsync(It.IsAny<PlatformDto>())).ReturnsAsync(true);
        
        var result = await _platformController
            .UpdatePlatform(Guid.NewGuid(), new PlatformRequestModel {Name = "PC", Description = "Personal Computer"});
        
        var noContent = result as NoContentResult;
        Assert.That(noContent, Is.Not.Null);
    }

    [Test]
    public async Task Update_ShouldReturnBadRequest_WhenUpdateFails()
    {
        _platformService.Setup(s => s.UpdateAsync(It.IsAny<PlatformDto>())).ReturnsAsync(false);
        
        var result = await _platformController
            .UpdatePlatform(Guid.NewGuid(), new PlatformRequestModel {Name = "PC", Description = "Personal Computer"});
        
        var badRequest = result as BadRequestObjectResult;
        
        Assert.That(badRequest, Is.Not.Null);
    }
    
    [Test]
    public async Task Delete_ShouldReturnNoContent_WhenPlatformIsDeleted()
    {
        var platformId = Guid.NewGuid();
        
        _platformService.Setup(s => s.DeleteAsync(platformId));
        
        var result = await _platformController.DeletePlatform(platformId);
        
        var noContentResult = result as NoContentResult;
        
        Assert.That(noContentResult, Is.Not.Null);
    }
}