using PracticeGamestore.Business.Enums;

namespace PracticeGamestore.Models.Order;

public class OrderUpdateRequestModel
{
    public required string UserEmail { get; set; }
    public OrderStatus Status { get; set; }
    public decimal Total { get; set; }
    public List<Guid> GameIds { get; set; } = [];
}