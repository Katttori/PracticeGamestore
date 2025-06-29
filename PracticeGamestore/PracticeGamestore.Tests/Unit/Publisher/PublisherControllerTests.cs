using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Services.HeaderHandle;
using PracticeGamestore.Business.Services.Publisher;
using PracticeGamestore.Controllers;
using PracticeGamestore.DataAccess.Enums;
using PracticeGamestore.Mappers;
using PracticeGamestore.Models.Publisher;

namespace PracticeGamestore.Tests.Unit.Publisher;

public class PublisherControllerTests
{
    private Mock<IPublisherService> _publisherService;
    private Mock<IHeaderHandleService> _headerHandleService;
    private Mock<ILogger<PublisherController>> _loggerMock;
    private PublisherController _publisherController;
    
    private const string CountryHeader = "Ukraine";
    private const string UserEmailHeader = "test@gmail.com";
    
    [SetUp]
    public void SetUp()
    {
        _publisherService = new Mock<IPublisherService>();
        _headerHandleService = new Mock<IHeaderHandleService>();
        _loggerMock = new Mock<ILogger<PublisherController>>();
        _publisherController = new PublisherController(_publisherService.Object, _headerHandleService.Object, _loggerMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() 
            }
        };
    }

    [Test]
    public async Task GetAll_WhenPublishersExist_ShouldReturnOkWithPublishers()
    {
        //Arrange
        var publisherDtos = TestData.Publisher.GeneratePublisherDtos();
        
        _publisherService.Setup(x => x.GetAllAsync())
            .ReturnsAsync(publisherDtos);
        var expected = publisherDtos.Select(dto => dto.MapToPublisherModel()).ToList();

        //Act
        var result = await _publisherController.GetAll(CountryHeader, UserEmailHeader);

        //Assert
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult!.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        var response = (okResult.Value as IEnumerable<PublisherResponseModel> ?? []).ToList();
        Assert.That(response, Is.Not.Null);
        Assert.That(response.Count, Is.EqualTo(expected.Count));
        var elementsAreTheSame = response.Zip(expected, (r, e) =>
            e.Id == r.Id &&
            e.Description == r.Description &&
            e.PageUrl == r.PageUrl &&
            e.Name == r.Name).All(equal => equal);
        Assert.That(elementsAreTheSame, Is.True);
    }

    [Test]
    public async Task GetPublisherById_WhenPublisherExists_ShouldReturnOkResult()
    {
        //Arrange
        var id = Guid.NewGuid();
        var publisherDto = TestData.Publisher.GeneratePublisherDto();
        _publisherService.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(publisherDto);
        var expected = publisherDto.MapToPublisherModel();

        //Act
        var result = await _publisherController.GetById(CountryHeader, UserEmailHeader, id);

        //Assert
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult!.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        var response = okResult.Value as PublisherResponseModel;
        Assert.That(response, Is.Not.Null);
        Assert.That(response, Has.Property("Id").EqualTo(expected.Id)
            .And.Property("Name").EqualTo(expected.Name)
            .And.Property("Description").EqualTo(expected.Description)
            .And.Property("PageUrl").EqualTo(expected.PageUrl));
    }

    [Test]
    public async Task GetPublisherById_WhenPublisherDoesNotExist_ShouldReturnNotFound()
    {
        //Arrange
        _publisherService.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as PublisherDto);

        //Act
        var result = await _publisherController.GetById(CountryHeader, UserEmailHeader, Guid.NewGuid());

        //Assert
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task CreatePublisher_WhenPublisherIsCreated_ShouldReturnCreatedResult()
    {
        //Arrange
        var id = Guid.NewGuid();
        var publisherRequestModel = TestData.Publisher.GeneratePublisherRequestModel();
        _publisherService.Setup(x => x.CreateAsync(It.IsAny<PublisherDto>()))
            .ReturnsAsync(id);

        //Act
        var result = await _publisherController.Create(publisherRequestModel);

        //Assert
        var createdResult = result as CreatedAtActionResult;
        Assert.That(createdResult, Is.Not.Null);
        Assert.That(createdResult!.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
        Assert.That(createdResult.Value, Is.EqualTo(id));
        Assert.That(createdResult.ActionName, Is.EqualTo(nameof(PublisherController.GetById)));
        Assert.That(createdResult.RouteValues, Is.Not.Null);
        Assert.That(createdResult.RouteValues!["id"], Is.EqualTo(id));
    }

    [Test]
    public async Task CreatePublisher_WhenCreationFails_ShouldReturnBadRequest()
    {
        //Arrange
        var publisherRequestModel = TestData.Publisher.GeneratePublisherRequestModel();
        _publisherService.Setup(x => x.CreateAsync(It.IsAny<PublisherDto>()))
            .ReturnsAsync(null as Guid?);

        //Act
        var result = await _publisherController.Create(publisherRequestModel);
        
        //Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task Update_WhenPublisherIsUpdated_ShouldReturnNoContent()
    {
        //Arrange
        var id = Guid.NewGuid();
        var publisherRequestModel = TestData.Publisher.GeneratePublisherRequestModel();
        _publisherService.Setup(x => x.UpdateAsync(id, It.IsAny<PublisherDto>()))
            .ReturnsAsync(true);

        //Act
        var result = await _publisherController.Update(id, publisherRequestModel);
        
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task Update_WhenUpdateFails_ShouldReturnBadRequest()
    {
        //Arrange
        var id = Guid.NewGuid();
        var publisherRequestModel = TestData.Publisher.GeneratePublisherRequestModel();
        _publisherService.Setup(x => x.UpdateAsync(id, It.IsAny<PublisherDto>()))
            .ReturnsAsync(false);

        //Act
        var result = await _publisherController.Update(id, publisherRequestModel);
        
        //Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task Delete_WhenPublisherIsDeleted_ShouldReturnNoContent()
    {
        //Arrange
        _publisherService.Setup(x => x.DeleteAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);
        
        //Act
        var result = await _publisherController.Delete(Guid.NewGuid());
        
        //Assert
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }
    
    [Test]
    public async Task GetPublisherGames_WhenPublisherDoesNotExist_ShouldReturnNotFound()
    {
        //Arrange
        _publisherService.Setup(x => x.GetGamesAsync(It.IsAny<Guid>(), It.IsAny<bool>()))
            .ReturnsAsync(null as IEnumerable<GameResponseDto>);

        //Act
        var result = await _publisherController.GetPublisherGames(CountryHeader, UserEmailHeader, Guid.NewGuid());
        
        //Assert
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task GetPublisherGames_WhenPublisherExistAndUserIsAdult_ShouldReturnAllPublisherGames()
    {
        //Arrange
        var hideAdultContent = false;
        var publisherId = Guid.NewGuid();
        var games = TestData.Game.GenerateGameResponseDtos()
            .Where(g => g.Publisher.Id == publisherId).ToList();
        _publisherService.Setup(x => x.GetGamesAsync(publisherId, hideAdultContent))
            .ReturnsAsync(games);
        _publisherController.ControllerContext.HttpContext.Items[HttpContextCustomItems.UnderageIndicator] = hideAdultContent;

        //Act
        var result = await _publisherController.GetPublisherGames(CountryHeader, UserEmailHeader, publisherId);
        
        //Assert
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult!.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        var response =
            (okResult.Value as IEnumerable<GameResponseDto> ?? Array.Empty<GameResponseDto>()).ToList();
        Assert.That(response, Is.Not.Null);
        Assert.That(response.All(dto => dto.Publisher.Id == publisherId), Is.True);
    }
    
    [Test]
    public async Task GetPublisherGames_WhenPublisherExistAndUserIsUnderage_ShouldReturnPublisherGamesWithAgeRatingLessThan18()
    {
        //Arrange
        var hideAdultContent = true;
        var publisherId = Guid.NewGuid();
        var games = TestData.Game.GenerateGameResponseDtosWithAgeRatingLessThan18()
            .Where(g => g.Publisher.Id == publisherId).ToList();
        _publisherService.Setup(x => x.GetGamesAsync(publisherId, hideAdultContent))
            .ReturnsAsync(games);
        _publisherController.ControllerContext.HttpContext.Items[HttpContextCustomItems.UnderageIndicator] = hideAdultContent;

        //Act
        var result = await _publisherController.GetPublisherGames(CountryHeader, UserEmailHeader, publisherId);
        
        //Assert
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult!.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        var response =
            (okResult.Value as IEnumerable<GameResponseDto> ?? Array.Empty<GameResponseDto>()).ToList();
        Assert.That(response, Is.Not.Null);
        Assert.That(response.All(dto => dto.Publisher.Id == publisherId), Is.True);
    }
}