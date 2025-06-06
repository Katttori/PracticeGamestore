using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Enums;
using PracticeGamestore.Business.Services.Blacklist;
using PracticeGamestore.Business.Services.Country;
using PracticeGamestore.Controllers;
using PracticeGamestore.Models.Blacklist;

namespace PracticeGamestore.Tests.Unit.Blacklist;

public class BlacklistControllerTests
{
    private Mock<IBlacklistService> _blacklistServiceMock;
    private Mock<ICountryService> _countryServiceMock;
    private Mock<ILogger<BlacklistController>> _loggerMock;
    private BlacklistController _blacklistController;

    [SetUp]
    public void Setup()
    {
        _blacklistServiceMock = new Mock<IBlacklistService>();
        _countryServiceMock = new Mock<ICountryService>();
        _loggerMock = new Mock<ILogger<BlacklistController>>();
        _blacklistController = new BlacklistController(
            _blacklistServiceMock.Object,
            _countryServiceMock.Object,
            _loggerMock.Object);
    }

    [Test]
    public async Task GetAll_ReturnsOkWithBlacklists()
    {
        // Arrange
        var blacklistDtos = TestData.Blacklist.GenerateBlacklistDtos();
        
        _blacklistServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(blacklistDtos);
        
        // Act
        var result = await _blacklistController.GetAll();
        
        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        var responseModels = 
            (okResult?.Value as IEnumerable<BlacklistResponseModel> ?? []).ToList();
        Assert.That(responseModels.Count, Is.EqualTo(blacklistDtos.Count));
        Assert.That(responseModels.First().Id, Is.EqualTo(blacklistDtos.First().Id));
        Assert.That(responseModels.First().UserEmail, Is.EqualTo(blacklistDtos.First().UserEmail));
    }
    
    [Test]
    public async Task GetById_WhenBlacklistIsNull_ReturnsNotFound()
    {
        // Arrange
        _blacklistServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as BlacklistDto);
        
        // Act
        var result = await _blacklistController.GetById(Guid.NewGuid());
        
        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }
    
    [Test]
    public async Task GetById_WhenBlacklistFound_ReturnsOkWithBlacklist()
    {
        // Arrange
        var blacklistDto = TestData.Blacklist.GenerateBlacklistDto();
        
        _blacklistServiceMock.Setup(x => x.GetByIdAsync(blacklistDto.Id!.Value)).ReturnsAsync(blacklistDto);
        
        // Act
        var result = await _blacklistController.GetById(blacklistDto.Id!.Value);
        
        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var receivedBlacklist = (result as OkObjectResult)?.Value as BlacklistResponseModel;
        Assert.That(receivedBlacklist, Is.Not.Null);
        Assert.That(receivedBlacklist?.Id, Is.EqualTo(blacklistDto.Id));
        Assert.That(receivedBlacklist?.UserEmail, Is.EqualTo(blacklistDto.UserEmail));
    }
    
    [Test]
    public async Task Create_WhenOperationFailed_ReturnsBadRequest()
    {
        // Arrange
        var model = TestData.Blacklist.GenerateBlacklistRequestModel();

        _blacklistServiceMock
            .Setup(x => x.CreateAsync(It.IsAny<BlacklistDto>()))
            .ReturnsAsync(null as Guid?);
        
        // Act
        var result = await _blacklistController.Create(model);
        
        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }
    
    [Test]
    public async Task Create_WhenOperationSuccessful_ReturnsCreatedWithId()
    {
        // Arrange
        var newId = Guid.NewGuid();
        
        _countryServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(TestData.Country.GenerateCountryDto());
        _blacklistServiceMock.Setup(x => x.CreateAsync(It.IsAny<BlacklistDto>())).ReturnsAsync(newId);
        
        // Act
        var result = await _blacklistController.Create(TestData.Blacklist.GenerateBlacklistRequestModel());
        
        // Assert
        Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
        var createdResult = result as CreatedAtActionResult;
        Assert.That(createdResult?.Value, Is.EqualTo(newId));
    }
    
    [Test]
    public async Task Update_WhenOperationSuccessful_ReturnsNoContent()
    {
        // Arrange
        _countryServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(TestData.Country.GenerateCountryDto());
        _blacklistServiceMock
            .Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<BlacklistDto>()))
            .ReturnsAsync(true);
        
        // Act
        var result = await _blacklistController.Update(Guid.NewGuid(), TestData.Blacklist.GenerateBlacklistRequestModel());
        
        // Assert
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }
    
    [Test]
    public async Task Update_WhenOperationFailed_ReturnsBadRequest()
    {
        // Arrange
        _blacklistServiceMock
            .Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<BlacklistDto>()))
            .ReturnsAsync(false);
        
        // Act
        var result = await _blacklistController.Update(Guid.NewGuid(), TestData.Blacklist.GenerateBlacklistRequestModel());
        
        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }
    
    [Test]
    public async Task Delete_ReturnsNoContent()
    {
        // Arrange
        _blacklistServiceMock.Setup(x => x.DeleteAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);
        
        // Act
        var result = await _blacklistController.Delete(Guid.NewGuid());
        
        // Assert
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }
}