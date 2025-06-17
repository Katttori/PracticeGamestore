namespace PracticeGamestore.Models.Auth;

public class TokenResponseModel
{
    public Guid UserId { get; set; }
    public required string Token { get; set; }
    public required long Expiration { get; set; }
}