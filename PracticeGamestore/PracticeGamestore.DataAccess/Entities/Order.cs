using PracticeGamestore.DataAccess.Enums;

namespace PracticeGamestore.DataAccess.Entities;

public class Order
{
    public Guid Id { get; set; }
    public OrderStatus Status { get; set; }
    public required string UserEmail { get; set; }
    public decimal Total { get; set; }
    public List<GameOrder> GameOrders { get; set; } = [];
}