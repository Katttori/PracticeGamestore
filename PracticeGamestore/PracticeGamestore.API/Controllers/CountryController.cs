using Microsoft.AspNetCore.Mvc;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Enums;
using PracticeGamestore.Business.Services.Country;
using PracticeGamestore.Filters;
using PracticeGamestore.Mappers;
using PracticeGamestore.Models.Country;

namespace PracticeGamestore.Controllers;

[ApiController, Route("countries")]
public class CountryController(ICountryService countryService, ILogger<CountryController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllCountries()
    {
        var countries = await countryService.GetAllAsync();
        return Ok(countries.Select(c => c.MapToCountryModel()));
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCountryById(Guid id)
    {
        var country = await countryService.GetByIdAsync(id);

        if (country is null)
        {
            logger.LogError("Country with id: {Id} not found.", id);
            return NotFound(ErrorMessages.NotFound("Country", id));
        }

        return Ok(country.MapToCountryModel());
    }
    
    [HttpPost]
    [ServiceFilter(typeof(RequestModelValidationFilter))]
    public async Task<IActionResult> CreateCountry([FromBody] CountryCreateRequestModel model)
    {
        var countryDto = new CountryDto(null, model.Name, CountryStatus.Allowed);
        var id = await countryService.CreateAsync(countryDto);
        
        if (id is null)
        {
            logger.LogError("Failed to create country with model: {Model}", model);
            return BadRequest(ErrorMessages.FailedToCreate("country"));
        }
        
        countryDto.Id = id.Value;
        logger.LogInformation("Created country with id: {Id}", countryDto.Id);
        return CreatedAtAction(nameof(GetCountryById), new { id = countryDto.Id }, id);
    }
    
    [HttpPut("{id:guid}")]
    [ServiceFilter(typeof(RequestModelValidationFilter))]
    public async Task<IActionResult> UpdateCountry(Guid id, [FromBody] CountryUpdateRequestModel model)
    {
        var countryDto = model.MapToCountryDto();
        countryDto.Id = id;
        
        var updated = await countryService.UpdateAsync(countryDto);

        if (updated) return NoContent();
        
        logger.LogError("Country with id: {Id} not found for update.", id);
        return BadRequest(ErrorMessages.FailedToUpdate("country", id));
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCountry(Guid id)
    {
        await countryService.DeleteAsync(id);
        return NoContent();
    }
}