using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;

    public LoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        
        Console.WriteLine($"Action: {context.Request.Path}");

        // Sonraki middleware'ı çağır
        await _next(context);
    }
}
