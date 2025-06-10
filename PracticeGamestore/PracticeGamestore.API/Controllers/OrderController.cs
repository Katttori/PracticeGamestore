using Microsoft.AspNetCore.Mvc;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Business.Services.Location;
using PracticeGamestore.Business.Services.Order;
using PracticeGamestore.Filters;
using PracticeGamestore.Mappers;
using PracticeGamestore.Models.Order;

namespace PracticeGamestore.Controllers;

[ApiController, Route("orders")]
public class OrderController(
    IOrderService orderService,
    ILocationService locationService,
    IHttpContextAccessor httpContextAccessor,
    ILogger<OrderController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var orders = await orderService.GetAllAsync();
        return Ok(orders.Select(o => o.MapToOrderModel()));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var order = await orderService.GetByIdAsync(id);
        
        if (order is null)
        {
            logger.LogError("Order with id: {Id} was not found.", id);
            return NotFound(ErrorMessages.NotFound("Order", id));
        }
        
        return Ok(order.MapToOrderModel());
    }

    [HttpPost]
    [ServiceFilter(typeof(RequestModelValidationFilter))]
    public async Task<IActionResult> Create([FromBody] OrderRequestModel model)
    {
        await locationService.HandleLocationAccessAsync(httpContextAccessor.HttpContext!);
        
        var createdId = await orderService.CreateAsync(model.MapToOrderDto());
        
        if (createdId is null)
        {
            logger.LogError("Failed to create order with model: {Model}", model);
            return BadRequest(ErrorMessages.FailedToCreate("order"));
        }
        
        logger.LogInformation("Created order with id: {Id}", createdId);
        return CreatedAtAction(nameof(GetById), new { id = createdId }, createdId);
    }

    [HttpPut("{id:guid}")]
    [ServiceFilter(typeof(RequestModelValidationFilter))]
    public async Task<IActionResult> Update(Guid id, [FromBody] OrderRequestModel model)
    {
        var isUpdated = await orderService.UpdateAsync(id, model.MapToOrderDto());

        if (!isUpdated)
        {
            logger.LogError("Order with id: {Id} was not found for update.", id);
            return BadRequest(ErrorMessages.FailedToUpdate("order", id));
        }
        
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await orderService.DeleteAsync(id);
        return NoContent();
    }
}