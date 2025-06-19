using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Business.Services.User;

namespace PracticeGamestore.Filters;

public class BirthdateRestrictionFromDbFilter: IAsyncActionFilter
{
    private readonly IUserService _userService;
    
    public BirthdateRestrictionFromDbFilter(IUserService userService)
    {
        _userService = userService;
    }
    
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        bool underage = true;
        
        var userIdStr = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (Guid.TryParse(userIdStr, out var userId))
        {
            var user = await _userService.GetByIdAsync(userId);
            if (user?.BirthDate is not null)
            {
                var birthdate = user.BirthDate;
                
                var today = DateTime.Today;
                var age = today.Year - birthdate.Year;
                if (today < birthdate.AddYears(age)) age--;

                underage = age < 18;
            }
        }

        context.HttpContext.Items[HttpContextCustomItems.UnderageIndicator] = underage;
        
        await next();
    }
}