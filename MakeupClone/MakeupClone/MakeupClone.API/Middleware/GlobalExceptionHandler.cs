using System.Net;
using Microsoft.AspNetCore.Diagnostics;

namespace MakeupClone.API.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        var statusCode = exception switch
        {
            KeyNotFoundException => HttpStatusCode.NotFound,
            UnauthorizedAccessException => HttpStatusCode.Unauthorized,
            ArgumentException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };

        _logger.LogError(exception, "An unhandled exception occurred while processing the request.");

        await CreateErrorResponseAsync(context, exception, statusCode, cancellationToken);

        return true;
    }

    private static Task CreateErrorResponseAsync(HttpContext context, Exception exception, HttpStatusCode statusCode, CancellationToken cancellationToken)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var errorResponse = new
        {
            exception.Message,
            ErrorCode = statusCode.ToString()
        };

        return context.Response.WriteAsJsonAsync(errorResponse, cancellationToken);
    }
}