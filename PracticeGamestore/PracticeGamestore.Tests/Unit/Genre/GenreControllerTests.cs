using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Services.Genre;
using PracticeGamestore.Controllers;
using PracticeGamestore.Models.Genre;

namespace PracticeGamestore.Tests.Unit.Genre;

public class GenreControllerTests
{
    private Mock<IGenreService> _genreServiceMock;
    private GenreController _genreController;

    [SetUp]
    public void Setup()
    {
        _genreServiceMock = new Mock<IGenreService>();
        _genreController = new GenreController(_genreServiceMock.Object);
    }

    [Test]
    public async Task GetAll_ReturnsOkWithGenres()
    {
        //Arrange
        var genreDtos = new List<GenreDto>
        {
            new(Guid.NewGuid(), "FPS"),
            new(Guid.NewGuid(), "Action"),
        };
        
        _genreServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(genreDtos);
        
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
        //Arrange
        _genreServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as GenreDto);
        
        // Act
        var result = await _genreController.GetById(Guid.NewGuid());
        
        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }
    
    [Test]
    public async Task GetById_WhenGenreFound_ReturnsOkWithGenre()
    {
        //Arrange
        var genreDto = new GenreDto(Guid.NewGuid(), "FPS");
        
        _genreServiceMock.Setup(x => x.GetByIdAsync(genreDto.Id!.Value)).ReturnsAsync(genreDto);
        
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
        //Arrange
        var model = new GenreRequestModel { Name = "FPS" };
        
        _genreServiceMock.Setup(x => x.CreateAsync(It.IsAny<GenreDto>())).ReturnsAsync(null as Guid?);
        
        // Act
        var result = await _genreController.Create(model);
        
        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }
    
    [Test]
    public async Task Create_WhenOperationSuccessful_ReturnsCreatedWithId()
    {
        //Arrange
        var newId = Guid.NewGuid();
        
        _genreServiceMock.Setup(x => x.CreateAsync(It.IsAny<GenreDto>())).ReturnsAsync(newId);
        
        // Act
        var result = await _genreController.Create(new GenreRequestModel { Name = "FPS" });
        
        // Assert
        Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
        var createdResult = result as CreatedAtActionResult;
        Assert.That(createdResult?.Value, Is.EqualTo(newId));
    }
    
    [Test]
    public async Task Update_WhenOperationSuccessful_ReturnsNoContent()
    {
        //Arrange
        _genreServiceMock.Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<GenreDto>())).ReturnsAsync(true);
        
        // Act
        var result = await _genreController.Update(Guid.NewGuid(), new GenreRequestModel { Name = "FPS" });
        
        // Assert
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }
    
    [Test]
    public async Task Update_WhenOperationFailed_ReturnsBadRequest()
    {
        //Arrange
        _genreServiceMock.Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<GenreDto>())).ReturnsAsync(false);
        
        // Act
        var result = await _genreController.Update(Guid.NewGuid(), new GenreRequestModel { Name = "FPS" });
        
        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }
    
    [Test]
    public async Task Delete_ReturnsNoContent()
    {
        //Arrange
        _genreServiceMock.Setup(x => x.DeleteAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);
        
        // Act
        var result = await _genreController.Delete(Guid.NewGuid());
        
        // Assert
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }
}