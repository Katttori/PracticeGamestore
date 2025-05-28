using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.DataAccess.Repositories.Publisher;
using PracticeGamestore.DataAccess.UnitOfWork;

namespace PracticeGamestore.Business.Services.Publisher;

public class PublisherService(IPublisherRepository repository, IUnitOfWork unitOfWork) : IPublisherService
{
    public async Task<IEnumerable<PublisherDto>> GetAllAsync()
    {
        var publishers = await repository.GetAllAsync();
        return publishers.Select(p => p.MapToPublisherDto());
    }

    public async Task<PublisherDto?> GetByIdAsync(Guid id)
    {
        var publisher = await repository.GetByIdAsync(id);
        return publisher?.MapToPublisherDto();
    }

    public async Task<Guid?> CreateAsync(PublisherDto dto)
    {
        var id = await repository.CreateAsync(dto.MapToPublisherEntity());
        var changes = await unitOfWork.SaveChangesAsync();
        return changes > 0 ? id : null;
    }

    public async Task<bool> UpdateAsync(Guid id, PublisherDto dto)
    {
        var existingPublisher = await GetByIdAsync(id);
        if (existingPublisher is null) return false;
        var updatedPublisher = dto.MapToPublisherEntity();
        updatedPublisher.Id = id;
        repository.Update(updatedPublisher);
        var changes = await unitOfWork.SaveChangesAsync();
        return changes > 0;
    }

    public async Task DeleteAsync(Guid id)
    {
        await repository.DeleteAsync(id);
        await unitOfWork.SaveChangesAsync();
    }
}