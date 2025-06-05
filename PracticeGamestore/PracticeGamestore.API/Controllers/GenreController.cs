using Microsoft.AspNetCore.Mvc;
using PracticeGamestore.Business.Services.Genre;
using PracticeGamestore.Mappers;
using PracticeGamestore.Models.Genre;

namespace PracticeGamestore.Controllers;

[ApiController, Route("genres")]
public class GenreController(IGenreService genreService, ILogger<GenreController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var genres = await genreService.GetAllAsync();
        return Ok(genres.Select(g => g.MapToGenreModel()));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var genre = await genreService.GetByIdAsync(id);
        
        if (genre is null)
        {
            logger.LogError("Genre with id: {Id} was not found.", id);
            return NotFound($"Genre with id: {id} was not found.");
        }
        
        return Ok(genre.MapToGenreModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] GenreRequestModel model)
    {
        var createdId = await genreService.CreateAsync(model.MapToGenreDto());
        
        if (createdId is null)
        {
            logger.LogError("Failed to create genre with model: {Model}", model);
            return BadRequest("Failed to create genre.");
        }
        
        logger.LogInformation("Created genre with id: {Id}", createdId);
        
        return CreatedAtAction(nameof(GetById), new { id = createdId }, createdId);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] GenreRequestModel model)
    {
        var isUpdated = await genreService.UpdateAsync(id, model.MapToGenreDto());
        
        if (!isUpdated)
        {
            logger.LogError("Genre with id: {Id} was not found for update.", id);
            return BadRequest($"Update failed. Genre with id: {id} might not exist.");
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await genreService.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("{id:guid}/games")]
    public async Task<IActionResult> GetGamesByGenre([FromRoute] Guid id)
    {
        var games = await genreService.GetGames(id);
        return games is null
            ? NotFound($"Genre with id {id} was not found.")
            : Ok(games.Select(g => g.MapToGameModel()));
    }
}