using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Enums;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.Business.Services.Country;
using PracticeGamestore.DataAccess.Repositories.Country;
using PracticeGamestore.DataAccess.UnitOfWork;

namespace PracticeGamestore.Tests.Unit.Country;

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
    public async Task GetAllAsync_WhenCountriesExist_ShouldReturnAllCountries()
    {
        // Arrange
        var countries = TestData.Country.GenerateCountryEntities();
        
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
    public async Task GetByIdAsync_WhenCountryExists_ShouldReturnCountryDto()
    {
        // Arrange
        var country = TestData.Country.GenerateCountryEntity();
        
        _countryRepository.Setup(repo => repo.GetByIdAsync(country.Id)).ReturnsAsync(country);
        
        // Act
        var result = await _countryService.GetByIdAsync(country.Id);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result?.Id, Is.EqualTo(country.Id));
        Assert.That(result?.Name, Is.EqualTo(country.Name));
    }

    [Test]
    public async Task GetByIdAsync_WhenCountryDoesNotExists_ShouldReturnNull()
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
    public async Task GetByNameAsync_WhenCountryExists_ShouldReturnCountryDto()
    {
        // Arrange
        var country = TestData.Country.GenerateCountryEntity();
        
        _countryRepository.Setup(repo => repo.GetByNameAsync(country.Name)).ReturnsAsync(country);
        
        // Act
        var result = await _countryService.GetByNameAsync(country.Name);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result?.Id, Is.EqualTo(country.Id));
        Assert.That(result?.Name, Is.EqualTo(country.Name));
    }
    
    [Test]
    public async Task GetByNameAsync_WhenCountryDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        _countryRepository.Setup(repo => repo.GetByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(null as DataAccess.Entities.Country);
        
        // Act
        var result = await _countryService.GetByNameAsync(It.IsAny<string>());
        
        // Assert
        Assert.That(result, Is.Null);
    }
    
    [Test]
    public async Task CreateAsync_WhenChangesSavedSuccessfully_ShouldAddCountry()
    {
        // Arrange
        var countryDto = TestData.Country.GenerateCountryDto();
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
    public async Task CreateAsync_WhenChangesNotSaved_ShouldReturnNull()
    {
        // Arrange
        var countryDto = TestData.Country.GenerateCountryDto();
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
    public void CreateAsync_WhenNameAlreadyExists_ThrowsArgumentException()
    {
        // Arrange
        var countryDto = TestData.Country.GenerateCountryDto();
        
        _countryRepository.Setup(c => c.ExistsByNameAsync(countryDto.Name)).ReturnsAsync(true);
    
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() => _countryService.CreateAsync(countryDto));
    }
    
    [Test]
    public async Task UpdateAsync_WhenCountryUpdatedSuccessfully_ShouldReturnTrue()
    {
        // Arrange
        var countryDto = TestData.Country.GenerateCountryDto();
        var country = TestData.Country.GenerateCountryEntity();
        _countryRepository
            .Setup(c => c.GetByIdAsync(countryDto.Id!.Value))
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
    public async Task UpdateAsync_WhenCountryNotFound_ShouldReturnFalse()
    {
        // Arrange
        var countryDto = TestData.Country.GenerateCountryDto();
        _countryRepository
            .Setup(c => c.GetByIdAsync(countryDto.MapToCountryEntity().Id))
            .ReturnsAsync(null as DataAccess.Entities.Country);
        
        // Act
        var result = await _countryService.UpdateAsync(countryDto);
        
        // Assert
        Assert.That(result, Is.False);
    }
    
    [Test]
    public async Task UpdateAsync_WhenChangesNotSaved_ShouldReturnFalse()
    {
        // Arrange
        var countryDto = TestData.Country.GenerateCountryDto();
        var country = TestData.Country.GenerateCountryEntity();
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
    public void UpdateAsync_WhenNameAlreadyExists_ThrowsArgumentException()
    {
        // Arrange
        var countryDto = TestData.Country.GenerateCountryDto();
        var country = TestData.Country.GenerateCountryEntity();
        
        _countryRepository.Setup(c => c.GetByIdAsync(countryDto.Id!.Value)).ReturnsAsync(country);
        _countryRepository.Setup(c => c.ExistsByNameAsync(countryDto.Name)).ReturnsAsync(true);
    
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() => _countryService.UpdateAsync(countryDto));
    }
    
    [Test]
    public async Task DeleteAsync_WhenCountryIsDeleted_ShouldCallDeleteAndSaveChanges()
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