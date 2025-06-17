namespace PracticeGamestore.Models.Auth;

public class TokenResponse
{
    public Guid UserId { get; set; }
    public required string Token { get; set; }
    public required long Expiration { get; set; }
}