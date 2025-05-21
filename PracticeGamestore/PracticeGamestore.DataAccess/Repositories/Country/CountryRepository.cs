using Microsoft.EntityFrameworkCore;

namespace PracticeGamestore.DataAccess.Repositories.Country;

public class CountryRepository(GamestoreDbContext context): ICountryRepository
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
    
    public async Task<Entities.Country>? UpdateAsync(Entities.Country country)
    {
        var c = await context.Countries.FirstOrDefaultAsync(c => c.Id == country.Id);
        if (c == null) return null;

        c.Name = country.Name;
        c.Blacklists = country.Blacklists;
        c.CountryStatus = country.CountryStatus;
        
        context.Countries.Update(c);
        return c;
    }
    
    public async Task DeleteAsync(Guid id)
    {
        var country = await context.Countries.FindAsync(id);
        if (country == null) return;

        context.Countries.Remove(country);
    }

}