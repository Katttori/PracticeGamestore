using Microsoft.AspNetCore.Mvc;
using PracticeGamestore.API.Mappers;
using PracticeGamestore.API.Models;
using PracticeGamestore.Business.Services.Country;

namespace PracticeGamestore.API.Controllers;

[ApiController, Route("countries")]
public class CountryController(ICountryService countryService) : ControllerBase
{
    
    [HttpGet]
    public async Task<IActionResult> GetAllCountries()
    {
        var countries = await countryService.GetAllAsync();
        return Ok(countries.Select(c => c.MapToResponseModel()));
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCountryById(Guid id)
    {
        var country = await countryService.GetByIdAsync(id);
        return country == null ? NotFound($"Country with id {id} not found") : Ok(country.MapToResponseModel());
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateCountry([FromBody] CountryRequestModel countryRequestModel)
    {
        var countryDto = countryRequestModel.MapToDto();
        var id = await countryService.CreateAsync(countryDto);
        return id == null ? BadRequest("Failed to create country") 
            : CreatedAtAction(nameof(GetCountryById), new { id }, countryDto.MapToResponseModel());
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCountry(Guid id, [FromBody] CountryRequestModel countryRequestModel)
    {
        var countryDto = countryRequestModel.MapToDto();
        countryDto.Id = id;
        var updated = await countryService.UpdateAsync(countryDto);
        if (!updated) return BadRequest($"Country with id {id} not found");
        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCountry(Guid id)
    {
        await countryService.DeleteAsync(id);
        return NoContent();
    }
}