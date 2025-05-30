using Microsoft.AspNetCore.Mvc;
using PracticeGamestore.Business.Services.Genre;
using PracticeGamestore.Mappers;
using PracticeGamestore.Models.Genre;

namespace PracticeGamestore.Controllers;

[ApiController, Route("genres")]
public class GenreController(IGenreService genreService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var genres = await genreService.GetAllAsync();
        return Ok(genres.Select(g => g.MapToGenreModel()));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var genre = await genreService.GetByIdAsync(id);
        return genre is null
            ? NotFound($"Genre with id: {id} was not found.") 
            : Ok(genre.MapToGenreModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] GenreRequestModel model)
    {
        var createdId = await genreService.CreateAsync(model.MapToGenreDto());
        return createdId is null 
            ? BadRequest("Failed to create genre.") 
            : CreatedAtAction(nameof(GetById), new { id = createdId }, createdId);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] GenreRequestModel model)
    {
        var isUpdated = await genreService.UpdateAsync(id, model.MapToGenreDto());
        return isUpdated 
            ? NoContent() 
            : BadRequest($"Update failed. Genre with id: {id} might not exist.");
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await genreService.DeleteAsync(id);
        return NoContent();
    }
}