using Microsoft.EntityFrameworkCore;

namespace PracticeGamestore.DataAccess.Repositories.Order;

public class OrderRepository(GamestoreDbContext context) : IOrderRepository
{
    private readonly IQueryable<Entities.Order> _ordersWithGamesIncluded = context.Orders
        .Include(o => o.GameOrders)
            .ThenInclude(go => go.Game)
                .ThenInclude(g => g.Publisher)
        .Include(o => o.GameOrders)
            .ThenInclude(go => go.Game)
                .ThenInclude(g => g.GamePlatforms)
                    .ThenInclude(gp => gp.Platform)
        .Include(o => o.GameOrders)
            .ThenInclude(go => go.Game)
                .ThenInclude(g => g.GameGenres)
                    .ThenInclude(gg => gg.Genre);
    
    public async Task<IEnumerable<Entities.Order>> GetAllAsync()
    {
        return await _ordersWithGamesIncluded.AsNoTracking().ToListAsync();
    }

    public async Task<Entities.Order?> GetByIdAsync(Guid id)
    {
        return await _ordersWithGamesIncluded.FirstOrDefaultAsync(g => g.Id == id);
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