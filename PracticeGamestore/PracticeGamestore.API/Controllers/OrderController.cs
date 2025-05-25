using Microsoft.AspNetCore.Mvc;
using PracticeGamestore.Business.Services.Order;
using PracticeGamestore.Mappers;
using PracticeGamestore.Models.Order;

namespace PracticeGamestore.Controllers;

[ApiController, Route("orders")]
public class OrderController(IOrderService orderService) : ControllerBase
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
        return order is null
            ? NotFound($"Order with id: {id} was not found.") 
            : Ok(order.MapToOrderModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OrderRequestModel model)
    {
        var createdId = await orderService.CreateAsync(model.MapToOrderDto());
        return createdId is null 
            ? BadRequest("Failed to create order.") 
            : CreatedAtAction(nameof(GetById), new { id = createdId }, createdId);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] OrderRequestModel model)
    {
        var isUpdated = await orderService.UpdateAsync(model.MapToOrderDto());
        return isUpdated 
            ? NoContent() 
            : BadRequest($"Update failed. Order with id: {id} might not exist.");
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await orderService.DeleteAsync(id);
        return NoContent();
    }
}