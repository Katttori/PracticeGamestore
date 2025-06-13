using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Business.Services.HeaderHandle;
using PracticeGamestore.Business.Services.Publisher;
using PracticeGamestore.Filters;
using PracticeGamestore.Mappers;
using PracticeGamestore.Models.Publisher;

namespace PracticeGamestore.Controllers;

[ApiController, Route("publishers")]
public class PublisherController(
    IPublisherService publisherService,
    IHeaderHandleService headerHandleService,
    ILogger<PublisherController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromHeader(Name = HeaderNames.LocationCountry), Required] string country,
        [FromHeader(Name = HeaderNames.UserEmail), Required] string email)
    {
        await headerHandleService.CheckAccessAsync(country, email);
        
        var publishers = await publisherService.GetAllAsync();
        return Ok(publishers.Select(p => p.MapToPublisherModel()));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        [FromHeader(Name = HeaderNames.LocationCountry), Required] string country,
        [FromHeader(Name = HeaderNames.UserEmail), Required] string email,
        [FromRoute] Guid id)
    {
        await headerHandleService.CheckAccessAsync(country, email);
        
        var publisher = await publisherService.GetByIdAsync(id);
        
        if (publisher is null)
        {
            logger.LogError("Publisher with id: {Id} was not found.", id);
            return NotFound(ErrorMessages.NotFound("Publisher", id));
        }
        
        return Ok(publisher.MapToPublisherModel());
    }

    [HttpGet("{id:guid}/games")]
    public async Task<IActionResult> GetPublisherGames(
        [FromHeader(Name = HeaderNames.LocationCountry), Required] string country,
        [FromHeader(Name = HeaderNames.UserEmail), Required] string email,
        [FromRoute] Guid id)
    {
        await headerHandleService.CheckAccessAsync(country, email);
        
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
        var isUpdated = await publisherService.UpdateAsync(id, model.MapToPublisherDto());

        if (!isUpdated)
        {
            logger.LogWarning("Publisher with id: {Id} was not found for update.", id);
            return BadRequest(ErrorMessages.FailedToUpdate("publisher", id));
        }
        
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await publisherService.DeleteAsync(id);
        return NoContent();
    }
}