using Microsoft.AspNetCore.Mvc;
using PracticeGamestore.Mappers;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Filtering;
using PracticeGamestore.Business.Services.Game;
using PracticeGamestore.Models.Game;

namespace PracticeGamestore.Controllers;

[ApiController, Route("games")]
public class GameController(IGameService gameService, ILogger<GameController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var games = await gameService.GetAllAsync();
        return Ok(games.Select(g => g.MapToGameModel()));
    }

    [HttpGet("/filter")]
    public async Task<IActionResult> GetFiltered([FromQuery] GameFilter filter)
    {
        if (!ModelState.IsValid || filter.IsInvalid())
        {
            logger.LogError("Invalid query parameters: {Query}", filter);
            return BadRequest("Invalid query parameters!");
        }
        
        var (games, totalCount) = await gameService.GetFilteredAsync(filter);
        return Ok(new PaginatedGameListResponseModel {
            Games = games.Select(g => g.MapToGameModel()).ToList(),
            PageNumber = filter.Page ?? 1,
            PageSize = filter.PageSize ?? 10,
            Count = totalCount
        });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var gameDto = await gameService.GetByIdAsync(id);
        
        if (gameDto is null)
        {
            logger.LogWarning("Game with id: {Id} was not found.", id);
            return NotFound($"Game with id {id} was not found.");
        }
        
        return Ok(gameDto.MapToGameModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] GameRequestModel model)
    {
        if (GameRequestDto.HasIncorrectAgeRatingEnum(model.AgeRating))
        {
            logger.LogError("Invalid age rating provided: {AgeRating}", model.AgeRating);
            return BadRequest("Cannot convert provided age to the enum.");
        }
        var id = await gameService.CreateAsync(model.MapToGameDto());
        
        if (id is null)
        {
            logger.LogError("Failed to create game with model: {Model}", model);
            return BadRequest("Failed to create game.");
        }
        
        logger.LogInformation("Created game with id: {Id}", id);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] GameRequestModel model)
    {
        if (GameRequestDto.HasIncorrectAgeRatingEnum(model.AgeRating))
        {
            logger.LogError("Invalid age rating provided: {AgeRating}", model.AgeRating);
            return BadRequest("Cannot convert provided age to the enum.");
        }
        var updated = await gameService.UpdateAsync(id, model.MapToGameDto());
        
        if (!updated)
        {
            logger.LogWarning("Game with id: {Id} was not found for update.", id);
            return BadRequest($"Failed to update the game.");
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