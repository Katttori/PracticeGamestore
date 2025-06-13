using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.Business.Services.User;
using PracticeGamestore.DataAccess.Repositories.Blacklist;
using PracticeGamestore.DataAccess.Repositories.User;
using PracticeGamestore.DataAccess.UnitOfWork;

namespace PracticeGamestore.Tests.Unit.User;

public class UserServiceTests
{
    private Mock<IUserRepository> _userRepository;
    private Mock<IBlacklistRepository> _blacklistRepository;
    private Mock<IUnitOfWork> _unitOfWork;
    private IUserService _userService;
    
    [SetUp]
    public void Setup()
    {
        _userRepository = new Mock<IUserRepository>();
        _blacklistRepository = new Mock<IBlacklistRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _userService = new UserService(_userRepository.Object, _blacklistRepository.Object, _unitOfWork.Object);
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnAllUsers()
    {
        // Arrange
        var users = TestData.User.GenerateUserEntities();
        _userRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(users);
        var expected = users.Select(p => p.MapToUserDto()).ToList();
        
        // Act
        var result = (await _userService.GetAllAsync()).ToList();
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        Assert.That(result.Count, Is.EqualTo(expected.Count));
    }

    [Test]
    public async Task GetByIdAsync_WhenUserExists_ReturnsUserDto()
     {
        // Arrange
        var user = TestData.User.GenerateUserEntity();
        _userRepository.Setup(x => x.GetByIdAsync(user.Id))
            .ReturnsAsync(user);
        var expected = user.MapToUserDto();
        
        // Act
        var result = await _userService.GetByIdAsync(user.Id);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(expected.Id));
        Assert.That(result.UserName, Is.EqualTo(expected.UserName));
        Assert.That(result.Email, Is.EqualTo(expected.Email));
     }

    [Test]
    public async Task GetByIdAsync_WhenUserDoesNotExist_ReturnsNull()
    {
        // Arrange
        _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as DataAccess.Entities.User);
        
        // Act
        var result = await _userService.GetByIdAsync(Guid.NewGuid());
        
        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task UpdateAsync_WhenUserExistsAndChangesSavedSuccessfully_ReturnsTrue()
    {
        // Arrange
        var user = TestData.User.GenerateUserEntity();
        _userRepository.Setup(x => x.GetByIdAsync(user.Id))
            .ReturnsAsync(user);
        _unitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        
        // Act
        var result = await _userService.UpdateAsync(user.Id, user.MapToUserDto());
        
        // Assert
        Assert.That(result, Is.True);
    }
    
    [Test]
    public async Task UpdateAsync_WhenUserDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var id = Guid.NewGuid();
        var user = TestData.User.GenerateUserEntity();
        _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as DataAccess.Entities.User);
        
        // Act
        var result = await _userService.UpdateAsync(id, user.MapToUserDto());
        
        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task CreateAsync_ShouldAddUser_WhenChangesSavedSuccessfully()
    {
        // Arrange
        var user = TestData.User.GenerateUserEntity();
        _userRepository.Setup(x => x.CreateAsync(It.IsAny<DataAccess.Entities.User>()))
            .ReturnsAsync(user.Id);
        _unitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        
        // Act
        var result = await _userService.CreateAsync(user.MapToUserDto());
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(user.Id));
    }

    [Test]
    public async Task CreateAsync_ShouldReturnNull_WhenChangesNotSaved()
    {
        // Arrange
        var user = TestData.User.GenerateUserEntity();
        _userRepository.Setup(x => x.CreateAsync(It.IsAny<DataAccess.Entities.User>()))
            .ReturnsAsync(user.Id); 
        _unitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);
        
        // Act
        var result = await _userService.CreateAsync(user.MapToUserDto());
        
        // Assert
        Assert.That(result, Is.Null);
    }
    
    [Test]
    public async Task CreateAsync_ShouldReturnStatusBanned_WhenUserInBlacklist()
    {
        // Arrange
        var userDto = TestData.User.GenerateUserDto();
        
        _blacklistRepository.Setup(x => x.ExistsByUserEmailAsync(userDto.Email))
            .ReturnsAsync(true);
        _blacklistRepository.Setup(b => b.GetAllAsync()).ReturnsAsync(new List<DataAccess.Entities.Blacklist>());
        _userRepository.Setup(r => r.CreateAsync(It.IsAny<DataAccess.Entities.User>())).ReturnsAsync(Guid.NewGuid());
        _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        
        // Act
        var result = await _userService.CreateAsync(userDto);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(userDto.Status, Is.EqualTo("Banned"));
    }
    
    [Test]
    public async Task CreateAsync_ShouldSetStatusBanned_WhenCountryMismatchInBlacklist()
    {
        // Arrange
        var userDto = TestData.User.GenerateUserDto();
        var blacklistItem = new DataAccess.Entities.Blacklist
        {
            Id = Guid.NewGuid(),
            UserEmail = userDto.Email,
            CountryId = Guid.NewGuid()
        };

        _blacklistRepository.Setup(b => b.ExistsByUserEmailAsync(userDto.Email)).ReturnsAsync(true);
        _blacklistRepository.Setup(b => b.GetAllAsync()).ReturnsAsync(new List<DataAccess.Entities.Blacklist> { blacklistItem });
        _userRepository.Setup(r => r.CreateAsync(It.IsAny<DataAccess.Entities.User>())).ReturnsAsync(Guid.NewGuid());
        _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _userService.CreateAsync(userDto);

        // Assert
        Assert.That(userDto.Status, Is.EqualTo("Banned"));
    }

    [Test]
    public async Task DeleteAsync_ShouldCallDeleteAndSaveChanges()
    {
        // Arrange
        var id = Guid.NewGuid();
        _userRepository.Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
            .Returns(Task.CompletedTask);
        _unitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        
        // Act
        await _userService.DeleteAsync(id);
        
        // Assert
        _userRepository.Verify(x => x.DeleteAsync(id), Times.Once);
        _unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Test]
    public async Task BanUserAsync_ShouldReturnTrue_WhenUserIsBannedSuccessfully()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userRepository.Setup(r => r.BanAsync(userId)).ReturnsAsync(true);
        _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _userService.BanUserAsync(userId);

        // Assert
        Assert.That(result, Is.True);
    }
    
    [Test]
    public async Task BanUserAsync_ShouldReturnFalse_WhenUserNotFoundOrAlreadyBanned()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userRepository.Setup(r => r.BanAsync(userId)).ReturnsAsync(false);

        // Act
        var result = await _userService.BanUserAsync(userId);

        // Assert
        Assert.That(result, Is.False);
    }
}