namespace PracticeGamestore.DataAccess.Entities;

public class Order
{
    public Guid Id { get; set; }
    public OrderStatus Status { get; set; }
    public string UserEmail { get; set; }
    public decimal Total { get; set; }
}

public enum OrderStatus
{
    Pending,
    Paid,
    Completed,
    Cancelled
}