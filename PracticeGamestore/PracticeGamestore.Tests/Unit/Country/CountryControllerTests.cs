using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.Business.Services.Country;
using PracticeGamestore.Controllers;
using PracticeGamestore.Models.Country;

namespace PracticeGamestore.Tests.Unit.Country;

public class CountryControllerTests
{
    private Mock<ICountryService> _countryService;
    private Mock<ILogger<CountryController>> _loggerMock;
    private CountryController _countryController;
    
    [SetUp]
    public void Setup()
    {
        _countryService = new Mock<ICountryService>();
        _loggerMock = new Mock<ILogger<CountryController>>();
        _countryController = new CountryController(_countryService.Object, _loggerMock.Object);
    }
    
    [Test]
    public async Task GetAllCountries_WhenCountriesExist_ShouldReturnOkResult()
    {
        // Arrange
        var countries = TestData.Country.GenerateCountryDtos();
        
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
    public async Task GetCountryById_WhenCountryExists_ShouldReturnOkResult()
    {
        // Arrange
        var country = TestData.Country.GenerateCountryDto();
        
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
    public async Task GetCountryById_WhenCountryDoesNotExist_ShouldReturnNotFound()
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
    public async Task CreateCountry_WhenCountryIsCreated_ShouldReturnCreatedResult()
    {
        // Arrange
        var countryRequest = TestData.Country.GenerateCountryCreateRequestModel();
        var countryDto = TestData.Country.GenerateCountryDto();
        
        _countryService.Setup(service => service.CreateAsync(It.IsAny<CountryDto>()))
            .ReturnsAsync(countryDto.Id);
        
        // Act
        var result = await _countryController.CreateCountry(countryRequest);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
    }
    
    [Test]
    public async Task CreateCountry_WhenCreationFails_ShouldReturnBadRequest()
    {
        // Arrange
        var countryRequest = TestData.Country.GenerateCountryCreateRequestModel();
        
        _countryService.Setup(service => service.CreateAsync(It.IsAny<CountryDto>()))
            .ReturnsAsync((Guid?)null);
        
        // Act
        var result = await _countryController.CreateCountry(countryRequest);
        
        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }
    
    [Test]
    public async Task Update_WhenCountryIsUpdated_ShouldReturnNoContent()
    {
        // Arrange
        _countryService.Setup(s => s.UpdateAsync(It.IsAny<CountryDto>())).ReturnsAsync(true);
        
        // Act
        var result = await _countryController
            .UpdateCountry(Guid.NewGuid(), TestData.Country.GenerateCountryUpdateRequestModel());
        
        // Assert
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task Update_WhenUpdateFails_ShouldReturnBadRequest()
    {
        // Arrange
        _countryService.Setup(s => s.UpdateAsync(It.IsAny<CountryDto>())).ReturnsAsync(false);
        
        // Act
        var result = await _countryController
            .UpdateCountry(Guid.NewGuid(), TestData.Country.GenerateCountryUpdateRequestModel());
        
        // Assert
        var badRequest = result as BadRequestObjectResult;
        
        Assert.That(badRequest, Is.Not.Null);
    }
    
    [Test]
    public async Task Delete_WhenCountryIsDeleted_ShouldReturnNoContent()
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