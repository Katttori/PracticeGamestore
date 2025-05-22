using Microsoft.EntityFrameworkCore;

namespace PracticeGamestore.DataAccess.Repositories.Order;

public class OrderRepository(GamestoreDbContext context) : IOrderRepository
{
    private readonly IQueryable<Entities.Order> _ordersNoTracking = context.Orders.AsNoTracking();
    
    public async Task<IEnumerable<Entities.Order>> GetAllAsync()
    {
        return await _ordersNoTracking.ToListAsync();
    }

    public async Task<Entities.Order?> GetByIdAsync(Guid id)
    {
        return await _ordersNoTracking.FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<Guid> CreateAsync(Entities.Order order)
    {
        var entry = await context.Orders.AddAsync(order);
        return entry.Entity.Id;
    }

    public void Update(Entities.Order order)
    {
        context.Orders.Update(order);
    }

    public async Task DeleteAsync(Guid id)
    {
        var order = await context.Orders.FindAsync(id);

        if (order is not null)
        {
            context.Orders.Remove(order);
        }
    }
}