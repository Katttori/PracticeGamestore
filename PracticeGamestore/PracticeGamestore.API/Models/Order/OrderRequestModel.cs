namespace PracticeGamestore.Models.Order;

public class OrderRequestModel
{
    public required string UserEmail { get; set; }
    public decimal Total { get; set; }
    public List<Guid> GameIds { get; set; } = [];
}