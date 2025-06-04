using PracticeGamestore.DataAccess.Enums;

namespace PracticeGamestore.Business.DataTransferObjects.Order;

public class OrderResponseDto
{
    public Guid? Id { get; }
    public OrderStatus Status { get; set; }
    public string UserEmail { get; set; }
    public decimal Total { get; set; }
    public List<GameResponseDto> Games { get; set; }
    
    public OrderResponseDto(Guid id, OrderStatus status, string userEmail, decimal total, List<GameResponseDto> games)
    {
        Id = id;
        Status = status;
        UserEmail = userEmail;
        Total = total;
        Games = games;
    }
}