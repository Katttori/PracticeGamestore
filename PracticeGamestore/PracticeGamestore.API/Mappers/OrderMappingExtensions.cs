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
            Id = dto.Id!.Value,
            Status = dto.Status.ToString(),
            UserEmail = dto.UserEmail,
            Total = dto.Total,
            Games = dto.Games!.Select(g => g.MapToGameModel()).ToList()
        };
    }
}