using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.DataAccess.Entities;

namespace PracticeGamestore.Business.Mappers;

public static class GameOrderMappingExtensions
{
    public static GameOrder MapToGameOrderEntity(this GameOrderDto dto)
    {
        return new()
        {
            GameId = dto.GameId,
            OrderId = dto.OrderId,
        };
    }

    public static GameOrderDto MapToGameOrderDto(this GameOrder entity)
    {
        return new()
        {
            GameId = entity.GameId,
            OrderId = entity.OrderId,
        };
    }
}