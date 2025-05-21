using Microsoft.AspNetCore.Mvc;
using PracticeGamestore.Business.Services.Publisher;
using PracticeGamestore.Mappers;
using PracticeGamestore.Models.Publisher;

namespace PracticeGamestore.Controllers;

[ApiController, Route("publishers")]
public class PublisherController(IPublisherService service): ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var publishers = await service.GetAllAsync();
        return Ok(publishers.Select(p => p.ToModel()));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var publisher = await service.GetByIdAsync(id);
        return publisher is null
            ? NotFound($"Publisher with id {id} was not found.")
            : Ok(publisher.ToModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PublisherRequestModel model)
    {
        var id = await service.CreateAsync(model.ToDto());
        return id is null
            ? BadRequest($"Failed to create publisher.")
            : CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] PublisherRequestModel model)
    {
        var updated = await service.UpdateAsync(model.ToDto(id));
        return updated ? NoContent() : BadRequest("Failed to update.");
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }
}