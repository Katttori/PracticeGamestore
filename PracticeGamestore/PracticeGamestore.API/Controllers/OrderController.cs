using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Business.Services.HeaderHandle;
using Microsoft.AspNetCore.Authorization;
using PracticeGamestore.Business.Services.Order;
using PracticeGamestore.Filters;
using PracticeGamestore.Mappers;
using PracticeGamestore.Models.Order;
using PracticeGamestore.Models.Payment;
using UserRole = PracticeGamestore.Business.Enums.UserRole;

namespace PracticeGamestore.Controllers;

[ApiController, Route("orders")]
public class OrderController(
    IOrderService orderService,
    IHeaderHandleService headerHandleService,
    ILogger<OrderController> logger) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = nameof(UserRole.Manager))]
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
    [Authorize(Roles = nameof(UserRole.User))]
    public async Task<IActionResult> Create(
        [FromHeader(Name = HeaderNames.LocationCountry), Required] string country,
        [FromHeader(Name = HeaderNames.UserEmail), Required] string email,
        [FromBody] OrderRequestModel model)
    {
        await headerHandleService.CheckAccessAsync(country, email);
        
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
    [Authorize(Roles = nameof(UserRole.Admin))]
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
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<IActionResult> Delete(Guid id)
    {
        await orderService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("{id:guid}/pay")]
    [Authorize(Roles = nameof(UserRole.User))]
    public async Task<IActionResult> PayOrder(Guid id, [FromBody] PaymentRequestModel model)
    {
        logger.LogInformation("Attempting to process payment for order with id: {Id}", id);

        var isSuccessful = await orderService.PayOrderAsync(id, model.MapToPaymentDto());
        if (!isSuccessful)
        {
            logger.LogError("Payment failed for order with id: {Id}", id);
            return BadRequest(ErrorMessages.SomethingWentWrong);
        }
        
        logger.LogInformation("Payment successful for order with id: {Id}", id);
        return Ok();
    }
}