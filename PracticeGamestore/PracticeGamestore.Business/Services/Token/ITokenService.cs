using PracticeGamestore.Business.DataTransferObjects;

namespace PracticeGamestore.Business.Services.Token;

public interface ITokenService
{
    string GenerateJwtToken(UserDto user);
}