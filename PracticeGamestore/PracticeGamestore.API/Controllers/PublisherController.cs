using Microsoft.AspNetCore.Mvc;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Business.Services.Publisher;
using PracticeGamestore.Filters;
using PracticeGamestore.Mappers;
using PracticeGamestore.Models.Publisher;

namespace PracticeGamestore.Controllers;

[ApiController, Route("publishers")]
public class PublisherController(IPublisherService publisherService, ILogger<PublisherController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var publishers = await publisherService.GetAllAsync();
        return Ok(publishers.Select(p => p.MapToPublisherModel()));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var publisher = await publisherService.GetByIdAsync(id);
        
        if (publisher is null)
        {
            logger.LogError("Publisher with id: {Id} was not found.", id);
            return NotFound(ErrorMessages.NotFound("Publisher", id));
        }
        
        return Ok(publisher.MapToPublisherModel());
    }

    [HttpGet("{id:guid}/games")]
    public async Task<IActionResult> GetPublisherGames([FromRoute] Guid id)
    {
         var games = await publisherService.GetGamesAsync(id);

         if (games is null)
         {
             logger.LogError("No games found for publisher with id: {Id}", id);
             return NotFound(ErrorMessages.NotFound("Publisher", id));
         }
         
         return Ok(games);
    }

    [HttpPost]
    [ServiceFilter(typeof(RequestModelValidationFilter))]
    public async Task<IActionResult> Create([FromBody] PublisherRequestModel model)
    {
        var id = await publisherService.CreateAsync(model.MapToPublisherDto());
        
        if (id is null)
        {
            logger.LogError("Failed to create publisher with model: {Model}", model);
            return BadRequest(ErrorMessages.FailedToCreate("publisher"));
        }
        
        logger.LogInformation("Created publisher with id: {Id}", id);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:guid}")]
    [ServiceFilter(typeof(RequestModelValidationFilter))]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] PublisherRequestModel model)
    {
        var updated = await publisherService.UpdateAsync(id, model.MapToPublisherDto());

        if (updated) return NoContent();
        
        logger.LogWarning("Publisher with id: {Id} was not found for update.", id);
        return BadRequest(ErrorMessages.FailedToUpdate("publisher", id));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await publisherService.DeleteAsync(id);
        return NoContent();
    }
}