using Microsoft.IdentityModel.Tokens;
using PracticeGamestore.DataAccess.Enums;

namespace PracticeGamestore.Business.Filtering;

public class GameFilter
{
    private static readonly HashSet<string> ValidOrderByFields = new(StringComparer.OrdinalIgnoreCase)
    {
        "name", "price", "rating", "age", "release-date"
    };
    
    private static readonly HashSet<string> ValidOrderDirections = new(StringComparer.OrdinalIgnoreCase)
    {
        "asc", "desc"
    };
    
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public DateTime? ReleaseDateStart { get; set; }
    public DateTime? ReleaseDateEnd { get; set; }
    public List<AgeRating>? Age { get; set; }
    public double? RatingFrom { get; set; }
    public double? RatingTo { get; set; }
    public string? Name { get; set; }
    public string? Order { get; set; }
    public List<string>? OrderBy { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }

    public bool IsInvalid() =>
        MinPrice is < 0 ||
        MaxPrice is < 0 ||
        (MinPrice.HasValue && MaxPrice.HasValue && MinPrice > MaxPrice) ||
        (ReleaseDateStart.HasValue && ReleaseDateEnd.HasValue && ReleaseDateStart > ReleaseDateEnd) ||
        RatingFrom is < 0 or > 5 ||
        RatingTo is < 0 or > 5 ||
        (RatingFrom.HasValue && RatingTo.HasValue && RatingFrom > RatingTo) ||
        (!string.IsNullOrEmpty(Order) && !ValidOrderDirections.Contains(Order)) ||
        (!Age.IsNullOrEmpty() && Age.Any(a => !Enum.IsDefined(typeof(AgeRating), a))) ||
        (!OrderBy.IsNullOrEmpty() && OrderBy.Any(o => !ValidOrderByFields.Contains(o.Trim())))
        || PageSize is < 0 || Page is < 0;
}

