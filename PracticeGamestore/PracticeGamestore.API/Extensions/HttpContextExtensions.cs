namespace PracticeGamestore.Extensions;

public static class HttpContextExtensions
{
    public static bool IsUnderage(this HttpContext context) =>
        context.Items["Underage"] as bool? ?? false;
}