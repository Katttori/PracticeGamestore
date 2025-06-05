using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.DataAccess.Enums;
using PracticeGamestore.DataAccess.Repositories.Country;
using PracticeGamestore.DataAccess.UnitOfWork;

namespace PracticeGamestore.Business.Services.Country;

public class CountryService(ICountryRepository countryRepository, IUnitOfWork unitOfWork) : ICountryService
{
    public async Task<IEnumerable<CountryDto>> GetAllAsync()
    {
        var countries = await countryRepository.GetAllAsync();
        return countries.Select(c => c.MapToCountryDto());
    }

    public async Task<CountryDto?> GetByIdAsync(Guid id)
    {
        var country = await countryRepository.GetByIdAsync(id);
        return country?.MapToCountryDto();
    }
    
    public async Task<Guid?> CreateAsync(CountryDto country)
    {
        if (await countryRepository.ExistsByNameAsync(country.Name))
        {
            throw new ArgumentException($"Country with name '{country.Name}' already exists.");
        }
        
        var entity = country.MapToCountryEntity();
        var id = await countryRepository.CreateAsync(entity);
        var changes = await unitOfWork.SaveChangesAsync();
        return changes > 0 ? id : null;
    }
    
    public async Task<bool> UpdateAsync(CountryDto country)
    {
        var entity = await countryRepository.GetByIdAsync(country.MapToCountryEntity().Id);
        if (entity == null) return false;

        if (country.Name != entity.Name && await countryRepository.ExistsByNameAsync(country.Name))
        {
            throw new ArgumentException($"Country with name '{country.Name}' already exists.");
        }

        entity.Name = country.Name;
        entity.CountryStatus = (CountryStatus)country.Status;
        
        countryRepository.Update(entity);
        var changes = await unitOfWork.SaveChangesAsync();
        return changes > 0;
    }
    
    public async Task DeleteAsync(Guid id)
    {
        await countryRepository.DeleteAsync(id);
        await unitOfWork.SaveChangesAsync();
    }
}