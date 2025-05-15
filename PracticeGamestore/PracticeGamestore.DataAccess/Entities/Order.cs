namespace PracticeGamestore.DataAccess.Entities;

using PracticeGamestore.DataAccess.Enums;

public class Order
{
    public Guid Id { get; set; }
    public OrderStatus Status { get; set; }
    public string UserEmail { get; set; }
    public decimal Total { get; set; }
}