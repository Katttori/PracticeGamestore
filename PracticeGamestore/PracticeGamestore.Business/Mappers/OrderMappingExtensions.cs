using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.DataAccess.Entities;

namespace PracticeGamestore.Business.Mappers;

public static class OrderMappingExtensions
{
    public static Order MapToOrderEntity(this OrderDto dto)
    {
        return new()
        {
            Id = dto.Id,
            Status = dto.Status,
            UserEmail = dto.UserEmail,
            Total = dto.Total,
            GameOrders = dto.GameOrders
        };
    }

    public static OrderDto MapToOrderDto(this Order entity)
    {
        return new(entity.Id, entity.Status, entity.UserEmail, entity.Total, entity.GameOrders);
    }
}