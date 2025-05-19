using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Services.Genre;
using PracticeGamestore.DataAccess.Repositories.Genre;
using PracticeGamestore.DataAccess.UnitOfWork;

namespace PracticeGamestore.Tests.Unit.Genre;

public class GenreServiceTests
{
    private Mock<IGenreRepository> _genreRepositoryMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private GenreService _service;

    [SetUp]
    public void Setup()
    {
        _genreRepositoryMock = new Mock<IGenreRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _service = new GenreService(_genreRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task GetAllAsync_ReturnsAllGenres()
    {
        var entities = new List<DataAccess.Entities.Genre>
        {
            new() { Id = Guid.NewGuid(), Name = "Action" },
            new() { Id = Guid.NewGuid(), Name = "FPS" },
        };
        _genreRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(entities);
        
        var result = await _service.GetAllAsync();
        
        Assert.That(result.Count(), Is.EqualTo(entities.Count));
        Assert.That(result.First().Id, Is.EqualTo(entities.First().Id));
        Assert.That(result.First().Name, Is.EqualTo(entities.First().Name));
    }

    [Test]
    public async Task GetByIdAsync_WhenGenreExists_ReturnsGenreDto()
    {
        var entity = new DataAccess.Entities.Genre() { Id = Guid.NewGuid(), Name = "Strategy" };
        _genreRepositoryMock.Setup(x => x.GetByIdAsync(entity.Id)).ReturnsAsync(entity);
        
        var result = await _service.GetByIdAsync(entity.Id);
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result?.Id, Is.EqualTo(entity.Id));
        Assert.That(result?.Name, Is.EqualTo(entity.Name));
    }
    
    [Test]
    public async Task GetByIdAsync_WhenGenreDoesNotExist_ReturnsNull()
    {
        _genreRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as DataAccess.Entities.Genre);
        
        var result = await _service.GetByIdAsync(Guid.NewGuid());
        
        Assert.That(result, Is.Null);
    }
}