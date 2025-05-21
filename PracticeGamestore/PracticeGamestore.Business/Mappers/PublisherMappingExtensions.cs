using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.DataAccess.Entities;
namespace PracticeGamestore.Business.Mappers;

public static class PublisherMappingExtensions
{
    public static PublisherDto ToDto(this Publisher publisher)
    {
        return new (publisher.Id, publisher.Name, publisher.Description, publisher.PageUrl);
    }

    public static Publisher ToEntity(this PublisherDto publisherDto)
    {
        return new ()
        {
            Name = publisherDto.Name,
            Description = publisherDto.Description,
            PageUrl = publisherDto.PageUrl,
            Id = publisherDto.Id
        };
    }
    
}