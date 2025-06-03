using PracticeGamestore.Business.DataTransferObjects;

namespace PracticeGamestore.Business.Services.Publisher;

public interface IPublisherService
{
    Task<IEnumerable<PublisherDto>> GetAllAsync();
    Task<PublisherDto?> GetByIdAsync(Guid id);
    Task<Guid?> CreateAsync(PublisherDto model);
    Task<bool> UpdateAsync(Guid id, PublisherDto model);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<GameResponseDto>?> GetGamesAsync(Guid id);
}