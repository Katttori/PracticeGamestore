using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.Business.Services.Country;
using PracticeGamestore.DataAccess.Repositories.Country;
using PracticeGamestore.DataAccess.UnitOfWork;

namespace PracticeGamestore.API.Tests.Unit.Country;

public class CountryServiceTests
{
    private Mock<ICountryRepository> _countryRepository;
    private Mock<IUnitOfWork> _unitOfWork;
    private ICountryService _countryService;
    
    [SetUp]
    public void Setup()
    {
        _countryRepository = new Mock<ICountryRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _countryService = new CountryService(_countryRepository.Object, _unitOfWork.Object);
    }
    
    [Test]
    public async Task GetAllAsync_ShouldReturnAllCountries()
    {
        // Arrange
        var countries = new List<DataAccess.Entities.Country>
        {
            new DataAccess.Entities.Country
            {
                Id = Guid.NewGuid(),
                Name = "USA",
                CountryStatus = DataAccess.Enums.CountryStatus.Allowed
            },
            new DataAccess.Entities.Country
            {
                Id = Guid.NewGuid(),
                Name = "Canada",
                CountryStatus = DataAccess.Enums.CountryStatus.Allowed
            }
        };
        
        _countryRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(countries);
        
        // Act
        var result = (await _countryService.GetAllAsync()).ToList();
        
        // Assert   
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(countries.Count));
        Assert.That(result[0].Id, Is.EqualTo(countries[0].Id));
        Assert.That(result[0].Name, Is.EqualTo(countries[0].Name));
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnCountryDto_WhenCountryExists()
    {
        // Arrange
        var country = new DataAccess.Entities.Country
        {
            Id = Guid.NewGuid(),
            Name = "USA",
            CountryStatus = DataAccess.Enums.CountryStatus.Allowed
        };
        
        _countryRepository.Setup(repo => repo.GetByIdAsync(country.Id)).ReturnsAsync(country);
        
        // Act
        var result = await _countryService.GetByIdAsync(country.Id);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result?.Id, Is.EqualTo(country.Id));
        Assert.That(result?.Name, Is.EqualTo(country.Name));
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnNull_WhenCountryDoesNotExists()
    {
        // Arrange
        _countryRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as DataAccess.Entities.Country);
        
        // Act
        var result = await _countryService.GetByIdAsync(Guid.NewGuid());
        
        // Assert
        Assert.That(result, Is.Null);
    }
    
    [Test]
    public async Task CreateAsync_ShouldAddCountry_WhenChangesSavedSuccessfully()
    {
        // Arrange
        var countryDto = new CountryDto(Guid.NewGuid(), "Canada", DataAccess.Enums.CountryStatus.Allowed);

        _countryRepository.Setup(c => c.CreateAsync(It.IsAny<DataAccess.Entities.Country>()))
            .ReturnsAsync(countryDto.MapToCountryEntity().Id);
        _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        
        // Act
        var result = await _countryService.CreateAsync(countryDto);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(countryDto.MapToCountryEntity().Id));
    }

    [Test]
    public async Task CreateAsync_ShouldReturnNull_WhenChangesNotSaved()
    {
        // Arrange
        var countryDto = new CountryDto(Guid.NewGuid(), "Canada", DataAccess.Enums.CountryStatus.Allowed);

        _countryRepository
            .Setup(c => c.CreateAsync(It.IsAny<DataAccess.Entities.Country>()))
            .ReturnsAsync(countryDto.MapToCountryEntity().Id);
        
        _unitOfWork
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        // Act
        var result = await _countryService.CreateAsync(countryDto);
        
        // Assert
        Assert.That(result, Is.Null);
    }
    
    [Test]
    public async Task UpdateAsync_ShouldReturnTrue_WhenCountryUpdatedSuccessfully()
    {
        // Arrange
        var countryDto = new CountryDto(Guid.NewGuid(), "Canada", DataAccess.Enums.CountryStatus.Allowed);
        var country = new DataAccess.Entities.Country
        {
            Id = countryDto.MapToCountryEntity().Id,
            Name = countryDto.Name,
            CountryStatus = countryDto.Status
        };
        
        _countryRepository
            .Setup(c => c.GetByIdAsync(country.Id))
            .ReturnsAsync(country);
        
        _unitOfWork
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        
        // Act
        var result = await _countryService.UpdateAsync(countryDto);
        
        // Assert
        Assert.That(result, Is.True);
    }
    
    [Test]
    public async Task UpdateAsync_ShouldReturnFalse_WhenCountryNotFound()
    {
        // Arrange
        var countryDto = new CountryDto(Guid.NewGuid(), "Canada", DataAccess.Enums.CountryStatus.Allowed);
        
        _countryRepository
            .Setup(c => c.GetByIdAsync(countryDto.MapToCountryEntity().Id))
            .ReturnsAsync(null as DataAccess.Entities.Country);
        
        // Act
        var result = await _countryService.UpdateAsync(countryDto);
        
        // Assert
        Assert.That(result, Is.False);
    }
    
    [Test]
    public async Task UpdateAsync_ShouldReturnFalse_WhenChangesNotSaved()
    {
        // Arrange
        var countryDto = new CountryDto(Guid.NewGuid(), "Canada", DataAccess.Enums.CountryStatus.Allowed);
        var country = new DataAccess.Entities.Country
        {
            Id = countryDto.MapToCountryEntity().Id,
            Name = countryDto.Name,
            CountryStatus = countryDto.Status
        };
        
        _countryRepository
            .Setup(c => c.GetByIdAsync(country.Id))
            .ReturnsAsync(country);
        
        _unitOfWork
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);
        
        // Act
        var result = await _countryService.UpdateAsync(countryDto);
        
        // Assert
        Assert.That(result, Is.False);
    }
    
    [Test]
    public async Task DeleteAsync_ShouldCallDeleteAndSaveChanges()
    {
        // Arrange
        var countryId = Guid.NewGuid();
        
        _countryRepository
            .Setup(c => c.DeleteAsync(countryId))
            .Returns(Task.CompletedTask);
        
        _unitOfWork
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        
        // Act
        await _countryService.DeleteAsync(countryId);
        
        // Assert
        _countryRepository.Verify(c => c.DeleteAsync(countryId), Times.Once);
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}