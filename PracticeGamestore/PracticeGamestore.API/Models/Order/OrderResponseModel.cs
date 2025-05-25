using PracticeGamestore.Models.GameOrder;

namespace PracticeGamestore.Models.Order;

public class OrderResponseModel
{
    public Guid Id { get; set; }
    public required string Status { get; set; }
    public required string UserEmail { get; set; }
    public decimal Total { get; set; }
    public List<GameOrderModel> GameOrders { get; set; } = [];
}