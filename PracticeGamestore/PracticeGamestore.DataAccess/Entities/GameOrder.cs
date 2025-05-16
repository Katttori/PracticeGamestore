namespace PracticeGamestore.DataAccess.Entities;

public class GameOrder
{
    public Guid GameId { get; set; }
    public Guid OrderId { get; set; }
    public Game Game { get; set; } = null!;
    public Order Order { get; set; } = null!;
}