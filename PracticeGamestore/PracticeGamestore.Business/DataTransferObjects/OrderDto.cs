using PracticeGamestore.DataAccess.Enums;

namespace PracticeGamestore.Business.DataTransferObjects;

public class OrderDto
{
    public Guid? Id { get; }
    public OrderStatus Status { get; set; }
    public string UserEmail { get; set; }
    public decimal Total { get; set; }
    public List<Guid>? GameIds { get; set; }
    public List<GameDto>? Games { get; set; }

    public OrderDto(string userEmail, decimal total, List<Guid> gameIds)
    {
        Status = OrderStatus.Initiated;
        UserEmail = userEmail;
        Total = total;
        GameIds = gameIds;
    }
    
    public OrderDto(Guid id, OrderStatus status, string userEmail, decimal total, List<GameDto> games)
    {
        Id = id;
        Status = status;
        UserEmail = userEmail;
        Total = total;
        Games = games;
    }
}