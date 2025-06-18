using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Options;

namespace PracticeGamestore.Business.Services.Token;

public class TokenService(IOptions<JwtOptions> jwtOptions) : ITokenService
{
    public static TokenValidationParameters CreateTokenValidationParameters(JwtOptions jwtOptions)
    {
        return new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    }

    public TokenResponseDto GenerateJwtToken(DataAccess.Entities.User user)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = CreateSecurityKey();
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = GenerateClaims(user),
            Expires = DateTime.UtcNow.AddMinutes(jwtOptions.Value.ExpirationTimeInMinutes),
            SigningCredentials = credentials,
            Issuer = jwtOptions.Value.Issuer,
            Audience = jwtOptions.Value.Audience
        };

        var token = handler.CreateToken(tokenDescriptor);
        return new TokenResponseDto(user.Id, handler.WriteToken(token), jwtOptions.Value.ExpirationTimeInMinutes);
    }

    private SymmetricSecurityKey CreateSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.SecretKey));
    }

    private static ClaimsIdentity GenerateClaims(DataAccess.Entities.User user)
    {
        return new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        });
    }
}