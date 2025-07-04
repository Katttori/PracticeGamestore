namespace PracticeGamestore.Business.Options;

public class JwtOptions
{
    public const string SectionName = "Jwt";
    
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required string SecretKey { get; set; }
    public required int ExpirationTimeInMinutes { get; set; }
}