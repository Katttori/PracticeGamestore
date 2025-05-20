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
        
        var result = (await _service.GetAllAsync()).ToList();
        
        Assert.That(result.Count, Is.EqualTo(entities.Count));
        Assert.That(result.First().Id, Is.EqualTo(entities.First().Id));
        Assert.That(result.First().Name, Is.EqualTo(entities.First().Name));
    }

    [Test]
    public async Task GetByIdAsync_WhenGenreExists_ReturnsGenreDto()
    {
        var entity = new DataAccess.Entities.Genre { Id = Guid.NewGuid(), Name = "Strategy" };
        _genreRepositoryMock.Setup(x => x.GetByIdAsync(entity.Id)).ReturnsAsync(entity);
        
        var result = await _service.GetByIdAsync(entity.Id);
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result?.Id, Is.EqualTo(entity.Id));
        Assert.That(result?.Name, Is.EqualTo(entity.Name));
    }
    
    [Test]
    public async Task GetByIdAsync_WhenGenreDoesNotExist_ReturnsNull()
    {
        _genreRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as DataAccess.Entities.Genre);
        
        var result = await _service.GetByIdAsync(Guid.NewGuid());
        
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task CreateAsync_WhenChangesSavedSuccessfully_ReturnsCreatedId()
    {
        var dto = new GenreDto(Guid.NewGuid(), "Action");
        _genreRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<DataAccess.Entities.Genre>()))
            .ReturnsAsync(dto.Id);
        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        
        var result = await _service.CreateAsync(dto);
        
        Assert.That(result, Is.EqualTo(dto.Id));
    }
    
    [Test]
    public async Task CreateAsync_WhenSaveChangesFailed_ReturnsNull()
    {
        var dto = new GenreDto(Guid.NewGuid(), "Action");
        _genreRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<DataAccess.Entities.Genre>()))
            .ReturnsAsync(dto.Id);
        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);
        
        var result = await _service.CreateAsync(dto);
        
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task UpdateAsync_WhenEntityExistsAndChangesSavedSuccessfully_ReturnsTrue()
    {
        var id = Guid.NewGuid();
        var dto = new GenreDto(id, "Action");
        var entity = new DataAccess.Entities.Genre { Id = id, Name = "Action" };
        _genreRepositoryMock
            .Setup(x => x.GetByIdAsync(entity.Id))
            .ReturnsAsync(entity);
        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        
        var result = await _service.UpdateAsync(dto);
        
        Assert.That(result, Is.True);
    }
    
    [Test]
    public async Task UpdateAsync_WhenEntityDoesNotExist_ReturnsFalse()
    {
        var dto = new GenreDto(Guid.NewGuid(), "Action");
        _genreRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as DataAccess.Entities.Genre);
        
        var result = await _service.UpdateAsync(dto);
        
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task DeleteAsync_CallsDeleteAndSaveChanges()
    {
        var id = Guid.NewGuid();
        
         await _service.DeleteAsync(id);
         
         _genreRepositoryMock.Verify(x => x.DeleteAsync(id), Times.Once);
         _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}