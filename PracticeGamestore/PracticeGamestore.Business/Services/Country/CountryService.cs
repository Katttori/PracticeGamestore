using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.DataAccess.Repositories.Country;
using PracticeGamestore.DataAccess.UnitOfWork;

namespace PracticeGamestore.Business.Services.Country;

public class CountryService : ICountryService
{
    private readonly ICountryRepository _countryRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public CountryService(ICountryRepository countryRepository, IUnitOfWork unitOfWork)
    {
        _countryRepository = countryRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IEnumerable<CountryDto>> GetAllAsync()
    {
        var countries = await _countryRepository.GetAllAsync();
        return countries.Select(c => c.ToDto());
    }

    public async Task<CountryDto?> GetByIdAsync(Guid id)
    {
        var country = await _countryRepository.GetByIdAsync(id);
        return country?.ToDto();
    }
    
    public async Task<Guid?> CreateAsync(CountryDto country)
    {
        var entity = country.ToEntity();
        var id = await _countryRepository.CreateAsync(entity);
        var changes = await _unitOfWork.SaveChangesAsync();
        return changes > 0 ? entity.Id : null;
    }
    
    public async Task<bool> UpdateAsync(CountryDto country)
    {
        var entity = await _countryRepository.GetByIdAsync(country.Id);
        if (entity == null) return false;

        entity.Name = country.Name;
        entity.CountryStatus = country.CountryStatus;
        
        _countryRepository.Update(entity);
        var changes = await _unitOfWork.SaveChangesAsync();
        return changes > 0;
    }
    
    public async Task DeleteAsync(Guid id)
    {
        await _countryRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }
    
    
}