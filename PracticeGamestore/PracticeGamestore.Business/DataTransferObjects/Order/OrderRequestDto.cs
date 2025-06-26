using PracticeGamestore.DataAccess.Enums;

namespace PracticeGamestore.Business.DataTransferObjects.Order;

public class OrderRequestDto
{
    public OrderStatus Status { get; set; }
    public string UserEmail { get; set; }
    public decimal Total { get; set; }
    public List<Guid> GameIds { get; set; }
    
    public OrderRequestDto(string userEmail, decimal total, List<Guid> gameIds, Enums.OrderStatus? status = null)
    {
        Status = status is null ? OrderStatus.Created : (OrderStatus)status;
        UserEmail = userEmail;
        Total = total;
        GameIds = gameIds;
    }
}