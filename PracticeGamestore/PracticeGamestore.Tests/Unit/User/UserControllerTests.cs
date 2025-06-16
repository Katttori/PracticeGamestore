using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Services.User;
using PracticeGamestore.Controllers;
using PracticeGamestore.Mappers;
using PracticeGamestore.Models.User;

namespace PracticeGamestore.Tests.Unit.User;

public class UserControllerTests
{
    private Mock<IUserService> _userService;
    private Mock<ILogger<UserController>> _loggerMock;
    private UserController _userController;
    
    [SetUp]
    public void SetUp()
    {
        _userService = new Mock<IUserService>();
        _loggerMock = new Mock<ILogger<UserController>>();
        _userController = new UserController(_userService.Object, _loggerMock.Object);
    }

    [Test]
    public async Task GetAll_ReturnsOkWithUsers()
    {
        // Arrange
        var userDtos = TestData.User.GenerateUserDtos();
        
        _userService.Setup(x => x.GetAllAsync())
            .ReturnsAsync(userDtos);
        var expected = userDtos.Select(dto => dto.MapToUserResponseModel()).ToList();

        // Act
        var result = await _userController.GetAll();

        // Assert
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult!.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        var response = (okResult.Value as IEnumerable<UserResponseModel> ?? []).ToList();
        Assert.That(response, Is.Not.Null);
        Assert.That(response.Count, Is.EqualTo(expected.Count));
    }

    [Test]
    public async Task GetUserById_WhenUserExists_ShouldReturnOkResult()
    {
        // Arrange
        var id = Guid.NewGuid();
        var userDto = TestData.User.GenerateUserDto();
        _userService.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(userDto);
        var expected = userDto.MapToUserResponseModel();
        
        // Act
        var result = await _userController.GetById(id);
        
        // Assert
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult!.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        var response = okResult.Value as UserResponseModel;
        Assert.That(response, Is.Not.Null);
        Assert.That(response!.Id, Is.EqualTo(expected.Id));
    }

    [Test]
    public async Task GetUserById_WhenUserDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        _userService.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as UserDto);
        
        // Act
        var result = await _userController.GetById(Guid.NewGuid());
        
        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task CreateUser_WhenUserIsCreated_ShouldReturnCreatedResult()
    {
        // Arrange
        var id = Guid.NewGuid();
        var userRequestModel = TestData.User.GenerateUserRequestModel();
        _userService.Setup(x => x.CreateAsync(It.IsAny<UserDto>()))
            .ReturnsAsync(id);
        
        // Act
        var result = await _userController.Create(userRequestModel);
        
        // Assert
        var createdResult = result as CreatedAtActionResult;
        Assert.That(createdResult, Is.Not.Null);
        Assert.That(createdResult!.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
        Assert.That(createdResult.Value, Is.EqualTo(id));
        Assert.That(createdResult.ActionName, Is.EqualTo(nameof(UserController.GetById)));
        Assert.That(createdResult.RouteValues, Is.Not.Null);
    }

    [Test]
    public async Task CreateUser_WhenCreationFails_ShouldReturnBadRequest()
    {
        // Arrange
        var userRequestModel = TestData.User.GenerateUserRequestModel();
        _userService.Setup(x => x.CreateAsync(It.IsAny<UserDto>()))
            .ReturnsAsync(null as Guid?);
        
        // Act
        var result = await _userController.Create(userRequestModel);
        
        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task Update_WhenUserIsUpdated_ShouldReturnNoContent()
    {
        // Arrange
        var id = Guid.NewGuid();
        var userRequestModel = TestData.User.GenerateUserRequestModel();
        _userService.Setup(x => x.UpdateAsync(id, It.IsAny<UserDto>()))
            .ReturnsAsync(true);
        
        // Act
        var result = await _userController.Update(id, userRequestModel);
        
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task Update_WhenUpdateFails_ShouldReturnBadRequest()
    {
        // Arrange
        var id = Guid.NewGuid();
        var userRequestModel = TestData.User.GenerateUserRequestModel();
        _userService.Setup(x => x.UpdateAsync(id, It.IsAny<UserDto>()))
            .ReturnsAsync(false);
        
        // Act
        var result = await _userController.Update(id, userRequestModel);
        
        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task Delete_WhenUserIsDeleted_ShouldReturnNoContent()
    {
        // Arrange
        _userService.Setup(x => x.DeleteAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);
        
        // Act
        var result = await _userController.Delete(Guid.NewGuid());
        
        // Assert
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }
    
    [Test]
    public async Task BanUser_WhenUserIsBanned_ShouldReturnNoContent()
    {
        // Arrange
        var id = Guid.NewGuid();
        _userService.Setup(s => s.BanUserAsync(id)).ReturnsAsync(true);
        
        // Act
        var result = await _userController.Ban(id);

        // Assert
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }
    
    [Test]
    public async Task BanUser_WhenBanFails_ShouldReturnBadRequest()
    {
        // Arrange
        var id = Guid.NewGuid();
        _userService.Setup(s => s.BanUserAsync(id)).ReturnsAsync(false);

        // Act
        var result = await _userController.Ban(id);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }
}