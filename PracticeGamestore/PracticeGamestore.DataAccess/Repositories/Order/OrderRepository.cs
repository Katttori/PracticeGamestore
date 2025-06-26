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

    public async Task<Dictionary<string, string>> GetGameKeysByOrderIdAsync(Guid id)
    {
        var order = await _ordersWithGamesIncluded.FirstOrDefaultAsync(o => o.Id == id);
        if (order is null)
        {
            return new Dictionary<string, string>();
        }

        return order.GameOrders.Select(go => new 
            {
                GameName = go.Game.Name,
                GameKey = go.Game.Key
                
            }).ToDictionary(x => x.GameName, x => x.GameKey);
    }
    
    public async Task<IEnumerable<Entities.Order>> GetOrdersByUserEmailAsync(string userEmail)
    {
        return await _ordersWithGamesIncluded
            .Where(o => o.UserEmail == userEmail)
            .ToListAsync();
    }
}