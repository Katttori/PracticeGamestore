using PracticeGamestore.Business.Constants;
using PracticeGamestore.Business.DataTransferObjects.Order;
using PracticeGamestore.Business.DataTransferObjects.Payment;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.Business.Services.Payment;
using PracticeGamestore.DataAccess.Entities;
using PracticeGamestore.DataAccess.Enums;
using PracticeGamestore.DataAccess.Repositories.Game;
using PracticeGamestore.DataAccess.Repositories.Order;
using PracticeGamestore.DataAccess.UnitOfWork;

namespace PracticeGamestore.Business.Services.Order;

public class OrderService(
    IPaymentService paymentService,
    IOrderRepository orderRepository,
    IGameRepository gameRepository,
    IUnitOfWork unitOfWork) : IOrderService
{
    public async Task<IEnumerable<OrderResponseDto>> GetAllAsync()
    {
        var entities = await orderRepository.GetAllAsync();
        return entities.Select(e => e.MapToOrderDto());
    }

    public async Task<OrderResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await orderRepository.GetByIdAsync(id);
        return entity?.MapToOrderDto();
    }

    public async Task<Guid?> CreateAsync(OrderRequestDto dto)
    {
        if (!await AreAllGameIdsValid(dto.GameIds)) return null;
        
        var order = dto.MapToOrderEntity();
        order.GameOrders = dto.GameIds.Select(gameId => new GameOrder
        {
            GameId = gameId,
            Order = order
        }).ToList();
        
        var createdId = await orderRepository.CreateAsync(order);
        var changes = await unitOfWork.SaveChangesAsync();
        return changes > 0 ? createdId : null;
    }

    public async Task<bool> UpdateAsync(Guid id, OrderRequestDto dto)
    {
        if (!await AreAllGameIdsValid(dto.GameIds)) return false;
        
        var order = await orderRepository.GetByIdAsync(id);
        if (order is null) return false;

        order.Status = dto.Status;
        order.UserEmail = dto.UserEmail;
        order.Total = dto.Total;
        order.GameOrders = dto.GameIds.Select(gameId => new GameOrder
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
    
    public async Task<Dictionary<string, string>?> PayOrderAsync(Guid orderId, PaymentDto payment)
    {
        var order = await orderRepository.GetByIdAsync(orderId);
        if (order is null)
            throw new KeyNotFoundException(ErrorMessages.NotFound("Order", orderId));

        if (order.Status != OrderStatus.Initiated)
            throw new ArgumentException(ErrorMessages.IncorrectOrderStatusForPayment);

        bool isSuccessful;
        if (payment.Iban is not null)
            isSuccessful = await paymentService.PayIbanAsync(payment.Iban);
        else if (payment.CreditCard is not null)
            isSuccessful = await paymentService.PayCardAsync(payment.CreditCard);
        else if (payment.Ibox is not null)
            isSuccessful = await paymentService.PayIboxAsync(payment.Ibox);
        else
            throw new ArgumentException(ErrorMessages.InvalidPaymentType);
        
        if (!isSuccessful) return null;

        order.Status = OrderStatus.Paid;
        orderRepository.Update(order);
        var changes = await unitOfWork.SaveChangesAsync();
        if (changes <= 0) return null;
        return await orderRepository.GetGameKeysByOrderIdAsync(orderId);
    }

    public async Task<IEnumerable<OrderResponseDto>> GetOrdersByUserEmailAsync(string userEmail)
    {
        var orders = await orderRepository.GetOrdersByUserEmailAsync(userEmail);
        return orders.Select(o => o.MapToOrderDto());
    }
    
    private async Task<bool> AreAllGameIdsValid(List<Guid> gameIds)
    {
        var existingIds = await gameRepository.GetExistingIdsAsync(gameIds);
        return existingIds.Count == gameIds.Count;
    }
}