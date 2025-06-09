using Microsoft.AspNetCore.Mvc;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Business.Services.Blacklist;
using PracticeGamestore.Business.Services.Country;
using PracticeGamestore.Filters;
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
            return NotFound(ErrorMessages.NotFound("Blacklist", id));
        }

        return Ok(blacklist.MapToBlacklistModel());
    }

    [HttpPost]
    [ServiceFilter(typeof(RequestModelValidationFilter))]
    public async Task<IActionResult> Create([FromBody] BlacklistRequestModel model)
    {
        if (await countryService.GetByIdAsync(model.CountryId) is null)
        {
            logger.LogError("Country with id: {CountryId} does not exist.", model.CountryId);
            return BadRequest(ErrorMessages.NotFound("Country", model.CountryId));
        }
        
        var createdId = await blacklistService.CreateAsync(model.MapToBlacklistDto());
        
        if (createdId is null)
        {
            logger.LogError("Failed to create blacklist for model: {Model}", model);
            return BadRequest(ErrorMessages.FailedToCreate("blacklist"));
        }
        
        logger.LogInformation("Created blacklist with id: {Id}", createdId);
        return CreatedAtAction(nameof(GetById), new { id = createdId }, createdId);
    }

    [HttpPut("{id:guid}")]
    [ServiceFilter(typeof(RequestModelValidationFilter))]
    public async Task<IActionResult> Update(Guid id, [FromBody] BlacklistRequestModel model)
    {
        if (await countryService.GetByIdAsync(model.CountryId) is null)
        {
            logger.LogError("Country with id: {CountryId} does not exist.", model.CountryId);
            return BadRequest(ErrorMessages.NotFound("Country", model.CountryId));
        }
        
        var isUpdated = await blacklistService.UpdateAsync(id, model.MapToBlacklistDto());

        if (!isUpdated)
        {
            logger.LogError("Failed to update blacklist with id: {Id}", id);
            return  BadRequest(ErrorMessages.FailedToUpdate("blacklist", id));
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