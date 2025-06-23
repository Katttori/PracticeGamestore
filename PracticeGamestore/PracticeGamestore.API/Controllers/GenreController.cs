using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Business.Enums;
using PracticeGamestore.Business.Services.Genre;
using PracticeGamestore.Extensions;
using PracticeGamestore.Business.Services.HeaderHandle;
using PracticeGamestore.Filters;
using PracticeGamestore.Mappers;
using PracticeGamestore.Models.Genre;

namespace PracticeGamestore.Controllers;

[ApiController, Route("genres")]
public class GenreController(
    IGenreService genreService,
    IHeaderHandleService headerHandleService,
    ILogger<GenreController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromHeader(Name = HeaderNames.LocationCountry), Required] string country,
        [FromHeader(Name = HeaderNames.UserEmail), Required] string email)
    {
        await headerHandleService.CheckAccessAsync(country, email);
        
        var genres = await genreService.GetAllAsync();
        return Ok(genres.Select(g => g.MapToGenreModel()));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        [FromHeader(Name = HeaderNames.LocationCountry), Required] string country,
        [FromHeader(Name = HeaderNames.UserEmail), Required] string email,
        [FromRoute] Guid id)
    {
        await headerHandleService.CheckAccessAsync(country, email);
        
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
    [Authorize(Roles = nameof(UserRole.Manager))]
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
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] GenreRequestModel model)
    {
        var isUpdated = await genreService.UpdateAsync(id, model.MapToGenreDto());

        if (!isUpdated)
        {
            logger.LogError("Genre with id: {Id} was not found for update.", id);
            return BadRequest(ErrorMessages.FailedToUpdate("genre", id));
        }
        
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<IActionResult> Delete(Guid id)
    {
        await genreService.DeleteAsync(id);
        return NoContent();
    }
    
    [BirthdateRestrictionFilter]
    [ServiceFilter(typeof(BirthdateRestrictionFromDbFilter))]
    [HttpGet("{id:guid}/games")]
    public async Task<IActionResult> GetGamesByGenre(
        [FromHeader(Name = HeaderNames.LocationCountry), Required] string country,
        [FromHeader(Name = HeaderNames.UserEmail), Required] string email,
        [FromRoute] Guid id)
    {
        await headerHandleService.CheckAccessAsync(country, email);
        
        var games = await genreService.GetGamesAsync(id, HttpContext.IsUnderage());
        return games is null
            ? NotFound(ErrorMessages.NotFound("Genre", id))
            : Ok(games.Select(g => g.MapToGameModel()));
    }
}