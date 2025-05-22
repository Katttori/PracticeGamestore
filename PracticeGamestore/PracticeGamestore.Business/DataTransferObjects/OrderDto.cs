using PracticeGamestore.DataAccess.Entities;
using PracticeGamestore.DataAccess.Enums;

namespace PracticeGamestore.Business.DataTransferObjects;

public class OrderDto
{
    public Guid Id { get; }
    public OrderStatus Status { get; set; }
    public string UserEmail { get; set; }
    public decimal Total { get; set; }
    public List<GameOrder> GameOrders { get; set; }

    public OrderDto(Guid? id, OrderStatus status, string userEmail, decimal total, List<GameOrder> gameOrders)
    {
        Id = id ?? Guid.NewGuid();
        Status = status;
        UserEmail = userEmail;
        Total = total;
        GameOrders = gameOrders;
    }
}