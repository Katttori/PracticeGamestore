using Microsoft.AspNetCore.Mvc;
using PracticeGamestore.Mappers;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Filtering;
using PracticeGamestore.Business.Services.Game;
using PracticeGamestore.Models.Game;

namespace PracticeGamestore.Controllers;

[ApiController, Route("games")]
public class GameController(IGameService gameService) : ControllerBase
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
        if (!ModelState.IsValid || filter.IsInvalid()) return BadRequest("Invalid query parameters!");
        var games = await gameService.GetFilteredAsync(filter);
        return Ok(games.Select(g => g.MapToGameModel()));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var gameDto = await gameService.GetByIdAsync(id);
        return gameDto is null
            ? NotFound($"Game with id {id} was not found.")
            : Ok(gameDto.MapToGameModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] GameRequestModel model)
    {
        if (GameRequestDto.HasIncorrectAgeRatingEnum(model.AgeRating))
            return BadRequest("Cannot convert provided age to the enum.");
        var id = await gameService.CreateAsync(model.MapToGameDto());
        return id is null
            ? BadRequest("Failed to create game.")
            : CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] GameRequestModel model)
    {
        if (GameRequestDto.HasIncorrectAgeRatingEnum(model.AgeRating))
            return BadRequest("Cannot convert provided age to the enum.");
        var updated = await gameService.UpdateAsync(id, model.MapToGameDto());
        return updated
            ? NoContent()
            : BadRequest($"Failed to update the game.");
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await gameService.DeleteAsync(id);
        return NoContent();
    }
}