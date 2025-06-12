using PracticeGamestore.Business.Constants;

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
            
            context.Response.ContentType = "application/json";

            int statusCode;
            string message;

            if (e is ArgumentException)
            {
                statusCode = StatusCodes.Status400BadRequest;
                message = e.Message;
            }
            else
            {
                statusCode = StatusCodes.Status500InternalServerError;
                message = ErrorMessages.GlobalError;
            }
            
            context.Response.StatusCode = statusCode;
            
            var response = new { message };
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}