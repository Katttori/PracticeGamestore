using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Models.Auth;

namespace PracticeGamestore.Mappers;

public static class TokenResponseMappingExtensions
{
    public static TokenResponseDto MapToTokenResponseDto(this TokenResponse tokenResponse)
    {
        return new TokenResponseDto(tokenResponse.UserId, tokenResponse.Token, tokenResponse.Expiration);
    }

    public static TokenResponse MapToTokenResponseModel(this TokenResponseDto dto)
    {
        return new TokenResponse
        {
            UserId = dto.UserId,
            Token = dto.Token,
            Expiration = dto.Expiration
        };
    }
}