using Microsoft.AspNetCore.Diagnostics;
using music_api.Exceptions;

namespace music_api.ExceptionHandlers;

public class PerformerValidationExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is PerformerValidationException ex)
        {
            httpContext.Response.StatusCode = 400;
            await httpContext.Response.WriteAsJsonAsync(ex.Errors, cancellationToken);
            return true;
        }
        return false;
    }
}
