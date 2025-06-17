using Microsoft.AspNetCore.Mvc;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Business.Services.Auth;
using PracticeGamestore.Business.Services.Country;
using PracticeGamestore.Business.Services.Location;
using PracticeGamestore.Business.Services.User;
using PracticeGamestore.Mappers;
using PracticeGamestore.Models.Auth;
using PracticeGamestore.Models.User;

namespace PracticeGamestore.Controllers;

[ApiController, Route("auth")]
public class AuthController(IUserService userService, 
    ICountryService countryService,
    ILocationService locationService,
    ILogger<AuthController> logger,
    IAuthService authService) : ControllerBase
{
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRequestModel model)
    {
        var country = await countryService.GetByIdAsync(model.CountryId);
        try
        {
            await locationService.HandleLocationAccessAsync(country?.Name, model.Email);

        }
        catch (ArgumentException)
        {
            logger.LogError(ErrorMessages.InvalidRegistrationCountry);
            return BadRequest(ErrorMessages.InvalidRegistrationCountry);
        }
        catch (UnauthorizedAccessException)
        {
            logger.LogWarning("User with email {Email} tried to register from banned country {CountryName}, added to blacklist", 
                model.Email, country!.Name);
            return BadRequest(ErrorMessages.FailedRegistrationBecauseOfBannedCountry(country.Name));
        }

        var id = await userService.CreateAsync(model.MapToUserDto());
        
        if (id is null)
        {
            logger.LogError(ErrorMessages.RegistrationFailed);
            return BadRequest(ErrorMessages.RegistrationFailed);
        }
        
        return CreatedAtAction("GetById", "User", new { id }, id);        
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var token = await authService.AuthenticateUser(request.Email, request.Password);
        return token is null 
            ? NotFound(ErrorMessages.FailedLogIn(request.Email)) 
            : Ok(token.MapToTokenResponseModel());
    }
}