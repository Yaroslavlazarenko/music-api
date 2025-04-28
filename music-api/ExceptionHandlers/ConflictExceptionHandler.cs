using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using music_api.Exceptions;

namespace music_api.ExceptionHandlers;

public class ConflictExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ConflictException)
        {
            return false;
        }
        
        var details = new ProblemDetails()
        {
            Status = StatusCodes.Status409Conflict,
            Title = string.IsNullOrWhiteSpace(exception.Message) ?  "Some conflict error" : exception.Message
        };
        
        httpContext.Response.StatusCode = details.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(details, cancellationToken);

        return true;
    }
}