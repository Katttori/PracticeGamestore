using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.Business.Services.Publisher;
using PracticeGamestore.DataAccess.Repositories.Game;
using PracticeGamestore.DataAccess.Repositories.Publisher;
using PracticeGamestore.DataAccess.UnitOfWork;

namespace PracticeGamestore.Tests.Unit.Publisher;

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
        var result = await _publisherService.GetByIdAsync(Guid.NewGuid());
        
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
    public void UpdateAsync_WhenNameAlreadyExists_ThrowsArgumentException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var publisher = TestData.Publisher.GeneratePublisherEntity();
        var dto = TestData.Publisher.GeneratePublisherDto();
        dto.Name = "New Name";
        
        _publisherRepository.Setup(p => p.GetByIdAsync(id)).ReturnsAsync(publisher);
        _publisherRepository.Setup(p => p.ExistsByNameAsync(dto.Name)).ReturnsAsync(true);
    
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() => _publisherService.UpdateAsync(id, dto));
    }
    
    [Test]
    public void UpdateAsync_WhenPageUrlAlreadyExists_ThrowsArgumentException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var publisher = TestData.Publisher.GeneratePublisherEntity();
        var dto = TestData.Publisher.GeneratePublisherDto();
        dto.PageUrl = "Existing Page Url";
        
        _publisherRepository.Setup(p => p.GetByIdAsync(id)).ReturnsAsync(publisher);
        _publisherRepository.Setup(p => p.ExistsByPageUrlAsync(dto.PageUrl)).ReturnsAsync(true);
    
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() => _publisherService.UpdateAsync(id, dto));
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
    public void CreateAsync_WhenNameAlreadyExists_ThrowsArgumentException()
    {
        // Arrange
        var publisher = TestData.Publisher.GeneratePublisherEntity();
        
        _publisherRepository.Setup(p => p.ExistsByNameAsync(publisher.Name)).ReturnsAsync(true);
    
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() => _publisherService.CreateAsync(publisher.MapToPublisherDto()));
    }
    
    [Test]
    public void CreateAsync_WhenPageUrlAlreadyExists_ThrowsArgumentException()
    {
        // Arrange
        var dto = TestData.Publisher.GeneratePublisherDto();
        
        _publisherRepository.Setup(p => p.ExistsByPageUrlAsync(dto.PageUrl)).ReturnsAsync(true);
    
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() => _publisherService.CreateAsync(dto));
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
    public async Task GetByPublisherGamesAsync_WhenPublisherExistsAndUserIsAdult_ShouldReturnAllGames()
    {
        //Arrange
        var publisherId = Guid.NewGuid();
        var hideAdultContent = false;
        var games = TestData.Game.GenerateGameEntities().Where(g => g.PublisherId == publisherId).ToList();
        _publisherRepository.Setup(x => x.ExistsAsync(publisherId))
            .ReturnsAsync(true);
        _gameRepository.Setup(x => x.GetByPublisherIdAsync(publisherId, hideAdultContent))
            .ReturnsAsync(games);
        
        //Act
        var result =
            (await _publisherService.GetGamesAsync(publisherId) ?? Array.Empty<GameResponseDto>()).ToList();
        
        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(games.Count));
        Assert.That(result.All(dto => dto.Publisher.Id == publisherId), Is.True);
    }
    
    [Test]
    public async Task GetByPublisherGamesAsync_WhenPublisherExistAndUserIsUnderage_ShouldReturnGamesWithAgeRatingLessThan18()
    {
        //Arrange
        var publisherId = Guid.NewGuid();
        var hideAdultContent = true;
        var games = TestData.Game.GenerateGameEntitiesWithAgeRatingLessThan18().Where(g => g.PublisherId == publisherId).ToList();
        _publisherRepository.Setup(x => x.ExistsAsync(publisherId))
            .ReturnsAsync(true);
        _gameRepository.Setup(x => x.GetByPublisherIdAsync(publisherId, hideAdultContent))
            .ReturnsAsync(games);
        
        //Act
        var result =
            (await _publisherService.GetGamesAsync(publisherId) ?? Array.Empty<GameResponseDto>()).ToList();
        
        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(games.Count));
        Assert.That(result.All(dto => dto.Publisher.Id == publisherId), Is.True);
    }
    
    [Test]
    public async Task GetByPublisherGamesAsync_WhenPublisherDoesNotExist_ShouldReturnNull()
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