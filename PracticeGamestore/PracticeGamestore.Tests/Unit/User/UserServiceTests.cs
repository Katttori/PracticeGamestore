﻿using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.Business.Services.User;
using PracticeGamestore.Business.Enums;
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
    public async Task GetAllAsync_WhenUsersExist_ShouldReturnAllUsers()
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
    public async Task GetByIdAsync_WhenUserExists_ShouldReturnUserDto()
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
    public async Task GetByIdAsync_WhenUserDoesNotExist_ShouldReturnNull()
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
    public async Task UpdateAsync_WhenUserExistsAndChangesSavedSuccessfully_ShouldReturnTrue()
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
    public async Task UpdateAsync_WhenUserDoesNotExist_ShouldReturnFalse()
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
    public async Task CreateAsync_WhenChangesSavedSuccessfully_ShouldAddUser()
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
    public async Task CreateAsync_WhenChangesNotSaved_ShouldReturnNull()
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
    public async Task CreateAsync_WhenUserInBlacklist_ShouldReturnStatusBanned()
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
        Assert.That(userDto.Status, Is.EqualTo(UserStatus.Banned));
    }
    
    [Test]
    public async Task CreateAsync_WhenCountryMismatchInBlacklist_ShouldSetStatusBanned()
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
        Assert.That(userDto.Status, Is.EqualTo(UserStatus.Banned));
    }

    [Test]
    public async Task DeleteAsync_WhenUserIsDeleted_ShouldCallDeleteAndSaveChanges()
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
    public async Task BanUserAsync_WhenUserIsBannedSuccessfully_ShouldReturnTrue()
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
    public async Task BanUserAsync_WhenUserNotFoundOrAlreadyBanned_ShouldReturnFalse()
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