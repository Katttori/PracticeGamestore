using Microsoft.AspNetCore.Mvc.Filters;
using PracticeGamestore.Business.Constants;

namespace PracticeGamestore.Filters;

public class BirthdateRestrictionFilter: ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var underage = true;
        if (context.HttpContext.Request.Headers.TryGetValue(HeaderNames.Birthdate, out var value)
            && !string.IsNullOrEmpty(value) && DateOnly.TryParse(value, out var birthdate))
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var age = today.Year - birthdate.Year;
            if (today < birthdate.AddYears(age)) age--;
            underage = age < 18;

        }
        context.HttpContext.Items[HttpContextCustomItems.UnderageIndicator] = underage;
    }
}
