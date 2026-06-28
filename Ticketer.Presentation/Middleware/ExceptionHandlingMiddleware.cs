using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace Ticketer.Presentation.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new { error = exception.Message };

        switch (exception)
        {
            case ValidationException validationEx:
                // 400 Bad Request: Input data was invalid (caught by FluentValidation)
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var errors = string.Join("; ", validationEx.Errors);
                response = new { error = $"Validation failed: {errors}" };
                break;

            case InvalidOperationException invalidOpEx:
                // 400 Bad Request: A domain rule was broken (e.g., seat already taken)
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;

            case DbUpdateConcurrencyException concurrencyEx:
                // 409 Conflict: Two users tried to buy the same seat at the exact same time
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                response = new { error = "The seats were modified by another user. Please try again." };
                break;

            default:
                // 500 Internal Server Error: Something unexpected crashed
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response = new { error = "An internal server error occurred." };
                break;
        }

        var result = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(result);
    }
}