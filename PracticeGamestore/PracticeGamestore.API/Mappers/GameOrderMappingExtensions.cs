using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Models.GameOrder;

namespace PracticeGamestore.Mappers;

public static class GameOrderMappingExtensions
{
    public static GameOrderDto MapToGameOrderDto(this GameOrderModel model)
    {
        return new()
        {
            GameId = model.GameId,
            OrderId = model.OrderId,
        };
    }

    public static GameOrderModel MapToGameOrderModel(this GameOrderDto dto)
    {
        return new()
        {
            GameId = dto.GameId,
            OrderId = dto.OrderId,
        };
    }
}