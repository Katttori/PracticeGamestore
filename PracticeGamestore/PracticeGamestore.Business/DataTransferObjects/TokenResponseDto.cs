namespace PracticeGamestore.Business.DataTransferObjects;

public class TokenResponseDto
{
    public Guid UserId { get; set; }
    public string Token { get; set; }
    public long Expiration { get; set; }

    public TokenResponseDto(Guid userId, string token, long expiration)
    {
        UserId = userId;
        Token = token;
        Expiration = expiration;
    }
}