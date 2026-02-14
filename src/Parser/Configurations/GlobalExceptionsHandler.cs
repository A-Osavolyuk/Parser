using Microsoft.AspNetCore.Diagnostics;

namespace Parser.Configurations;

public sealed class GlobalExceptionsHandler(ILogger<GlobalExceptionsHandler> logger) : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionsHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, 
        Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError("Handling global exception: {exception}", exception);

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(new Error
        {
            Code = "Server error",
            Description = exception.Message
        }, cancellationToken);
        
        _logger.LogError("Handled global exception");

        return true;
    }
}