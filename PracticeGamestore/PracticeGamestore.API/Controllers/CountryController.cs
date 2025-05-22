using Microsoft.AspNetCore.Mvc;
using PracticeGamestore.API.Mappers;
using PracticeGamestore.API.Models;
using PracticeGamestore.Business.Services.Country;

namespace PracticeGamestore.API.Controllers;

[ApiController, Route("countries")]
public class CountryController(ICountryService countryService): ControllerBase
{
    
    [HttpGet]
    public async Task<IActionResult> GetAllCountries()
    {
        try {
            var countries = await countryService.GetAllAsync();
            return Ok(countries.Select(c => c.MapToResponseModel()));
        }
        catch (Exception ex) {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCountryById(Guid id)
    {
        try {
            var country = await countryService.GetByIdAsync(id);
            return country == null ? NotFound($"Country with id {id} not found") : Ok(country.MapToResponseModel());
        }
        catch (Exception ex) {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateCountry([FromBody] CountryRequestModel countryRequestModel)
    {
        try {
            var countryDto = countryRequestModel.MapToDto();
            var id = await countryService.CreateAsync(countryDto);
            return id == null ? BadRequest("Failed to create country") 
                : CreatedAtAction(nameof(GetCountryById), new { id }, countryDto.MapToResponseModel());
        }
        catch (Exception ex) {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCountry(Guid id, [FromBody] CountryRequestModel countryRequestModel)
    {
        try {
            var countryDto = countryRequestModel.MapToDto();
            countryDto.Id = id;
            var updated = await countryService.UpdateAsync(countryDto);
            if (!updated) return BadRequest($"Country with id {id} not found");
            return NoContent();
        }
        catch (Exception ex) {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCountry(Guid id)
    {
        try {
            await countryService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex) {
            return BadRequest(ex.Message);
        }
    }
}