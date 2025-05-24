using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.Services.Game;
using PracticeGamestore.DataAccess.Repositories.Game;
using PracticeGamestore.DataAccess.Repositories.Genre;
using PracticeGamestore.DataAccess.Repositories.Platform;
using PracticeGamestore.DataAccess.Repositories.Publisher;
using PracticeGamestore.DataAccess.UnitOfWork;

namespace PracticeGamestore.Tests.Unit.Game;

[TestFixture]
public class GameServiceTests
{
    private Mock<IGameRepository> _gameRepository;
    private Mock<IPublisherRepository> _publisherRepository;
    private Mock<IGenreRepository> _genreRepository;
    private Mock<IPlatformRepository> _platformRepository;
    private Mock<IUnitOfWork> _unitOfWork;
    private IGameService _gameService;

    [SetUp]
    public void SetUp()
    {
        _gameRepository = new Mock<IGameRepository>();
        _publisherRepository = new Mock<IPublisherRepository>();
        _genreRepository = new Mock<IGenreRepository>();
        _platformRepository = new Mock<IPlatformRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _gameService = new GameService(_gameRepository.Object, _publisherRepository.Object, _genreRepository.Object, _platformRepository.Object, _unitOfWork.Object);
    }

}