using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.Services.Game;
using PracticeGamestore.Controllers;

namespace PracticeGamestore.Tests.Unit.Game;

[TestFixture]
public class GameControllerTests
{
    private Mock<IGameService> _gameService;
    private GameController _gameController;

    [SetUp]
    public void SetUp()
    {
        _gameService = new Mock<IGameService>();
        _gameController = new GameController(_gameService.Object);
    }

}