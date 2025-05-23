using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Models.Publisher;

namespace PracticeGamestore.Mappers;

public static class PublisherMappingExtensions
{
    public static PublisherDto MapToPublisherDto(this PublisherRequestModel model, Guid? id = null)
    {
        return new (id, model.Name, model.Description, model.PageUrl);
    }

    public static PublisherResponseModel MapToPublisherModel(this PublisherDto publisherDto)
    {
        return new ()
        {
            Id = publisherDto.Id,
            Name = publisherDto.Name,
            PageUrl = publisherDto.PageUrl,
            Description = publisherDto.Description
        };
    }
}