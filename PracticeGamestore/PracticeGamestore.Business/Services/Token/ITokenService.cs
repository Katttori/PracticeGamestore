using Microsoft.IdentityModel.Tokens;
using PracticeGamestore.Business.DataTransferObjects;

namespace PracticeGamestore.Business.Services.Token;

public interface ITokenService
{
    TokenResponseDto GenerateToken(DataAccess.Entities.User user);
}