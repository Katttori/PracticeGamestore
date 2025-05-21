using PracticeGamestore.Business.DataTransferObjects;

namespace PracticeGamestore.Business.Services.Country;

public interface ICountryService
{
    public Task<IEnumerable<CountryDto>> GetAllAsync();
    public Task<CountryDto?> GetByIdAsync(Guid id);
    public Task<Guid?> CreateAsync(CountryDto country);
    public Task<bool> UpdateAsync(CountryDto country);
    public Task DeleteAsync(Guid id);
}