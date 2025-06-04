using Microsoft.AspNetCore.Mvc;
using PracticeGamestore.API.Mappers;
using PracticeGamestore.API.Models;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Services.Country;
using PracticeGamestore.DataAccess.Enums;

namespace PracticeGamestore.Controllers;

[ApiController, Route("countries")]
public class CountryController(ICountryService countryService) : ControllerBase
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
        return (country is null)
            ? NotFound($"Country with id {id} not found") 
            : Ok(country.MapToCountryModel());
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateCountry([FromBody] CountryCreateRequestModel countryCreateRequestModel)
    {
        var countryDto = new CountryDto(null, countryCreateRequestModel.Name, CountryStatus.Allowed);
        var id = await countryService.CreateAsync(countryDto);
        
        if (id is null)
        {
            return BadRequest("Failed to create country.");
        }
        
        countryDto.Id = id.Value;
        return CreatedAtAction(nameof(GetCountryById), new { id = countryDto.Id }, id);
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCountry(Guid id, [FromBody] CountryUpdateRequestModel countryUpdateRequestModel)
    {
        var countryDto = countryUpdateRequestModel.MapToCountryDto();
        countryDto.Id = id;
        
        var updated = await countryService.UpdateAsync(countryDto);
        return !updated 
            ? BadRequest($"Country with id {id} not found")
            : NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCountry(Guid id)
    {
        await countryService.DeleteAsync(id);
        return NoContent();
    }
}