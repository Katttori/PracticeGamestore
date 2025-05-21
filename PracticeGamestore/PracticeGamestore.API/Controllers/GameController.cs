using Microsoft.AspNetCore.Mvc;
using PracticeGamestore.Business.Services;
using PracticeGamestore.Mappers;
using PracticeGamestore.Models;

namespace PracticeGamestore.Controllers;

[ApiController, Route("games")]
public class GameController(IGameService gameService): ControllerBase

{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
       var games =  await gameService.GetAllAsync();
       return Ok(games.Select(g => g.ToModel()));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var gameDto = await gameService.GetByIdAsync(id);
        return gameDto is null
            ? NotFound($"Game with id {id} was not found.")
            : Ok(gameDto.ToModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] GameRequestModel model)
    {
        var id = await gameService.CreateAsync(model.ToDto());
        return id is null
            ? BadRequest("Failed to create game.")
            : CreatedAtAction(nameof(GetById), new {id}, model);

    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] GameRequestModel model)
    {
        var updated = await gameService.UpdateAsync(model.ToDto(id));
        return updated
            ? NoContent()
            : NotFound($"Game with id {id} was not found");
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await gameService.DeleteAsync(id);
        return NoContent();
    }
}