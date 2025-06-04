using Microsoft.AspNetCore.Mvc;
using PracticeGamestore.Business.Services.Platform;
using PracticeGamestore.Mappers;
using PracticeGamestore.Models.Platform;

namespace PracticeGamestore.Controllers;

[ApiController, Route("platforms")]
public class PlatformController(IPlatformService platformService) : ControllerBase
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
        return platform is null
            ? NotFound($"Platform with id: {id} was not found.")
            : Ok(platform.MapToPlatformModel());
    }

    [HttpPost]
    public async Task<IActionResult> CreatePlatform([FromBody] PlatformRequestModel platform)
    {
        if (string.IsNullOrWhiteSpace(platform.Name))
        {
            return BadRequest("Platform name cannot be empty or whitespace.");
        }
        
        if (string.IsNullOrWhiteSpace(platform.Description))
        {
            return BadRequest("Platform description cannot be empty or whitespace.");
        }

        if (platform.Name.Length > 100)
            return BadRequest("Platform name must be between 1 and 100 characters long.");
        if (platform.Description.Length > 255)
            return BadRequest("Platform description must be up to 255 characters long.");
        
        var platformDto = platform.MapToPlatformDto();
        var id = await platformService.CreateAsync(platformDto);
        if (id is null) return BadRequest("Failed to create platform");
        platformDto.Id = id.Value;
        return CreatedAtAction(nameof(GetPlatformById), new { id }, id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdatePlatform(Guid id, [FromBody] PlatformRequestModel platform)
    {
        if (string.IsNullOrWhiteSpace(platform.Name))
        {
            return BadRequest("Platform name cannot be empty or whitespace.");
        }
        
        if (string.IsNullOrWhiteSpace(platform.Description))
        {
            return BadRequest("Platform description cannot be empty or whitespace.");
        }
        
        if (platform.Name.Length > 100)
            return BadRequest("Platform name must be between 1 and 100 characters long.");
        if (platform.Description.Length > 255)
            return BadRequest("Platform description must be up to 255 characters long.");
        
        var dto = platform.MapToPlatformDto();
        dto.Id = id;

        var isUpdated = await platformService.UpdateAsync(dto);
        return isUpdated
            ? NoContent()
            : BadRequest($"Error while trying to update the platform");
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeletePlatform(Guid id)
    {
        await platformService.DeleteAsync(id);
        return NoContent();
    }
}