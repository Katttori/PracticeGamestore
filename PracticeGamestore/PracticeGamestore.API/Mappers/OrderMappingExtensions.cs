using PracticeGamestore.Business.DataTransferObjects.Order;
using PracticeGamestore.Models.Order;

namespace PracticeGamestore.Mappers;

public static class OrderMappingExtensions
{
    public static OrderRequestDto MapToOrderDto(this OrderCreateRequestModel model)
    {
        return new(
            model.UserEmail,
            model.Total,
            model.GameIds
            );
    }
    
    public static OrderRequestDto MapToOrderDto(this OrderUpdateRequestModel model)
    {
        return new(
            model.UserEmail,
            model.Total,
            model.GameIds,
            model.Status
        );
    }

    public static OrderResponseModel MapToOrderModel(this OrderResponseDto dto)
    {
        return new()
        {
            Id = dto.Id!.Value,
            Status = dto.Status.ToString(),
            UserEmail = dto.UserEmail,
            Total = dto.Total,
            Games = dto.Games.Select(g => g.MapToGameModel()).ToList()
        };
    }
}