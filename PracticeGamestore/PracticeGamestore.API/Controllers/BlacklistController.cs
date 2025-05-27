using Microsoft.AspNetCore.Mvc;
using PracticeGamestore.Business.Services.Blacklist;
using PracticeGamestore.Mappers;
using PracticeGamestore.Models.Blacklist;

namespace PracticeGamestore.Controllers;

[ApiController, Route("blacklists")]
public class BlacklistController(IBlacklistService blacklistService) : ControllerBase
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
        return blacklist is null
            ? NotFound($"Blacklist with id: {id} was not found.") 
            : Ok(blacklist.MapToBlacklistModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BlacklistRequestModel model)
    {
        var createdId = await blacklistService.CreateAsync(model.MapToBlacklistDto());
        return createdId is null 
            ? BadRequest("Failed to create blacklist.") 
            : CreatedAtAction(nameof(GetById), new { id = createdId }, createdId);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] BlacklistRequestModel model)
    {
        var isUpdated = await blacklistService.UpdateAsync(id, model.MapToBlacklistDto());
        return isUpdated 
            ? NoContent() 
            : BadRequest($"Update failed. Blacklist with id: {id} might not exist.");
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await blacklistService.DeleteAsync(id);
        return NoContent();
    }
}