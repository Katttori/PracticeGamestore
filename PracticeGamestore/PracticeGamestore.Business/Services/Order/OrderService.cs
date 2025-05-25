using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Mappers;
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
        var createdId = await orderRepository.CreateAsync(dto.MapToOrderEntity());
        var changes = await unitOfWork.SaveChangesAsync();
        return changes > 0 ? createdId : null;
    }

    public async Task<bool> UpdateAsync(OrderDto dto)
    {
        var entity = await orderRepository.GetByIdAsync(dto.Id);
        if (entity is null) return false;

        entity.Status = dto.Status;
        entity.UserEmail = dto.UserEmail;
        entity.Total = dto.Total;
        entity.GameOrders = dto.GameOrders.Select(go => go.MapToGameOrderEntity()).ToList();

        orderRepository.Update(entity);
        var changes = await unitOfWork.SaveChangesAsync();
        return changes > 0;
    }

    public async Task DeleteAsync(Guid id)
    {
        await orderRepository.DeleteAsync(id);
        await unitOfWork.SaveChangesAsync();
    }
}