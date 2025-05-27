using System.Text.Json;

namespace PracticeGamestore.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unhandled exception occured.");
            
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            
            var response = new { message = "something happened?" };
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}