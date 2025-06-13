using PracticeGamestore.Business.DataTransferObjects.Order;
using PracticeGamestore.DataAccess.Entities;

namespace PracticeGamestore.Business.Mappers;

public static class OrderMappingExtensions
{
    public static Order MapToOrderEntity(this OrderRequestDto dto)
    {
        return new()
        {
            Status = (DataAccess.Enums.OrderStatus)dto.Status,
            UserEmail = dto.UserEmail,
            Total = dto.Total,
        };
    }

    public static OrderResponseDto MapToOrderDto(this Order entity)
    {
        var games = entity.GameOrders.Select(go => go.Game.MapToGameDto()).ToList();            
        
        return new(
            entity.Id, 
            entity.Status, 
            entity.UserEmail, 
            entity.Total, 
            games
        );
    }
}