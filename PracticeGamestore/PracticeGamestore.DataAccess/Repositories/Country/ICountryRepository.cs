namespace PracticeGamestore.DataAccess.Repositories.Country;

public interface ICountryRepository
{
    public Task<IEnumerable<Entities.Country>> GetAllAsync();
    public Task<Entities.Country?> GetByIdAsync(Guid id);
    public Task<Guid> CreateAsync(Entities.Country country);
    public void Update(Entities.Country country);
    public Task DeleteAsync(Guid id);
}