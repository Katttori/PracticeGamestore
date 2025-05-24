namespace PracticeGamestore.DataAccess.Repositories.Country;

public interface ICountryRepository
{ 
    Task<IEnumerable<Entities.Country>> GetAllAsync(); 
    Task<Entities.Country?> GetByIdAsync(Guid id); 
    Task<Guid> CreateAsync(Entities.Country country); 
    void Update(Entities.Country country); 
    Task DeleteAsync(Guid id);
}