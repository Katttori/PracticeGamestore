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
    private LocationService _locationService;

    [SetUp]
    public void SetUp()
    {
        _countryServiceMock = new Mock<ICountryService>();
        _blacklistServiceMock = new Mock<IBlacklistService>();
        _locationService = new LocationService(_countryServiceMock.Object, _blacklistServiceMock.Object);
    }

    [Test]
    public async Task HandleLocationAccessAsync_WhenCountryNotFound_ShouldCreateNewCountry()
    {
        // Arrange
        const string countryName = "Narnia";
        const string userEmail = "test@gmail.com";

        _countryServiceMock.Setup(s => s.GetByNameAsync("Narnia")).ReturnsAsync(null as CountryDto);
        
        // Act
        await _locationService.HandleLocationAccessAsync(countryName, userEmail);

        // Assert
        _countryServiceMock.Verify(s => s.CreateAsync(It.Is<CountryDto>(dto =>
            dto.Name == "Narnia" && dto.Status == CountryStatus.Allowed)), Times.Once);
    }

    [Test]
    public void HandleLocationAccessAsync_WhenLocationHeaderIsMissing_ShouldThrowArgumentException()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(() => _locationService.HandleLocationAccessAsync("", ""));
        Assert.That(ex?.Message, Is.EqualTo(ErrorMessages.MissingLocationHeader));
    }

    [Test]
    public void HandleLocationAccessAsync_WhenCountryIsBanned_ShouldAddToBlacklistAndThrows()
    {
        // Arrange
        var bannedCountry = TestData.Country.GenerateBannedCountryDto();
        const string userEmail = "test@gmail.com";

        _countryServiceMock.Setup(s => s.GetByNameAsync(bannedCountry.Name)).ReturnsAsync(bannedCountry);

        // Act & Assert
        var ex = Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _locationService.HandleLocationAccessAsync(bannedCountry.Name, userEmail));
        
        _blacklistServiceMock.Verify(s => s.CreateAsync(It.Is<BlacklistDto>(dto =>
            dto.UserEmail == userEmail && dto.CountryId == bannedCountry.Id)), Times.Once);
        
        Assert.That(ex?.Message, Is.EqualTo(ErrorMessages.BlacklistedUser));
    }
}
