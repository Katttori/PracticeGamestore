using PracticeGamestore.Business.DataTransferObjects.Order;
using PracticeGamestore.DataAccess.Entities;
using PracticeGamestore.DataAccess.Enums;
using PracticeGamestore.Models.Order;

namespace PracticeGamestore.Tests.TestData;

public class Order
{
    private static readonly Guid FirstId = Guid.NewGuid();
    private static readonly Guid SecondId = Guid.NewGuid();
    
    private static readonly List<DataAccess.Entities.Game> GameEntities = Game.GenerateGameEntities();
    
    public static List<DataAccess.Entities.Order> GenerateOrderEntities()
    {
        return
        [
            new()
            {
                Id = FirstId,
                Status = OrderStatus.Initiated,
                UserEmail = "test@test.com",
                Total = 100,
                GameOrders =
                [
                    new GameOrder { GameId = FirstId, OrderId = FirstId, Game = GameEntities[0] },
                    new GameOrder { GameId = SecondId, OrderId = FirstId, Game = GameEntities[1] },
                ]
            },


            new()
            {
                Id = SecondId,
                Status = OrderStatus.Paid,
                UserEmail = "test2@test.com",
                Total = 200,
                GameOrders =
                [
                    new GameOrder { GameId = SecondId, OrderId = SecondId, Game = GameEntities[1] },
                    new GameOrder { GameId = FirstId, OrderId = SecondId, Game = GameEntities[0] }
                ]
            }
        ];
    }

    public static List<OrderResponseDto> GenerateOrderResponseDtos()
    {
        return [
            new(
                FirstId,
                OrderStatus.Initiated,
                "test@test.com",
                100,
                Game.GenerateGameResponseDtos()
            ),

            new(
                SecondId,
                OrderStatus.Created,
                "test2@test.com",
                200,
                Game.GenerateGameResponseDtos()
            )
        ];
    }

    public static OrderRequestDto GenerateOrderRequestDto()
    {
        return new(
            "test@test.com",
            100,
            [FirstId, SecondId]
        );
    }

    public static OrderResponseDto GenerateOrderResponseDto()
    {
        return new(
            FirstId,
            OrderStatus.Initiated,
            "test@test.com",
            100,
            Game.GenerateGameResponseDtos()
        );
    }

    public static OrderCreateRequestModel GenerateOrderCreateRequestModel()
    {
        return new()
        {
            UserEmail = "test@test.com",
            Total = 100,
            GameIds = [FirstId, SecondId]
        };
    }

    public static Dictionary<string, string> GenerateGameKeyMapForOrder(DataAccess.Entities.Order order)
    {
        return order.GameOrders.Select(go => new 
        {
            GameName = go.Game.Name,
            GameKey = go.Game.Key
                
        }).ToDictionary(x => x.GameName, x => x.GameKey);
    }

    public static OrderUpdateRequestModel GenerateOrderUpdateRequestModel()
    {
        return new()
        {
            UserEmail = "test@test.com",
            Status = Business.Enums.OrderStatus.Initiated,
            Total = 100,
            GameIds = [FirstId, SecondId]
        };
    }
}