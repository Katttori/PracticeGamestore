using Microsoft.AspNetCore.Mvc.Filters;

namespace PracticeGamestore.Filters;

public class BirthdateRestrictionFilter: ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue("X-Birthdate", out var value)
            || string.IsNullOrEmpty(value) || !DateOnly.TryParse(value, out var birthdate)) return;
        
        var today = DateOnly.FromDateTime(DateTime.Today);
        var age = today.Year - birthdate.Year;
        if (today < birthdate.AddYears(age)) age--;
        context.HttpContext.Items["Age"] = age;

    }
}