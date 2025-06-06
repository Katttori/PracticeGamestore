using Microsoft.AspNetCore.Mvc;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Business.Services.Game;
using PracticeGamestore.Business.Services.Platform;
using PracticeGamestore.Filters;
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
            return NotFound(ErrorMessages.NotFound("Platform", id));
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
            return NotFound(ErrorMessages.NotFound("Platform", platformId));
        }
        
        return Ok(games.Select(g => g.MapToGameModel()));
    }
    
    [HttpPost]
    [ServiceFilter(typeof(RequestModelValidationFilter))]
    public async Task<IActionResult> CreatePlatform([FromBody] PlatformRequestModel platform)
    {
        var platformDto = platform.MapToPlatformDto();
        var id = await platformService.CreateAsync(platformDto);
        
        if (id is null)
        {
            logger.LogError("Failed to create platform with model: {Model}", platform);
            return BadRequest(ErrorMessages.FailedToCreate("platform"));
        }
        
        platformDto.Id = id.Value;
        logger.LogInformation("Created platform with id: {Id}", platformDto.Id);
        return CreatedAtAction(nameof(GetPlatformById), new { id }, id);
    }

    [HttpPut("{id:guid}")]
    [ServiceFilter(typeof(RequestModelValidationFilter))]
    public async Task<IActionResult> UpdatePlatform(Guid id, [FromBody] PlatformRequestModel platform)
    {
        var dto = platform.MapToPlatformDto();
        dto.Id = id;

        var isUpdated = await platformService.UpdateAsync(dto);

        if (isUpdated) return NoContent();
        
        logger.LogError("Platform with id: {Id} was not found for update.", id);
        return BadRequest(ErrorMessages.FailedToUpdate("platform", id));
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeletePlatform(Guid id)
    {
        await platformService.DeleteAsync(id);
        return NoContent();
    }
}