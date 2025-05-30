using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PracticeGamestore.API.Models;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.Business.Services.Country;
using PracticeGamestore.Controllers;
using PracticeGamestore.DataAccess.Enums;
using PracticeGamestore.DataAccess.Repositories.Country;

namespace PracticeGamestore.API.Tests.Unit.Country;

public class CountryControllerTests
{
    private Mock<ICountryService> _countryService;
    private CountryController _countryController;
    
    [SetUp]
    public void Setup()
    {
        _countryService = new Mock<ICountryService>();
        _countryController = new CountryController(_countryService.Object);
    }
    
    [Test]
    public async Task GetAllCountrys_ShouldReturnOkResult_WhenCountrysExist()
    {
        // Arrange
        var countries = new List<CountryDto>
        {
            new(Guid.NewGuid(), "Canada", CountryStatus.Allowed),
            new(Guid.NewGuid(), "USA", CountryStatus.Allowed)
        };
        
        _countryService.Setup(service => service.GetAllAsync()).ReturnsAsync(countries);
        
        // Act
        var result = await _countryController.GetAllCountries();
        
        // Assert
        var okResult = result as OkObjectResult;
        
        var response = (okResult?.Value as IEnumerable<CountryResponseModel> 
                        ?? []).ToList();
        
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult?.StatusCode, Is.EqualTo(200));
        Assert.That(response.Count, Is.EqualTo(countries.Count));
        Assert.That(response[0].Id, Is.EqualTo(countries[0].Id));
        Assert.That(response[0].Name, Is.EqualTo(countries[0].Name));
    }
    
    [Test]
    public async Task GetCountryById_ShouldReturnOkResult_WhenCountryExists()
    {
        // Arrange
        var country = new CountryDto(Guid.NewGuid(), "Canada", CountryStatus.Allowed);
        
        _countryService.Setup(service => service.GetByIdAsync(country.MapToCountryEntity().Id)).ReturnsAsync(country);
        
        // Act
        var result = await _countryController.GetCountryById(country.MapToCountryEntity().Id);
        
        // Assert
        var okResult = result as OkObjectResult;
        
        var response = okResult?.Value as CountryResponseModel;
        
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult?.StatusCode, Is.EqualTo(200));
        Assert.That(response?.Id, Is.EqualTo(country.Id));
        Assert.That(response?.Name, Is.EqualTo(country.Name));
    }
    
    [Test]
    public async Task GetCountryById_ShouldReturnNotFound_WhenCountryDoesNotExist()
    {
        // Arrange
        var countryId = Guid.NewGuid();
        
        _countryService.Setup(service => service.GetByIdAsync(countryId)).ReturnsAsync((CountryDto?)null);
        
        // Act
        var result = await _countryController.GetCountryById(countryId);
        
        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }
    
    [Test]
    public async Task CreateCountry_ShouldReturnCreatedResult_WhenCountryIsCreated()
    {
        // Arrange
        var countryRequest = new CountryCreateRequestModel
        {
            Name = "Canada"
        };
        
        var countryDto = new CountryDto(Guid.NewGuid(), countryRequest.Name, CountryStatus.Allowed);
        
        _countryService.Setup(service => service.CreateAsync(It.IsAny<CountryDto>()))
            .ReturnsAsync(countryDto.Id);
        
        // Act
        var result = await _countryController.CreateCountry(countryRequest);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
    }
    
    [Test]
    public async Task CreateCountry_ShouldReturnBadRequest_WhenCreationFails()
    {
        // Arrange
        var countryRequest = new CountryCreateRequestModel
        {
            Name = "USA"
        };
        
        _countryService.Setup(service => service.CreateAsync(It.IsAny<CountryDto>()))
            .ReturnsAsync((Guid?)null);
        
        // Act
        var result = await _countryController.CreateCountry(countryRequest);
        
        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }
    
    [Test]
    public async Task Update_ShouldReturnNoContent_WhenCountryIsUpdated()
    {
        // Arrange
        _countryService.Setup(s => s.UpdateAsync(It.IsAny<CountryDto>())).ReturnsAsync(true);
        
        // Act
        var result = await _countryController
            .UpdateCountry(Guid.NewGuid(), new CountryUpdateRequestModel {Name = "UK", Status = CountryStatus.Banned});
        
        // Assert
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task Update_ShouldReturnBadRequest_WhenUpdateFails()
    {
        // Arrange
        _countryService.Setup(s => s.UpdateAsync(It.IsAny<CountryDto>())).ReturnsAsync(false);
        
        // Act
        var result = await _countryController
            .UpdateCountry(Guid.NewGuid(), new CountryUpdateRequestModel {Name = "UKR", Status = CountryStatus.Allowed});
        
        // Assert
        var badRequest = result as BadRequestObjectResult;
        
        Assert.That(badRequest, Is.Not.Null);
    }
    
    [Test]
    public async Task Delete_ShouldReturnNoContent_WhenCountryIsDeleted()
    {
        // Arrange
        var countryId = Guid.NewGuid();
        
        _countryService.Setup(s => s.DeleteAsync(countryId));
        
        // Act
        var result = await _countryController.DeleteCountry(countryId);
        
        // Assert
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }
}