using PracticeGamestore.Business.DataTransferObjects;

namespace PracticeGamestore.Business.Services.Auth;

public interface IAuthService
{
    Task<TokenResponseDto?> AuthenticateUser(string email, string password);

}