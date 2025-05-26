using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.DataAccess.Repositories.Country;
using PracticeGamestore.DataAccess.UnitOfWork;

namespace PracticeGamestore.Business.Services.Country;

public class CountryService(ICountryRepository countryRepository, IUnitOfWork unitOfWork) : ICountryService
{
    public async Task<IEnumerable<CountryDto>> GetAllAsync()
    {
        var countries = await countryRepository.GetAllAsync();
        return countries.Select(c => c.ToDto());
    }

    public async Task<CountryDto?> GetByIdAsync(Guid id)
    {
        var country = await countryRepository.GetByIdAsync(id);
        return country?.ToDto();
    }
    
    public async Task<Guid?> CreateAsync(CountryDto country)
    {
        var entity = country.ToEntity();
        var id = await countryRepository.CreateAsync(entity);
        var changes = await unitOfWork.SaveChangesAsync();
        return changes > 0 ? entity.Id : null;
    }
    
    public async Task<bool> UpdateAsync(CountryDto country)
    {
        var entity = await countryRepository.GetByIdAsync(country.Id);
        if (entity == null) return false;

        entity.Name = country.Name;
        entity.CountryStatus = country.Status;
        
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