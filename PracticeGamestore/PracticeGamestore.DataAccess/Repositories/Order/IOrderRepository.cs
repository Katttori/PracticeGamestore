namespace PracticeGamestore.DataAccess.Repositories.Order;

public interface IOrderRepository
{ 
    Task<IEnumerable<Entities.Order>> GetAllAsync();
    Task<Entities.Order?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(Entities.Order order);
    void Update(Entities.Order order);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<Entities.Order>> GetOrdersByUserEmailAsync(string userEmail);
}