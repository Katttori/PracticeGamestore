using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Models.Auth;

namespace PracticeGamestore.Mappers;

public static class TokenResponseMappingExtensions
{
    public static TokenResponseDto MapToTokenResponseDto(this TokenResponseModel tokenResponseModel)
    {
        return new TokenResponseDto(tokenResponseModel.UserId, tokenResponseModel.Token, tokenResponseModel.Expiration);
    }

    public static TokenResponseModel MapToTokenResponseModel(this TokenResponseDto dto)
    {
        return new TokenResponseModel
        {
            UserId = dto.UserId,
            Token = dto.Token,
            Expiration = dto.Expiration
        };
    }
}