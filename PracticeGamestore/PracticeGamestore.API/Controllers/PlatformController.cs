using Microsoft.AspNetCore.Mvc;
using PracticeGamestore.Business.Services.Platform;
using PracticeGamestore.Mappers;
using PracticeGamestore.Models.Platform;

namespace PracticeGamestore.Controllers;

[ApiController, Route("platforms")]
public class PlatformController: ControllerBase
{
    private readonly IPlatformService _platformService;
    
    public PlatformController(IPlatformService platformService)
    {
        _platformService = platformService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllPlatforms()
    {
        var platforms = await _platformService.GetAllAsync();
        return Ok(platforms.Select(p => p.MapToPlatformModel()));
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPlatformById(Guid id)
    {
        var platform = await _platformService.GetByIdAsync(id);
        if (platform is null) return NotFound($"Platform with id {id} not found");
        return Ok(platform.MapToPlatformModel());
    }
    
    [HttpPost]
    public async Task<IActionResult> CreatePlatform([FromBody] PlatformRequestModel platform)
    {
        var platformDto = platform.MapToPlatformDto();
        var id = await _platformService.CreateAsync(platformDto);
        if (id is null) return BadRequest("Failed to create platform");
        return CreatedAtAction(nameof(GetPlatformById), new { id }, platform);
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdatePlatform(Guid id, [FromBody] PlatformRequestModel platform)
    {
        var platformDto = platform.MapToPlatformDto();
        platformDto.Id = id;
        var updated = await _platformService.UpdateAsync(platformDto);
        if (!updated) return NotFound($"Platform with id {id} not found");
        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeletePlatform(Guid id)
    {
        await _platformService.DeleteAsync(id);
        return NoContent();
    }
}