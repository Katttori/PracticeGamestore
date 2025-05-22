using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.Services;
using PracticeGamestore.DataAccess.Repositories;
using PracticeGamestore.DataAccess.UnitOfWork;

namespace PracticeGamestore.API.Tests.Unit.Game;

[TestFixture]
public class GameServiceTests
{
    private Mock<IGameRepository> _gameRepository;
    private Mock<IUnitOfWork> _unitOfWork;
    private IGameService _gameService;

    [SetUp]
    public void SetUp()
    {
        _gameRepository = new Mock<IGameRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _gameService = new GameService(_gameRepository.Object, _unitOfWork.Object);
    }

}