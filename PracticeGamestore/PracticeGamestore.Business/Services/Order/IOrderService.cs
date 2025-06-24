using PracticeGamestore.Business.DataTransferObjects.Order;
using PracticeGamestore.Business.DataTransferObjects.Payment;

namespace PracticeGamestore.Business.Services.Order;

public interface IOrderService
{
    Task<IEnumerable<OrderResponseDto>> GetAllAsync();
    Task<OrderResponseDto?> GetByIdAsync(Guid id);
    Task<Guid?> CreateAsync(OrderRequestDto dto);
    Task<bool> UpdateAsync(Guid id, OrderRequestDto dto);
    Task DeleteAsync(Guid id);
    Task<bool> PayOrderAsync(Guid orderId, PaymentDto payment);
}