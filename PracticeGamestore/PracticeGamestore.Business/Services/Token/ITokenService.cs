using PracticeGamestore.Business.DataTransferObjects;

namespace PracticeGamestore.Business.Services.Token;

public interface ITokenService
{
    TokenResponseDto GenerateJwtToken(DataAccess.Entities.User user);
}