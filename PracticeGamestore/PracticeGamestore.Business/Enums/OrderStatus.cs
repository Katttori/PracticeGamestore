namespace PracticeGamestore.Business.Enums;

public enum OrderStatus
{
    Created = DataAccess.Enums.OrderStatus.Created,
    Initiated = DataAccess.Enums.OrderStatus.Initiated,
    Paid = DataAccess.Enums.OrderStatus.Paid,
    Cancelled = DataAccess.Enums.OrderStatus.Cancelled,
}