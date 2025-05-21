using PracticeGamestore.Business.DataTransferObjects;
namespace PracticeGamestore.Business.Services.Publisher;

public interface IPublisherService
{
    public Task<IEnumerable<PublisherDto>> GetAllAsync();
    public Task<PublisherDto?> GetByIdAsync(Guid id);
    public Task<Guid?> CreateAsync(PublisherDto model);
    public Task<bool> UpdateAsync(PublisherDto model);
    public Task DeleteAsync(Guid id);
}