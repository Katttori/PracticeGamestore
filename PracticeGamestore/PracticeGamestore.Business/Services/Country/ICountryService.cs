using PracticeGamestore.Business.DataTransferObjects;

namespace PracticeGamestore.Business.Services.Country;

public interface ICountryService
{
    Task<IEnumerable<CountryDto>> GetAllAsync();
    Task<CountryDto?> GetByIdAsync(Guid id);
    Task<CountryDto?> GetByNameAsync(string name);
    Task<Guid?> CreateAsync(CountryDto country);
    Task<bool> UpdateAsync(CountryDto country);
    Task DeleteAsync(Guid id);
}