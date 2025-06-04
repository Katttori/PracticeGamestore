using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.DataAccess.Repositories.Game;
using PracticeGamestore.DataAccess.Repositories.Publisher;
using PracticeGamestore.DataAccess.UnitOfWork;

namespace PracticeGamestore.Business.Services.Publisher;

public class PublisherService(IPublisherRepository publisherRepository, IGameRepository gameRepository, IUnitOfWork unitOfWork) : IPublisherService
{
    public async Task<IEnumerable<PublisherDto>> GetAllAsync()
    {
        var publishers = await publisherRepository.GetAllAsync();
        return publishers.Select(p => p.MapToPublisherDto());
    }

    public async Task<PublisherDto?> GetByIdAsync(Guid id)
    {
        var publisher = await publisherRepository.GetByIdAsync(id);
        return publisher?.MapToPublisherDto();
    }

    public async Task<Guid?> CreateAsync(PublisherDto dto)
    {
        var id = await publisherRepository.CreateAsync(dto.MapToPublisherEntity());
        var changes = await unitOfWork.SaveChangesAsync();
        return changes > 0 ? id : null;
    }

    public async Task<bool> UpdateAsync(Guid id, PublisherDto dto)
    {
        var existingPublisher = await GetByIdAsync(id);
        if (existingPublisher is null) return false;
        var updatedPublisher = dto.MapToPublisherEntity();
        updatedPublisher.Id = id;
        publisherRepository.Update(updatedPublisher);
        var changes = await unitOfWork.SaveChangesAsync();
        return changes > 0;
    }

    public async Task DeleteAsync(Guid id)
    {
        await publisherRepository.DeleteAsync(id);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<GameResponseDto>?> GetGamesAsync(Guid id)
    {
        var publisherExists = await publisherRepository.ExistsAsync(id);
        if (!publisherExists) return null;
        var games = await gameRepository.GetByPublisherIdAsync(id);
        return games.Select(g => g.MapToGameDto());
    }
}