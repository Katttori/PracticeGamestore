using Microsoft.AspNetCore.Mvc;
using PracticeGamestore.Business.Services.Platform;
using PracticeGamestore.Mappers;
using PracticeGamestore.Models.Platform;

namespace PracticeGamestore.Controllers;

[ApiController, Route("platforms")]
public class PlatformController(IPlatformService platformService): ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllPlatforms()
    {
        var platforms = await platformService.GetAllAsync();
        return Ok(platforms.Select(p => p.MapToPlatformModel()));
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPlatformById(Guid id)
    {
        var platform = await platformService.GetByIdAsync(id);
        if (platform is null) return NotFound($"Platform with id {id} not found");
        return Ok(platform.MapToPlatformModel());
    }
    
    [HttpPost]
    public async Task<IActionResult> CreatePlatform([FromBody] PlatformRequestModel platform)
    {
        var platformDto = platform.MapToPlatformDto();
        var id = await platformService.CreateAsync(platformDto);
        if (id is null) return BadRequest("Failed to create platform");
        return CreatedAtAction(nameof(GetPlatformById), new { id },  platformDto.MapToPlatformModel());
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdatePlatform(Guid id, [FromBody] PlatformRequestModel platform)
    {
        var isUpdated = await platformService.UpdateAsync(platform.MapToPlatformDto());
        return isUpdated ? NoContent() : BadRequest($"Error while trying to update the platform");
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeletePlatform(Guid id)
    {
        await platformService.DeleteAsync(id);
        return NoContent();
    }
}