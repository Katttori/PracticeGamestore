using Microsoft.EntityFrameworkCore;

namespace PracticeGamestore.DataAccess.Repositories.User;

public class UserRepository(GamestoreDbContext context) : IUserRepository
{
    private readonly IQueryable<Entities.User> _usersNoTracking = context.Users.AsNoTracking();
    
    public async Task<IEnumerable<Entities.User>> GetAllAsync()
    {
        return await _usersNoTracking
            .Where(u => u.Status != "Deleted")
            .ToListAsync();
    }

    public async Task<Entities.User?> GetByIdAsync(Guid id)
    {
        return await _usersNoTracking
                .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<Guid?> CreateAsync(Entities.User user)
    {
        var existingUser = await _usersNoTracking
            .FirstOrDefaultAsync(u => u.Email == user.Email);

        if (existingUser != null)
        {
            if (existingUser.Status == "Deleted")
            {
                existingUser.Status = "Active";
                existingUser.UserName = user.UserName;
                existingUser.PhoneNumber = user.PhoneNumber;
                existingUser.PasswordHash = user.PasswordHash;
                existingUser.CountryId = user.CountryId;
                existingUser.BirthDate = user.BirthDate;
                existingUser.Role = user.Role;
                return existingUser.Id;
            }

            return null;
        }
        
        user.Id = Guid.NewGuid();
        await context.Users.AddAsync(user);
        return user.Id;
    }
    
    public Task Update(Entities.User user)
    {
        context.Users.Update(user);
        return Task.CompletedTask;
    }
    
    public async Task DeleteAsync(Guid id)
    {
        var user = await context.Users.FindAsync(id);
        if (user is not null)
        {
            user.Status = "Deleted";
            context.Users.Update(user);
        }
    }

    public async Task<bool> BanAsync(Guid id)
    {
        var user = await context.Users.FindAsync(id);
        if (user is null || user.Status == "Banned")
        {
            return false; // User not found or already banned
        }
        
        user.Status = "Banned";
        context.Users.Update(user);
        return true;
    }
}