using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Services.Auth;
using PracticeGamestore.Business.Services.Token;
using PracticeGamestore.Business.Utils;
using PracticeGamestore.DataAccess.Enums;
using PracticeGamestore.DataAccess.Repositories.User;

namespace PracticeGamestore.Tests.Unit.Auth;

[TestFixture]
public class AuthServiceTests
{
    private Mock<ITokenService> _tokenService;
    private Mock<IUserRepository> _userRepository;
    private Mock<ILogger<AuthService>> _logger;
    private AuthService _authService;

    [SetUp]
    public void SetUp()
    {
        _tokenService = new Mock<ITokenService>();
        _userRepository = new Mock<IUserRepository>();
        _logger = new Mock<ILogger<AuthService>>();
        _authService = new AuthService(_tokenService.Object, _userRepository.Object, _logger.Object);
    }

    [Test]
    public async Task AuthenticateUser_WhenUserExistsAndPasswordIsCorrect_ShouldReturnTokenResponse()
    {
        // Arrange
        var user = TestData.User.GenerateUserEntity();
        var password = "testpassword";
        
        user.PasswordHash = PasswordHasher.HashPassword(password, user.PasswordSalt);
        var tokenResponse = new TokenResponseDto(user.Id, "test-jwt-token", 60);

        _userRepository.Setup(x => x.GetByEmailAsync(user.Email))
            .ReturnsAsync(user);
        
        _tokenService.Setup(x => x.GenerateJwtToken(user))
            .Returns(tokenResponse);

        // Act
        var result = await _authService.AuthenticateUser(user.Email, password);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.UserId, Is.EqualTo(user.Id));
        Assert.That(result.Token, Is.EqualTo("test-jwt-token"));
        Assert.That(result.Expiration, Is.EqualTo(60));
        
        _tokenService.Verify(x => x.GenerateJwtToken(user), Times.Once);
    }

    [Test]
    public async Task AuthenticateUser_WhenUserDoesNotExist_ShouldReturnNullAndLogWarning()
    {
        // Arrange
        var email = "nonexistent@example.com";
        var password = "password";

        _userRepository.Setup(x => x.GetByEmailAsync(email))
            .ReturnsAsync(null as DataAccess.Entities.User);

        // Act
        var result = await _authService.AuthenticateUser(email, password);

        // Assert
        Assert.That(result, Is.Null);
        
        _logger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains($"User with email {email} does not exist.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Test]
    public async Task AuthenticateUser_WhenPasswordIsIncorrect_ShouldReturnNull()
    {
        // Arrange
        var user = TestData.User.GenerateUserEntity();
        var incorrectPassword = "wrongpassword";

        _userRepository.Setup(x => x.GetByEmailAsync(user.Email))
            .ReturnsAsync(user);

        // Act
        var result = await _authService.AuthenticateUser(user.Email, incorrectPassword);

        // Assert
        Assert.That(result, Is.Null);
        
        _tokenService.Verify(x => x.GenerateJwtToken(It.IsAny<DataAccess.Entities.User>()), Times.Never);
    }
    
    [TestCase(UserStatus.Deleted, TestName = "When user's status is deleted returns null")]
    [TestCase(UserStatus.Banned, TestName = "When user's status is banned returns null")]
    public async Task AuthenticateUser_WhenTheirStatusIsBannedOrDeleted_ShouldReturnNull(UserStatus status)
    {
        // Arrange
        var user = TestData.User.GenerateUserEntity();
        user.Status = status;

        _userRepository.Setup(x => x.GetByEmailAsync(user.Email))
            .ReturnsAsync(user);

        // Act
        var result = await _authService.AuthenticateUser(user.Email, It.IsAny<string>());

        // Assert
        Assert.That(result, Is.Null);
        
        _tokenService.Verify(x => x.GenerateJwtToken(It.IsAny<DataAccess.Entities.User>()), Times.Never);
    }
    
}
