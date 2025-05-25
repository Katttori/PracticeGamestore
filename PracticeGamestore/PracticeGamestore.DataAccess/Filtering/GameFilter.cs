using PracticeGamestore.DataAccess.Enums;

namespace PracticeGamestore.DataAccess.Filtering;

public class GameFilter
{
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public DateTime? ReleaseDateStart { get; set; }
    public DateTime? ReleaseDateEnd { get; set; }
    public HashSet<AgeRating>? Age { get; set; }
    public double? RatingFrom { get; set; }
    public double? RatingTo { get; set; }
    public string? Name { get; set; }
    public string OrderType { get; set; } = "asc";
    public List<string> OrderByFields { get; set; } = ["name"];
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}