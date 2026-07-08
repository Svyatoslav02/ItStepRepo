using System.Net;
using MoodboardAI.Api.Models;

namespace MoodboardAI.Api.Middleware;

/// <summary>
/// Catches any unhandled exception thrown further down the request pipeline
/// and converts it into a standardized <see cref="ErrorResponse"/> JSON body
/// with a 500 status code, instead of letting ASP.NET Core return its
/// default (non-standard) developer exception page or empty response.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionHandlingMiddleware"/> class.
    /// </summary>
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invokes the middleware, forwarding the request and catching unhandled exceptions.
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unhandled exception while processing {Method} {Path}",
                context.Request.Method, context.Request.Path);

            context.Response.Clear();
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new ErrorResponse
            {
                Message = "An unexpected error occurred. Please try again later."
            });
        }
    }
}
