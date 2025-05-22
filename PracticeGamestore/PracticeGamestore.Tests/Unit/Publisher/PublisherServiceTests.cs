using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.Business.Services.Publisher;
using PracticeGamestore.DataAccess.Repositories;
using PracticeGamestore.DataAccess.UnitOfWork;

namespace PracticeGamestore.API.Tests.Unit.Publisher;

[TestFixture]
public class PublisherServiceTests
{
    private Mock<IPublisherRepository> _publisherRepository;
    private Mock<IUnitOfWork> _unitOfWork;
    private IPublisherService _publisherService;

    private static DataAccess.Entities.Publisher CreatePublisher(Guid id = new Guid())
    {
        return new()
        {
            Id = id, Name = "Electronic Arts", Description = "American video game company",
            PageUrl = "https://www.ea.com"
        };
    }

    [SetUp]
    public void Setup()
    {
        _publisherRepository = new Mock<IPublisherRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _publisherService = new PublisherService(_publisherRepository.Object, _unitOfWork.Object);
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnAllPublishers()
    {
        //Arrange
        var publishers = new List<DataAccess.Entities.Publisher>()
        {
            new() { Id = Guid.NewGuid(), Name = "Electronic Arts", Description = "American video game company", PageUrl = "https://www.ea.com" },
            new() { Id = Guid.NewGuid(), Name = "Ubisoft", Description = "French video game company", PageUrl = "https://www.ubisoft.com" },
            new() { Id = Guid.NewGuid(), Name = "Activision Blizzard", Description = "American video game holding company", PageUrl = "https://www.activisionblizzard.com" },
        };
        _publisherRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(publishers);
        var expected = publishers.Select(p => p.MapToPublisherDto()).ToList();
        
        //Act
        var result = (await _publisherService.GetAllAsync()).ToList();
        
        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        Assert.That(result.Count, Is.EqualTo(expected.Count));
        var elementsAreTheSame = expected.Zip(result, (e, r) =>
            e.Id == r.Id &&
            e.Description == r.Description &&
            e.PageUrl == r.PageUrl &&
            e.Name == r.Name).All(equal => equal);
        Assert.That(elementsAreTheSame, Is.True);
    }

    [Test]
    public async Task GetByIdAsync_WhenPublisherExists_ReturnsPublisherDto()
    {
        //Arrange
        var id = new Guid();
        var publisher = CreatePublisher(id);
        _publisherRepository.Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(publisher);
        var expected = publisher.MapToPublisherDto();

        //Act
        var result = await _publisherService.GetByIdAsync(id);

        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Property("Id").EqualTo(expected.Id)
            .And.Property("Name").EqualTo(expected.Name)
            .And.Property("Description").EqualTo(expected.Description)
            .And.Property("PageUrl").EqualTo(expected.PageUrl));
    }

    [Test]
    public async Task GetByIdAsync_WhenPublisherDoesNotExist_ReturnsNull()
    {
        //Arrange
        _publisherRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as DataAccess.Entities.Publisher);

        //Act
        var result = await _publisherService.GetByIdAsync(new Guid());
        
        //Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task UpdateAsync_WhenPublisherExistsAndChangesSavedSuccessfully_ReturnsTrue()
    {
        //Arrange
        var id = new Guid();
        var publisher = CreatePublisher(id);
        _publisherRepository.Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(publisher);
        _unitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        
        //Act
        var result = await _publisherService.UpdateAsync(publisher.MapToPublisherDto());
        
        //Assert
        Assert.That(result, Is.True);
    }
    
    [Test]
    public async Task UpdateAsync_WhenPublisherDoesNotExist_ReturnsFalse()
    {
        //Arrange
        var publisher = CreatePublisher();
        _publisherRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as DataAccess.Entities.Publisher);

        //Act
        var result = await _publisherService.UpdateAsync(publisher.MapToPublisherDto());
        
        //Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task CreateAsync_ShouldAddPublisher_WhenChangesSavedSuccessfully()
    {
        //Arrange
        var id = new Guid();
        var publisher = CreatePublisher(id);
        _publisherRepository.Setup(x => x.CreateAsync(It.IsAny<DataAccess.Entities.Publisher>()))
            .ReturnsAsync(publisher.Id);
        _unitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        //Act
        var result = await _publisherService.CreateAsync(publisher.MapToPublisherDto());
        
        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(id));
    }

    [Test]
    public async Task CreateAsync_ShouldReturnNull_WhenChangesNotSaved()
    {
        //Arrange
        var id = new Guid();
        var publisher = CreatePublisher(id);
        _publisherRepository.Setup(x => x.CreateAsync(It.IsAny<DataAccess.Entities.Publisher>()))
            .ReturnsAsync(publisher.Id); 
        _unitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);
        
        //Act
        var result = await _publisherService.CreateAsync(publisher.MapToPublisherDto());
        
        //Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task DeleteAsync_ShouldCallDeleteAndSaveChanges()
    {
        //Arrange
        var id = new Guid();
        _publisherRepository.Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
            .Returns(Task.CompletedTask);
        _unitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        //Act
        await _publisherService.DeleteAsync(id);
        
        //Assert
        _publisherRepository.Verify(x => x.DeleteAsync(id), Times.Once);
        _unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}