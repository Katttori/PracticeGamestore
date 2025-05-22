using Microsoft.EntityFrameworkCore;

namespace PracticeGamestore.DataAccess.Repositories.Country;

public class CountryRepository(GamestoreDbContext context) : ICountryRepository
{
    private readonly IQueryable<Entities.Country> _countriesNoTracking = context.Countries.AsNoTracking();

    public async Task<IEnumerable<Entities.Country>> GetAllAsync()
    {
        return await _countriesNoTracking.ToListAsync();
    }

    public async Task<Entities.Country?> GetByIdAsync(Guid id)
    {
        return await _countriesNoTracking.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Guid> CreateAsync(Entities.Country country)
    {
        var c = await context.Countries.AddAsync(country);
        return c.Entity.Id;
    }
    
    public void Update(Entities.Country country)
    {
        context.Countries.Update(country);
    }
    
    public async Task DeleteAsync(Guid id)
    {
        var country = await context.Countries.FindAsync(id);
        if (country == null) return;

        context.Countries.Remove(country);
    }

}