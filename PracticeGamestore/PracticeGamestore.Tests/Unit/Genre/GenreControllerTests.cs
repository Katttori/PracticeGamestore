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
        var genreDtos = new List<GenreDto>
        {
            new(Guid.NewGuid(), "FPS"),
            new(Guid.NewGuid(), "Action"),
        };
        _genreServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(genreDtos);
        
        var result = await _genreController.GetAll();
        
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        var responseModels = 
            (okResult?.Value as IEnumerable<GenreResponseModel> ?? Array.Empty<GenreResponseModel>()).ToList();
        Assert.That(responseModels.Count, Is.EqualTo(genreDtos.Count));
        Assert.That(responseModels.First().Id, Is.EqualTo(genreDtos.First().Id));
        Assert.That(responseModels.First().Name, Is.EqualTo(genreDtos.First().Name));
    }
    
    [Test]
    public async Task GetById_WhenGenreIsNull_ReturnsNotFound()
    {
        _genreServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as GenreDto);
        
        var result = await _genreController.GetById(Guid.NewGuid());
        
        var notFoundResult = result as NotFoundObjectResult;
        Assert.That(notFoundResult, Is.Not.Null);
    }
    
    [Test]
    public async Task GetById_WhenGenreFound_ReturnsOkWithGenre()
    {
        var genreDto = new GenreDto(Guid.NewGuid(), "FPS");
        _genreServiceMock.Setup(x => x.GetByIdAsync(genreDto.Id)).ReturnsAsync(genreDto);
        
        var result = await _genreController.GetById(genreDto.Id);
        
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        var receivedGenre = okResult?.Value as GenreResponseModel;
        Assert.That(receivedGenre, Is.Not.Null);
        Assert.That(receivedGenre?.Id, Is.EqualTo(genreDto.Id));
        Assert.That(receivedGenre?.Name, Is.EqualTo(genreDto.Name));
    }
    
    [Test]
    public async Task Create_WhenOperationFailed_ReturnsBadRequest()
    {
        var model = new GenreRequestModel { Name = "FPS" };
        _genreServiceMock.Setup(x => x.CreateAsync(It.IsAny<GenreDto>())).ReturnsAsync(null as Guid?);
        
        var result = await _genreController.Create(model);
        
        var badRequestResult = result as BadRequestObjectResult;
        Assert.That(badRequestResult, Is.Not.Null);
    }
    
    [Test]
    public async Task Create_WhenOperationSuccessful_ReturnsCreatedWithId()
    {
        var newId = Guid.NewGuid();
        _genreServiceMock.Setup(x => x.CreateAsync(It.IsAny<GenreDto>())).ReturnsAsync(newId);
        
        var result = await _genreController.Create(new GenreRequestModel { Name = "FPS" });
        
        var createdResult = result as CreatedAtActionResult;
        Assert.That(createdResult, Is.Not.Null);
        Assert.That(createdResult?.Value, Is.EqualTo(newId));
    }
    
    [Test]
    public async Task Update_WhenOperationSuccessful_ReturnsNoContent()
    {
        _genreServiceMock.Setup(x => x.UpdateAsync(It.IsAny<GenreDto>())).ReturnsAsync(true);
        
        var result = await _genreController.Update(Guid.NewGuid(), new GenreRequestModel { Name = "FPS" });
        
        var noContentResult = result as NoContentResult;
        Assert.That(noContentResult, Is.Not.Null);
    }
    
    [Test]
    public async Task Update_WhenOperationFailed_ReturnsBadRequest()
    {
        _genreServiceMock.Setup(x => x.UpdateAsync(It.IsAny<GenreDto>())).ReturnsAsync(false);
        
        var result = await _genreController.Update(Guid.NewGuid(), new GenreRequestModel { Name = "FPS" });
        
        var badRequestResult = result as BadRequestObjectResult;
        Assert.That(badRequestResult, Is.Not.Null);
    }
    
    [Test]
    public async Task Delete_ReturnsNoContent()
    {
        _genreServiceMock.Setup(x => x.DeleteAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);
        
        var result = await _genreController.Delete(Guid.NewGuid());
        
        var noContentResult = result as NoContentResult;
        Assert.That(noContentResult, Is.Not.Null);
    }
}