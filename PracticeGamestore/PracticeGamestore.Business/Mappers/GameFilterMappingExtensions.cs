using PracticeGamestore.DataAccess.Enums;
using PracticeGamestore.DataAccess.Filtering;

namespace PracticeGamestore.Business.Mappers;

public static class GameFilterMappingExtensions
{
    public static GameFilter MapToDataAccessGameFilter(this Filtering.GameFilter filter)
    {
        return new ()
        {
            Age = filter.Age?.Count > 0 ? filter.Age.Select(a => (AgeRating)a).ToHashSet() : null,        
            MinPrice = filter.MinPrice,
            MaxPrice = filter.MaxPrice,
            Name = filter.Name is null? null : NormalizeStringValue(filter.Name),
            OrderType = filter.Order is null? "asc": NormalizeStringValue(filter.Order),
            OrderByFields = filter.OrderBy?.Count > 0 ? filter.OrderBy.Select(NormalizeStringValue).Distinct().ToList() : ["name"],
            RatingTo = filter.RatingTo,
            RatingFrom = filter.RatingFrom,
            ReleaseDateStart = filter.ReleaseDateStart,
            ReleaseDateEnd = filter.ReleaseDateEnd,
            Page = filter.Page ?? 1,
            PageSize = filter.PageSize ?? 10
        };
    }
    
    private static string NormalizeStringValue(string value) => value.Trim().ToLowerInvariant();
}