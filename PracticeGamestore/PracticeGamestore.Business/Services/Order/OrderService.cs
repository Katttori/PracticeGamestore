using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.DataAccess.Entities;
using PracticeGamestore.DataAccess.Repositories.Order;
using PracticeGamestore.DataAccess.UnitOfWork;

namespace PracticeGamestore.Business.Services.Order;

public class OrderService(IOrderRepository orderRepository, IUnitOfWork unitOfWork) : IOrderService
{
    public async Task<IEnumerable<OrderDto>> GetAllAsync()
    {
        var entities = await orderRepository.GetAllAsync();
        return entities.Select(e => e.MapToOrderDto());
    }

    public async Task<OrderDto?> GetByIdAsync(Guid id)
    {
        var entity = await orderRepository.GetByIdAsync(id);
        return entity?.MapToOrderDto();
    }

    public async Task<Guid?> CreateAsync(OrderDto dto)
    {
        var order = dto.MapToOrderEntity();
        order.GameOrders = dto.GameIds!.Select(gameId => new GameOrder
        {
            GameId = gameId,
            Order = order
        }).ToList();
        
        var createdId = await orderRepository.CreateAsync(order);
        var changes = await unitOfWork.SaveChangesAsync();
        return changes > 0 ? createdId : null;
    }

    public async Task<bool> UpdateAsync(OrderDto dto)
    {
        var order = await orderRepository.GetByIdAsync(dto.Id!.Value);
        if (order is null) return false;

        order.Status = dto.Status;
        order.UserEmail = dto.UserEmail;
        order.Total = dto.Total;
        order.GameOrders = dto.GameIds!.Select(gameId => new GameOrder
        {
            GameId = gameId,
            OrderId = order.Id,
        }).ToList();

        orderRepository.Update(order);
        var changes = await unitOfWork.SaveChangesAsync();
        return changes > 0;
    }

    public async Task DeleteAsync(Guid id)
    {
        await orderRepository.DeleteAsync(id);
        await unitOfWork.SaveChangesAsync();
    }
}