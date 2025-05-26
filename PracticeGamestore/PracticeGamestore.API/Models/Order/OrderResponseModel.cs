namespace PracticeGamestore.Models.Order;

public class OrderResponseModel
{
    public Guid Id { get; set; }
    public required string Status { get; set; }
    public required string UserEmail { get; set; }
    public decimal Total { get; set; }
    public List<GameResponseModel> Games { get; set; } = [];
}