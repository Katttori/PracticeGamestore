using PracticeGamestore.Business.DataTransferObjects;

namespace PracticeGamestore.Business.Services.Order;

public interface IOrderService
{
    Task<IEnumerable<OrderDto>> GetAllAsync();
    Task<OrderDto?> GetByIdAsync(Guid id);
    Task<Guid?> CreateAsync(OrderDto dto);
    Task<bool> UpdateAsync(Guid id, OrderDto dto);
    Task DeleteAsync(Guid id);
}