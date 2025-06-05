using Microsoft.AspNetCore.Mvc;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Business.Services.Genre;
using PracticeGamestore.Filters;
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
            return NotFound(ErrorMessages.NotFound("Genre", id));
        }
        
        return Ok(genre.MapToGenreModel());
    }

    [HttpPost]
    [ServiceFilter(typeof(RequestModelValidationFilter))]
    public async Task<IActionResult> Create([FromBody] GenreRequestModel model)
    {
        var createdId = await genreService.CreateAsync(model.MapToGenreDto());
        
        if (createdId is null)
        {
            logger.LogError("Failed to create genre with model: {Model}", model);
            return BadRequest(ErrorMessages.FailedToCreate("genre"));
        }
        
        logger.LogInformation("Created genre with id: {Id}", createdId);
        
        return CreatedAtAction(nameof(GetById), new { id = createdId }, createdId);
    }

    [HttpPut("{id:guid}")]
    [ServiceFilter(typeof(RequestModelValidationFilter))]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] GenreRequestModel model)
    {
        var isUpdated = await genreService.UpdateAsync(id, model.MapToGenreDto());

        if (isUpdated) return NoContent();
        
        logger.LogError("Genre with id: {Id} was not found for update.", id);
        return BadRequest(ErrorMessages.FailedToUpdate("genre", id));
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
            ? NotFound(ErrorMessages.NotFound("Genre", id))
            : Ok(games.Select(g => g.MapToGameModel()));
    }
}