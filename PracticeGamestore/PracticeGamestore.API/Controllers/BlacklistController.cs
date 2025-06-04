using Microsoft.AspNetCore.Mvc;
using PracticeGamestore.Business.Services.Blacklist;
using PracticeGamestore.Business.Services.Country;
using PracticeGamestore.Mappers;
using PracticeGamestore.Models.Blacklist;

namespace PracticeGamestore.Controllers;

[ApiController, Route("blacklists")]
public class BlacklistController(
    IBlacklistService blacklistService,
    ICountryService countryService,
    ILogger<BlacklistController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var blacklists = await blacklistService.GetAllAsync();
        return Ok(blacklists.Select(b => b.MapToBlacklistModel()));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var blacklist = await blacklistService.GetByIdAsync(id);
        
        if (blacklist is null)
        {
            logger.LogError("Blacklist with id: {Id} was not found.", id);
            return NotFound($"Blacklist with id: {id} was not found.");
        }
        
        return Ok(blacklist.MapToBlacklistModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BlacklistRequestModel model)
    {
        if (await countryService.GetByIdAsync(model.CountryId) is null)
        {
            logger.LogError("Country with id: {CountryId} does not exist.", model.CountryId);
            return BadRequest($"Country with id: {model.CountryId} does not exist.");
        }
        
        var createdId = await blacklistService.CreateAsync(model.MapToBlacklistDto());
        
        if (createdId is null)
        {
            logger.LogError("Failed to create blacklist for model: {Model}", model);
            return BadRequest("Failed to create blacklist.");
        }
        
        logger.LogInformation("Created blacklist with id: {Id}", createdId);
        return CreatedAtAction(nameof(GetById), new { id = createdId }, createdId);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] BlacklistRequestModel model)
    {
        if (await countryService.GetByIdAsync(model.CountryId) is null)
        {
            logger.LogError("Country with id: {CountryId} does not exist.", model.CountryId);
            return BadRequest($"Country with id: {model.CountryId} does not exist.");
        }
        
        var isUpdated = await blacklistService.UpdateAsync(id, model.MapToBlacklistDto());
        
        if (!isUpdated)
        {
            logger.LogError("Failed to update blacklist with id: {Id}", id);
            return  BadRequest($"Update failed. Blacklist with id: {id} might not exist.");
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await blacklistService.DeleteAsync(id);
        return NoContent();
    }
}