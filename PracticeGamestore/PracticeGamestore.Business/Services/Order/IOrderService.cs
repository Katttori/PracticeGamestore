using PracticeGamestore.Business.DataTransferObjects.Order;

namespace PracticeGamestore.Business.Services.Order;

public interface IOrderService
{
    Task<IEnumerable<OrderResponseDto>> GetAllAsync();
    Task<OrderResponseDto?> GetByIdAsync(Guid id);
    Task<Guid?> CreateAsync(OrderRequestDto dto);
    Task<bool> UpdateAsync(Guid id, OrderRequestDto dto);
    Task DeleteAsync(Guid id);
}