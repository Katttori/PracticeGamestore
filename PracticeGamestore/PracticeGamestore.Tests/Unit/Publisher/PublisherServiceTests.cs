using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.Business.Services.Publisher;
using PracticeGamestore.DataAccess.Repositories.Game;
using PracticeGamestore.DataAccess.Repositories.Publisher;
using PracticeGamestore.DataAccess.UnitOfWork;
using PracticeGamestore.Tests.TestData;

namespace PracticeGamestore.Tests.Unit.Publisher;

[TestFixture]
public class PublisherServiceTests
{
    private Mock<IPublisherRepository> _publisherRepository;
    private Mock<IUnitOfWork> _unitOfWork;
    private IPublisherService _publisherService;
    private Mock<IGameRepository> _gameRepository;

    [SetUp]
    public void Setup()
    {
        _publisherRepository = new Mock<IPublisherRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _gameRepository = new Mock<IGameRepository>();
        _publisherService = new PublisherService(_publisherRepository.Object, _gameRepository.Object, _unitOfWork.Object);
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnAllPublishers()
    {
        //Arrange
        var publishers = TestData.Publisher.GeneratePublisherEntities();
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
        var publisher = TestData.Publisher.GeneratePublisherEntity();
        _publisherRepository.Setup(x => x.GetByIdAsync(publisher.Id))
            .ReturnsAsync(publisher);
        var expected = publisher.MapToPublisherDto();

        //Act
        var result = await _publisherService.GetByIdAsync(publisher.Id);

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
        var publisher = TestData.Publisher.GeneratePublisherEntity();
        _publisherRepository.Setup(x => x.GetByIdAsync(publisher.Id))
            .ReturnsAsync(publisher);
        _unitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        
        //Act
        var result = await _publisherService.UpdateAsync(publisher.Id, publisher.MapToPublisherDto());
        
        //Assert
        Assert.That(result, Is.True);
    }
    
    [Test]
    public async Task UpdateAsync_WhenPublisherDoesNotExist_ReturnsFalse()
    {
        //Arrange
        var id = Guid.NewGuid();
        var publisher = TestData.Publisher.GeneratePublisherEntity();
        _publisherRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as DataAccess.Entities.Publisher);

        //Act
        var result = await _publisherService.UpdateAsync(id, publisher.MapToPublisherDto());
        
        //Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task CreateAsync_ShouldAddPublisher_WhenChangesSavedSuccessfully()
    {
        //Arrange
        var publisher = TestData.Publisher.GeneratePublisherEntity();
        _publisherRepository.Setup(x => x.CreateAsync(It.IsAny<DataAccess.Entities.Publisher>()))
            .ReturnsAsync(publisher.Id);
        _unitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        //Act
        var result = await _publisherService.CreateAsync(publisher.MapToPublisherDto());
        
        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(publisher.Id));
    }

    [Test]
    public async Task CreateAsync_ShouldReturnNull_WhenChangesNotSaved()
    {
        //Arrange
        var publisher = TestData.Publisher.GeneratePublisherEntity();
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
        var id = Guid.NewGuid();
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
    
    [Test]
    public async Task GetGamesAsync_ShouldReturnPublisherGames_WhenPublisherExist()
    {
        //Arrange
        var publisherId = Guid.NewGuid();
        var games = TestData.Game.GenerateGameEntities().Where(g => g.PublisherId == publisherId).ToList();
        _publisherRepository.Setup(x => x.ExistsAsync(publisherId))
            .ReturnsAsync(true);
        _gameRepository.Setup(x => x.GetByPublisherIdAsync(publisherId))
            .ReturnsAsync(games);
        
        //Act
        var result = await _publisherService.GetGamesAsync(publisherId);
        
        //Assert
        Assert.That(result, Is.Not.Null);
        var gameResponseDtos = result!.ToList();
        Assert.That(gameResponseDtos.Count, Is.EqualTo(games.Count));
        Assert.That(gameResponseDtos.All(dto => dto.Publisher.Id == publisherId), Is.True);
    }
    
    [Test]
    public async Task GetGamesAsync_ShouldReturnNull_WhenPublisherDoesNotExist()
    {
        //Arrange
        _publisherRepository.Setup(x => x.ExistsAsync(It.IsAny<Guid>()))
            .ReturnsAsync(false); 
        
        //Act
        var result = await _publisherService.GetGamesAsync(Guid.NewGuid());
        
        //Assert
        Assert.That(result, Is.Null);
    }
}