using PracticeGamestore.Business.DataTransferObjects;

namespace PracticeGamestore.Business.Services.Publisher;

public interface IPublisherService
{
    Task<IEnumerable<PublisherDto>> GetAllAsync();
    Task<PublisherDto?> GetByIdAsync(Guid id);
    Task<Guid?> CreateAsync(PublisherDto model);
    Task<bool> UpdateAsync(PublisherDto model);
    Task DeleteAsync(Guid id);
}