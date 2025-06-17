using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PracticeGamestore.Business.Enums;
using PracticeGamestore.Business.Services.Token;
using PracticeGamestore.Business.Services.User;
using PracticeGamestore.Mappers;
using PracticeGamestore.Models.User;

namespace PracticeGamestore.Controllers;

[ApiController, Route("users")]
public class UserController(
    IUserService userService,
    ITokenService tokenService,
    ILogger<UserController> logger) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<IActionResult> GetAll()
    {
        var users = await userService.GetAllAsync();
        return Ok(users.Select(u => u.MapToUserResponseModel()));
    }
    
    [HttpGet("{id:guid}")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await userService.GetByIdAsync(id);
        if (user is null)
        {
            logger.LogWarning("User with ID {Id} not found", id);
            return NotFound("User not found");
        }
        
        return Ok(user.MapToUserResponseModel());
    }
    
    [HttpPost]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<IActionResult> Create([FromBody] UserRequestModel request)
    {
        var id = await userService.CreateAsync(request.MapToUserDto());
        
        if (id is null)
        {
            logger.LogWarning("Failed to create user with email {Email}", request.Email);
            return BadRequest("User creation failed. Email might already exist.");
        }
        
        logger.LogInformation("User created with ID {Id}", id);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }
    
    [HttpPut("{id:guid}")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<IActionResult> Update(Guid id, [FromBody] UserRequestModel request)
    {
        var updated = await userService.UpdateAsync(id, request.MapToUserDto());
        
        if (!updated)
        {
            logger.LogWarning("Failed to update user with ID {Id}", id);
            return BadRequest("User update failed. User might not exist or email might already be in use.");
        }
        
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<IActionResult> Delete(Guid id)
    {
        await userService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("{id:guid}/ban")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<IActionResult> Ban(Guid id)
    {
        var user = await userService.BanUserAsync(id);
        if (user) return NoContent();
        logger.LogWarning("Failed to ban user with ID {Id}", id);
        return BadRequest("User ban failed. User might not exist or is already banned.");
    }
    
    [HttpGet("/token")]
    public IActionResult GetToken([FromQuery] string email, [FromQuery] string role, [FromQuery] Guid id)
    {
        // this endpoint is only for testing purposes
        // should be deleted later
        
        UserRole userRole;

        switch (role)
        {
            case "Admin":
                userRole = UserRole.Admin;
                break;
            case "User":
                userRole = UserRole.User;
                break;
            case "Manager":
                userRole = UserRole.Manager;
                break;
            default:
                userRole = UserRole.User;
                break;
        }
        
        var user = new UserRequestModel()
        {
            Email = email,
            Role = userRole,
        }.MapToUserDto();
        user.Id = id;

        var token = tokenService.GenerateJwtToken(user);
        return Ok(new { token });
    }
}