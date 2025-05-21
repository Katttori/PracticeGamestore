using Microsoft.AspNetCore.Mvc;
using PracticeGamestore.API.Mappers;
using PracticeGamestore.API.Models;
using PracticeGamestore.Business.Services.Country;

namespace PracticeGamestore.API.Controllers;

[ApiController, Route("countries")]
public class CountryController: ControllerBase
{
    private readonly ICountryService _countryService;
    
    public CountryController(ICountryService countryService)
    {
        _countryService = countryService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllCountries()
    {
        try {
            var countries = await _countryService.GetAllAsync();
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
            var country = await _countryService.GetByIdAsync(id);
            if (country == null) return NotFound($"Country with id {id} not found");
            return Ok(country.MapToResponseModel());
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
            var id = await _countryService.CreateAsync(countryDto);
            if (id is null) return BadRequest("Failed to create country");
            return CreatedAtAction(nameof(GetCountryById), new { id }, countryDto.MapToResponseModel());
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
            var updated = await _countryService.UpdateAsync(countryDto);
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
            await _countryService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex) {
            return BadRequest(ex.Message);
        }
    }
}