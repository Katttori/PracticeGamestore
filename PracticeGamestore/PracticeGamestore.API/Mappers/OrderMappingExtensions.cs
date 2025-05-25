using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Models.Order;

namespace PracticeGamestore.Mappers;

public static class OrderMappingExtensions
{
    public static OrderDto MapToOrderDto(this OrderRequestModel model)
    {
        return new(
            model.UserEmail,
            model.Total,
            model.GameIds
            );
    }

    public static OrderResponseModel MapToOrderModel(this OrderDto dto)
    {
        return new()
        {
            Id = dto.Id,
            Status = dto.Status.ToString(),
            UserEmail = dto.UserEmail,
            Total = dto.Total,
            GameOrders = dto.GameOrders.Select(go => go.MapToGameOrderModel()).ToList()
        };
    }
}