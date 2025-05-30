namespace PracticeGamestore.Models.Game;

public class PaginatedGameListResponseModel
{
    public List<GameResponseModel> Games { get; set; } = [];
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int Count { get; set; }
}