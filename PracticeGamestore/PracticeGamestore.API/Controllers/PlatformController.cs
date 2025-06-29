using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Business.Enums;
using PracticeGamestore.Business.Services.HeaderHandle;
using PracticeGamestore.Business.Services.Platform;
using PracticeGamestore.Extensions;
using PracticeGamestore.Filters;
using PracticeGamestore.Mappers;
using PracticeGamestore.Models.Platform;

namespace PracticeGamestore.Controllers;

[ApiController, Route("platforms")]
public class PlatformController(
    IPlatformService platformService,
    IHeaderHandleService headerHandleService,
    ILogger<PlatformController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllPlatforms(
        [FromHeader(Name = HeaderNames.LocationCountry), Required] string country,
        [FromHeader(Name = HeaderNames.UserEmail), Required] string email)
    {
        await headerHandleService.CheckAccessAsync(country, email);
        
        var platforms = await platformService.GetAllAsync();
        return Ok(platforms.Select(p => p.MapToPlatformModel()));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPlatformById(
        [FromHeader(Name = HeaderNames.LocationCountry), Required] string country,
        [FromHeader(Name = HeaderNames.UserEmail), Required] string email,
        Guid id)
    {
        await headerHandleService.CheckAccessAsync(country, email);
        
        var platform = await platformService.GetByIdAsync(id);
        
        if (platform is null)
        {
            logger.LogError("Platform with id: {Id} was not found.", id);
            return NotFound(ErrorMessages.NotFound("Platform", id));
        }
        
        return Ok(platform.MapToPlatformModel());
    }
    
    [BirthdateRestrictionFilter]
    [ServiceFilter(typeof(BirthdateRestrictionFromDbFilter))]
    [HttpGet("{platformId:guid}/games")]
    public async Task<IActionResult> GetGamesByPlatform(
        [FromHeader(Name = HeaderNames.LocationCountry), Required] string country,
        [FromHeader(Name = HeaderNames.UserEmail), Required] string email,
        Guid platformId)
    {
        await headerHandleService.CheckAccessAsync(country, email);
        
        var games = await platformService.GetGamesAsync(platformId, HttpContext.IsUnderage());
        
        if (games is null)
        {
            logger.LogError("No games found for platform with id: {PlatformId}", platformId);
            return NotFound(ErrorMessages.NotFound("Platform", platformId));
        }
        
        return Ok(games.Select(g => g.MapToGameModel()));
    }
    
    [HttpPost]
    [ServiceFilter(typeof(RequestModelValidationFilter))]
    [Authorize(Roles = nameof(UserRole.Manager))]
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
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<IActionResult> UpdatePlatform(Guid id, [FromBody] PlatformRequestModel platform)
    {
        var dto = platform.MapToPlatformDto();
        dto.Id = id;

        var isUpdated = await platformService.UpdateAsync(dto);

        if (!isUpdated)
        {
            logger.LogError("Platform with id: {Id} was not found for update.", id);
            return BadRequest(ErrorMessages.FailedToUpdate("platform", id));
        }
        
        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<IActionResult> DeletePlatform(Guid id)
    {
        await platformService.DeleteAsync(id);
        return NoContent();
    }
}