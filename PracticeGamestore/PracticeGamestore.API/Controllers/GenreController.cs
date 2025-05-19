using Microsoft.AspNetCore.Mvc;
using PracticeGamestore.Business.Services.Genre;
using PracticeGamestore.DTOs.Genre;
using PracticeGamestore.Mappers;

namespace PracticeGamestore.Controllers;

[ApiController, Route("genres")]
public class GenreController(IGenreService genreService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var genres = await genreService.GetAllAsync();
        return Ok(genres.Select(g => g.ToDto()));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var genre = await genreService.GetByIdAsync(id);
        return genre is null ? NotFound() : Ok(genre.ToDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGenreDto dto)
    {
        var createdId = await genreService.CreateAsync(dto.ToModel());
        return createdId is null ? BadRequest() : CreatedAtAction(nameof(GetById), new { id = createdId }, createdId);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateGenreDto dto)
    {
        var genre = await genreService.GetByIdAsync(id);
        if (genre is null) return NotFound();
        
        genre.ApplyUpdate(dto);
        var isUpdated = await genreService.UpdateAsync(genre);
        return isUpdated ? NoContent() : BadRequest();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await genreService.DeleteAsync(id);
        return NoContent();
    }
}