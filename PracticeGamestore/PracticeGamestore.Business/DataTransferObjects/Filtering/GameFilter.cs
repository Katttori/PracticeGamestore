namespace PracticeGamestore.Business.DataTransferObjects.Filtering;

public class GameFilter
{
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public DateTime? ReleaseDateStart { get; set; }
    public DateTime? ReleaseDateEnd { get; set; }
    public List<int>? Age { get; set; }
    public double? RatingFrom { get; set; }
    public double? RatingTo { get; set; }
    public string? Name { get; set; }
    public string? Order { get; set; }
    public List<string>? OrderBy { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}

