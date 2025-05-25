using PracticeGamestore.DataAccess.Enums;

namespace PracticeGamestore.Business.DataTransferObjects;

public class OrderDto
{
    public Guid Id { get; }
    public OrderStatus Status { get; set; }
    public string UserEmail { get; set; }
    public decimal Total { get; set; }
    public List<GameOrderDto> GameOrders { get; set; }

    public OrderDto(string userEmail, decimal total, List<Guid> gameIds)
    {
        Id = Guid.NewGuid();
        Status = OrderStatus.Initiated;
        UserEmail = userEmail;
        Total = total;
        GameOrders = gameIds.Select(gameId => new GameOrderDto
        {
            GameId = gameId,
            OrderId = Id
        }).ToList();
    }
    
    public OrderDto(Guid id, OrderStatus status, string userEmail, decimal total, List<GameOrderDto> gameOrders)
    {
        Id = id;
        Status = status;
        UserEmail = userEmail;
        Total = total;
        GameOrders = gameOrders;
    }
}