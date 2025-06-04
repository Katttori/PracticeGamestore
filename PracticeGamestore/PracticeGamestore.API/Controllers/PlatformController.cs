using Microsoft.AspNetCore.Mvc;
using PracticeGamestore.Business.Services.Game;
using PracticeGamestore.Business.Services.Platform;
using PracticeGamestore.Mappers;
using PracticeGamestore.Models.Platform;

namespace PracticeGamestore.Controllers;

[ApiController, Route("platforms")]
public class PlatformController(
    IPlatformService platformService,
    IGameService gameService,
    ILogger<PlatformController> logger) : ControllerBase
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
        
        if (platform is null)
        {
            logger.LogError("Platform with id: {Id} was not found.", id);
            return NotFound($"Platform with id: {id} was not found.");
        }
        
        return Ok(platform.MapToPlatformModel());
    }
    
    [HttpGet("{platformId:guid}/games")]
    public async Task<IActionResult> GetGamesByPlatform(Guid platformId)
    {
        var games = await gameService.GetByPlatformAsync(platformId);
        
        if (games is null)
        {
            logger.LogError("No games found for platform with id: {PlatformId}", platformId);
            return NotFound($"Platform with id: {platformId} was not found.");
        }
        
        return Ok(games.Select(g => g.MapToGameModel()));
    }
    
    [HttpPost]
    public async Task<IActionResult> CreatePlatform([FromBody] PlatformRequestModel platform)
    {
        if (string.IsNullOrWhiteSpace(platform.Name))
        {
            logger.LogError("Platform name cannot be empty or whitespace.");
            return BadRequest("Platform name cannot be empty or whitespace.");
        }
        
        if (string.IsNullOrWhiteSpace(platform.Description))
        {
            logger.LogError("Platform description cannot be empty or whitespace.");
            return BadRequest("Platform description cannot be empty or whitespace.");
        }

        if (platform.Name.Length > 100)
        {
            logger.LogError("Platform name must be between 1 and 100 characters long.");
            return BadRequest("Platform name must be between 1 and 100 characters long.");
        }
        if (platform.Description.Length > 255)
        {
            logger.LogError("Platform description must be up to 255 characters long.");
            return BadRequest("Platform description must be up to 255 characters long.");
        }
        
        var platformDto = platform.MapToPlatformDto();
        var id = await platformService.CreateAsync(platformDto);
        
        if (id is null)
        {
            logger.LogError("Failed to create platform with model: {Model}", platform);
            return BadRequest("Failed to create platform");
        }
        
        platformDto.Id = id.Value;
        logger.LogInformation("Created platform with id: {Id}", platformDto.Id);
        return CreatedAtAction(nameof(GetPlatformById), new { id }, id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdatePlatform(Guid id, [FromBody] PlatformRequestModel platform)
    {
        if (string.IsNullOrWhiteSpace(platform.Name))
        {
            logger.LogError("Platform name cannot be empty or whitespace.");
            return BadRequest("Platform name cannot be empty or whitespace.");
        }
        
        if (string.IsNullOrWhiteSpace(platform.Description))
        {
            logger.LogError("Platform description cannot be empty or whitespace.");
            return BadRequest("Platform description cannot be empty or whitespace.");
        }
        
        if (platform.Name.Length > 100)
        {
            logger.LogError("Platform name must be between 1 and 100 characters long.");
            return BadRequest("Platform name must be between 1 and 100 characters long.");
        }
        
        if (platform.Description.Length > 255)
        {
            logger.LogError("Platform description must be up to 255 characters long.");
            return BadRequest("Platform description must be up to 255 characters long.");
        }
        
        var dto = platform.MapToPlatformDto();
        dto.Id = id;

        var isUpdated = await platformService.UpdateAsync(dto);
        
        if (!isUpdated)
        {
            logger.LogError("Platform with id: {Id} was not found for update.", id);
            return BadRequest($"Error while trying to update the platform");
        }

        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeletePlatform(Guid id)
    {
        await platformService.DeleteAsync(id);
        return NoContent();
    }
}