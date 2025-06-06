using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PracticeGamestore.Filters;

public class RequestModelValidationFilter(ILogger<RequestModelValidationFilter> logger) : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid) return;
        var errors = context.ModelState
            .Where(x => x.Value?.Errors.Count > 0)
            .SelectMany(x =>
                x.Value!.Errors.Select(e => $"{x.Key}: {e.ErrorMessage}")).ToList();
        
        logger.LogWarning("Model validation failed for {Action}. Errors:\n{Errors}", 
            context.ActionDescriptor.DisplayName, string.Join("\n", errors));
        
        context.Result = new BadRequestObjectResult(context.ModelState);
    }
}