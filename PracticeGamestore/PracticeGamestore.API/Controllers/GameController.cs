using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Mappers;
using PracticeGamestore.Business.DataTransferObjects.Filtering;
using PracticeGamestore.Business.Services.Game;
using PracticeGamestore.Business.Services.HeaderHandle;
using PracticeGamestore.Filters;
using PracticeGamestore.Models.Game;

namespace PracticeGamestore.Controllers;

[ApiController, Route("games")]
public class GameController(
    IGameService gameService,
    IHeaderHandleService headerHandleService,
    ILogger<GameController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromHeader(Name = HeaderNames.LocationCountry), Required] string country,
        [FromHeader(Name = HeaderNames.UserEmail), Required] string email)
    {
        await headerHandleService.CheckAccessAsync(country, email);
        
        var games = await gameService.GetAllAsync();
        return Ok(games.Select(g => g.MapToGameModel()));
    }

    [HttpGet("/filter")]
    public async Task<IActionResult> GetFiltered(
        [FromHeader(Name = HeaderNames.LocationCountry), Required] string country,
        [FromHeader(Name = HeaderNames.UserEmail), Required] string email,
        [FromQuery] GameFilter filter)
    {
        await headerHandleService.CheckAccessAsync(country, email);
        
        var (games, totalCount) = await gameService.GetFilteredAsync(filter);
        return Ok(new PaginatedGameListResponseModel {
            Games = games.Select(g => g.MapToGameModel()).ToList(),
            PageNumber = filter.Page ?? 1,
            PageSize = filter.PageSize ?? 10,
            Count = totalCount
        });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        [FromHeader(Name = HeaderNames.LocationCountry), Required] string country,
        [FromHeader(Name = HeaderNames.UserEmail), Required] string email,
        [FromRoute] Guid id)
    {
        await headerHandleService.CheckAccessAsync(country, email);
        
        var gameDto = await gameService.GetByIdAsync(id);
        
        if (gameDto is null)
        {
            logger.LogWarning("Game with id: {Id} was not found.", id);
            return NotFound(ErrorMessages.NotFound("Game", id));
        }
        
        return Ok(gameDto.MapToGameModel());
    }

    [HttpPost]
    [ServiceFilter(typeof(RequestModelValidationFilter))]
    public async Task<IActionResult> Create([FromBody] GameRequestModel model)
    {
        var id = await gameService.CreateAsync(model.MapToGameDto());
        
        if (id is null)
        {
            logger.LogError("Failed to create game with model: {Model}", model);
            return BadRequest(ErrorMessages.FailedToCreate("game"));
        }
        
        logger.LogInformation("Created game with id: {Id}", id);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:guid}")]
    [ServiceFilter(typeof(RequestModelValidationFilter))]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] GameRequestModel model)
    {
        var isUpdated = await gameService.UpdateAsync(id, model.MapToGameDto());

        if (!isUpdated)
        {
            logger.LogWarning("Game with id: {Id} was not found for update.", id);
            return BadRequest(ErrorMessages.FailedToUpdate("game", id));
        }
        
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await gameService.DeleteAsync(id);
        return NoContent();
    }
}