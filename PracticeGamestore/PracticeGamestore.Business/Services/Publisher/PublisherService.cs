using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.DataAccess.Repositories;
using PracticeGamestore.DataAccess.UnitOfWork;
namespace PracticeGamestore.Business.Services.Publisher;

public class PublisherService(IPublisherRepository repository, IUnitOfWork unitOfWork): IPublisherService
{
    public async Task<IEnumerable<PublisherDto>> GetAllAsync()
    {
        var publishers = await repository.GetAllAsync();
        return publishers.Select(p => p.ToDto());
    }

    public async Task<PublisherDto?> GetByIdAsync(Guid id)
    {
        var publisher = await repository.GetByIdAsync(id);
        return publisher?.ToDto();
    }

    public async Task<Guid?> CreateAsync(PublisherDto dto)
    {
        var id = await repository.CreateAsync(dto.ToEntity());
        var changes = await unitOfWork.SaveChangesAsync();
        return changes > 0 ? id : null;
    }

    public async Task<bool> UpdateAsync(PublisherDto dto)
    {
        var existingPublisher = await GetByIdAsync(dto.Id);
        if (existingPublisher is null) return false;
        repository.Update(dto.ToEntity());
        var changes = await unitOfWork.SaveChangesAsync();
        return changes > 0;
    }

    public async Task DeleteAsync(Guid id)
    {
        await repository.DeleteAsync(id);
        await unitOfWork.SaveChangesAsync();
    }
}