using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Services.Auth;
using PracticeGamestore.Business.Services.Country;
using PracticeGamestore.Business.Services.Location;
using PracticeGamestore.Business.Services.User;
using PracticeGamestore.Controllers;
using PracticeGamestore.Models.Auth;

namespace PracticeGamestore.Tests.Unit.Auth;

[TestFixture]
public class AuthControllerTests
{
    private Mock<IUserService> _userServiceMock;
    private Mock<ICountryService> _countryServiceMock;
    private Mock<ILocationService> _locationServiceMock;
    private Mock<ILogger<AuthController>> _loggerMock;
    private Mock<IAuthService> _authServiceMock;
    private AuthController _authController;

    [SetUp]
    public void Setup()
    {
        _userServiceMock = new Mock<IUserService>();
        _countryServiceMock = new Mock<ICountryService>();
        _locationServiceMock = new Mock<ILocationService>();
        _loggerMock = new Mock<ILogger<AuthController>>();
        _authServiceMock = new Mock<IAuthService>();
        _authController = new AuthController(
            _userServiceMock.Object,
            _countryServiceMock.Object,
            _locationServiceMock.Object,
            _loggerMock.Object,
            _authServiceMock.Object);
    }
    
    [Test]
    public async Task Register_WhenValidRequest_ShouldReturnCreated()
    {
        // Arrange
        var userRequest = TestData.User.GenerateUserRequestModel();
        var country = TestData.Country.GenerateCountryDto();
        var userId = Guid.NewGuid();
        
        _countryServiceMock.Setup(x => x.GetByIdAsync(userRequest.CountryId))
            .ReturnsAsync(country);
        _locationServiceMock.Setup(x => x.HandleLocationAccessAsync(country.Name, userRequest.Email))
            .Returns(Task.CompletedTask);
        _userServiceMock.Setup(x => x.CreateAsync(It.IsAny<UserDto>()))
            .ReturnsAsync(userId);

        // Act
        var result = await _authController.Register(userRequest);

        // Assert
        var createdResult = result as CreatedAtActionResult;
        Assert.That(createdResult, Is.Not.Null);
        Assert.That(createdResult!.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
        Assert.That(createdResult.Value, Is.EqualTo(userId));
        Assert.That(createdResult.ActionName, Is.EqualTo("GetById"));
        Assert.That(createdResult.ControllerName, Is.EqualTo("User"));
        Assert.That(createdResult.RouteValues!["id"], Is.EqualTo(userId));

        _userServiceMock.Verify(x => x.CreateAsync(It.Is<UserDto>(dto =>
            dto.UserName == userRequest.UserName &&
            dto.Email == userRequest.Email &&
            dto.PhoneNumber == userRequest.PhoneNumber &&
            dto.Password == userRequest.Password &&
            dto.Role == userRequest.Role &&
            dto.CountryId == userRequest.CountryId &&
            dto.BirthDate == userRequest.BirthDate)), Times.Once);
    }

    [Test]
    public async Task Register_WhenUserCreationFails_ShouldReturnBadRequestAndLogError()
    {
        // Arrange
        var userRequest = TestData.User.GenerateUserRequestModel();
        var country = TestData.Country.GenerateCountryDto();
        
        _countryServiceMock.Setup(x => x.GetByIdAsync(userRequest.CountryId))
            .ReturnsAsync(country);
        _locationServiceMock.Setup(x => x.HandleLocationAccessAsync(country.Name, userRequest.Email))
            .Returns(Task.CompletedTask);
        _userServiceMock.Setup(x => x.CreateAsync(It.IsAny<UserDto>()))
            .ReturnsAsync(null as Guid?);

        // Act
        var result = await _authController.Register(userRequest);

        // Assert
        var badRequestResult = result as BadRequestObjectResult;
        Assert.That(badRequestResult, Is.Not.Null);
        Assert.That(badRequestResult!.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
        Assert.That(badRequestResult.Value, Is.EqualTo(ErrorMessages.RegistrationFailed));

        _countryServiceMock.Verify(x => x.GetByIdAsync(userRequest.CountryId), Times.Once);
        _locationServiceMock.Verify(x => x.HandleLocationAccessAsync(country.Name, userRequest.Email), Times.Once);
        _userServiceMock.Verify(x => x.CreateAsync(It.IsAny<UserDto>()), Times.Once);
        
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains(ErrorMessages.RegistrationFailed)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Test]
    public async Task Register_WhenLocationServiceThrowsArgumentExceptionBecauseProvidedCountryWasNotFound_ShouldReturnBadRequestAndLogError()
    {
        // Arrange
        var userRequest = TestData.User.GenerateUserRequestModel();
        var country = TestData.Country.GenerateCountryDto();
        
        _countryServiceMock.Setup(x => x.GetByIdAsync(userRequest.CountryId))
            .ReturnsAsync(country);
        _locationServiceMock.Setup(x => x.HandleLocationAccessAsync(country.Name, userRequest.Email))
            .ThrowsAsync(new ArgumentException());

        // Act
        var result = await _authController.Register(userRequest);

        // Assert
        var badRequestResult = result as BadRequestObjectResult;
        Assert.That(badRequestResult, Is.Not.Null);
        Assert.That(badRequestResult!.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
        Assert.That(badRequestResult.Value, Is.EqualTo(ErrorMessages.InvalidRegistrationCountry));

        _userServiceMock.Verify(x => x.CreateAsync(It.IsAny<UserDto>()), Times.Never);
        
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains(ErrorMessages.InvalidRegistrationCountry)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Test]
    public async Task Register_WhenLocationServiceThrowsUnauthorizedAccessExceptionBecauseUserCountryIsBanned_ShouldContinueProcessingAndLogWarning()
    {
        // Arrange
        var userRequest = TestData.User.GenerateUserRequestModel();
        var country = TestData.Country.GenerateCountryDto();
        var userId = Guid.NewGuid();
        
        _countryServiceMock.Setup(x => x.GetByIdAsync(userRequest.CountryId))
            .ReturnsAsync(country);
        _locationServiceMock.Setup(x => x.HandleLocationAccessAsync(country.Name, userRequest.Email))
            .ThrowsAsync(new UnauthorizedAccessException());
        _userServiceMock.Setup(x => x.CreateAsync(It.IsAny<UserDto>()))
            .ReturnsAsync(userId);

        // Act
        var result = await _authController.Register(userRequest);

        // Assert
        var createdResult = result as CreatedAtActionResult;
        Assert.That(createdResult, Is.Not.Null);
        Assert.That(createdResult!.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
        Assert.That(createdResult.Value, Is.EqualTo(userId));

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("User with email") && 
                                              o.ToString()!.Contains("is registering from banned country") &&
                                              o.ToString()!.Contains("added to blacklist")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }
    
    
    [Test]
    public async Task Login_WhenCredentialsAreValid_ShouldReturnOkWithToken()
    {
        // Arrange
        var loginRequest = new LoginRequest 
        { 
            Email = "test@example.com", 
            Password = "password123" 
        };
        var userId = Guid.NewGuid();
        var tokenDto = new TokenResponseDto(userId, "valid-jwt-token", 60);
        
        _authServiceMock.Setup(x => x.AuthenticateUser(loginRequest.Email, loginRequest.Password))
            .ReturnsAsync(tokenDto);

        // Act
        var result = await _authController.Login(loginRequest);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult!.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        
        var tokenResponse = okResult.Value as TokenResponseModel;
        Assert.That(tokenResponse, Is.Not.Null);
        Assert.That(tokenResponse!.Token, Is.EqualTo(tokenDto.Token));
        Assert.That(tokenResponse.Expiration, Is.EqualTo(tokenDto.Expiration));
    }

    [Test]
    public async Task Login_WhenCredentialsAreInvalid_ShouldReturnUnauthorized()
    {
        // Arrange
        var loginRequest = new LoginRequest 
        { 
            Email = "test@example.com", 
            Password = "wrongpassword" 
        };
        
        _authServiceMock.Setup(x => x.AuthenticateUser(loginRequest.Email, loginRequest.Password))
            .ReturnsAsync(null as TokenResponseDto);

        // Act
        var result = await _authController.Login(loginRequest);

        // Assert
        var unauthorizedResult = result as UnauthorizedObjectResult;
        Assert.That(unauthorizedResult, Is.Not.Null);
        Assert.That(unauthorizedResult!.StatusCode, Is.EqualTo(StatusCodes.Status401Unauthorized));
        Assert.That(unauthorizedResult.Value, Is.EqualTo(ErrorMessages.Unauthorized));
    }
}