using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Enums;
using PracticeGamestore.Business.Services.Blacklist;
using PracticeGamestore.Business.Services.Country;
using PracticeGamestore.Business.Services.Location;

namespace PracticeGamestore.Tests.Unit.Location;

[TestFixture]
public class LocationServiceTests
{
    private Mock<ICountryService> _countryServiceMock;
    private Mock<IBlacklistService> _blacklistServiceMock;
    private DefaultHttpContext _httpContext;
    private LocationService _locationService;

    [SetUp]
    public void SetUp()
    {
        _countryServiceMock = new Mock<ICountryService>();
        _blacklistServiceMock = new Mock<IBlacklistService>();
        _httpContext = new DefaultHttpContext();
        _locationService = new LocationService(_countryServiceMock.Object, _blacklistServiceMock.Object);
    }

    [Test]
    public async Task HandleLocationAccessAsync_WhenCountryNotFound_CreatesNewCountry()
    {
        // Arrange
        _httpContext.Request.Headers["X-Location-Country"] = "Narnia";
        _httpContext.Request.Headers["X-User-Email"] = "test@gmail.com";

        _countryServiceMock.Setup(s => s.GetByNameAsync("Narnia")).ReturnsAsync(null as CountryDto);
        
        // Act
        await _locationService.HandleLocationAccessAsync(_httpContext);

        // Assert
        _countryServiceMock.Verify(s => s.CreateAsync(It.Is<CountryDto>(dto =>
            dto.Name == "Narnia" && dto.Status == CountryStatus.Allowed)), Times.Once);
    }

    [Test]
    public void HandleLocationAccessAsync_WhenHeadersAreMissing_ThrowsArgumentException()
    {
        // Arrange
        var context = new DefaultHttpContext();

        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(() => _locationService.HandleLocationAccessAsync(context));
        Assert.That(ex?.Message, Is.EqualTo(ErrorMessages.MissingLocationHeaders));
    }

    [Test]
    public void HandleLocationAccessAsync_WhenUserIsBlacklisted_ThrowsUnauthorized()
    {
        // Arrange
        _httpContext.Request.Headers["X-Location-Country"] = "Canada";
        _httpContext.Request.Headers["X-User-Email"] = "banned@gmail.com";

        _countryServiceMock.Setup(s => s.GetByNameAsync("Canada"))
            .ReturnsAsync(TestData.Country.GenerateCountryDto());
        _blacklistServiceMock.Setup(s => s.ExistsByUserEmailAsync("banned@gmail.com"))
            .ReturnsAsync(true);

        // Act & Assert
        var ex = Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _locationService.HandleLocationAccessAsync(_httpContext));
        
        Assert.That(ex?.Message, Is.EqualTo(ErrorMessages.BlacklistedUser));
    }

    [Test]
    public void HandleLocationAccessAsync_WhenCountryIsBanned_AddsToBlacklistAndThrows()
    {
        // Arrange
        var bannedCountry = TestData.Country.GenerateBannedCountryDto();

        _httpContext.Request.Headers["X-Location-Country"] = bannedCountry.Name;
        _httpContext.Request.Headers["X-User-Email"] = "user@example.com";

        _countryServiceMock.Setup(s => s.GetByNameAsync(bannedCountry.Name)).ReturnsAsync(bannedCountry);
        _blacklistServiceMock.Setup(s => s.ExistsByUserEmailAsync("user@example.com")).ReturnsAsync(false);

        // Act & Assert
        var ex = Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _locationService.HandleLocationAccessAsync(_httpContext));
        
        _blacklistServiceMock.Verify(s => s.CreateAsync(It.Is<BlacklistDto>(dto =>
            dto.UserEmail == "user@example.com" && dto.CountryId == bannedCountry.Id)), Times.Once);
        
        Assert.That(ex?.Message, Is.EqualTo(ErrorMessages.BlacklistedUser));
    }
}
