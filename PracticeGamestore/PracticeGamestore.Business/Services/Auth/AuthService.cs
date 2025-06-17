using Microsoft.Extensions.Logging;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Services.Token;
using PracticeGamestore.Business.Utils;
using PracticeGamestore.DataAccess.Repositories.User;

namespace PracticeGamestore.Business.Services.Auth;

public class AuthService(ITokenService tokenService,
                        IUserRepository userRepository, 
                        ILogger<AuthService> logger) : IAuthService
{
   
    
    public async Task<TokenResponseDto?> AuthenticateUser(string email, string password)
    {
        var user = await userRepository.GetByEmailAsync(email.Trim());
        
        if (user is null)
        {
            logger.LogWarning("User with email {} does not exist.", email);
            return null;
        }

        var hashedPassword = PasswordHasher.HashPassword(password, user.PasswordSalt);
        return hashedPassword != user.PasswordHash ? null : tokenService.GenerateJwtToken(user);
    }
}