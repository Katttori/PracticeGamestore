namespace PracticeGamestore.Models.Order;

public class OrderCreateRequestModel
{
    public required string UserEmail { get; set; }
    public decimal Total { get; set; }
    public List<Guid> GameIds { get; set; } = [];
}