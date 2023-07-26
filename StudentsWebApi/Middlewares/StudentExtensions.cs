using Microsoft.AspNetCore.Builder;

public static class StudentExtensions
{
    public static IApplicationBuilder UseSimpleLogging(this IApplicationBuilder app)
    {
        app.UseMiddleware<LoggingMiddleware>();
        return app;
    }
}