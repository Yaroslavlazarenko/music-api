using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace music_api.ExceptionHandlers;

public class ValidationExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is ValidationException ex)
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            await httpContext.Response.WriteAsJsonAsync(new {
                Errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
            }, cancellationToken);
            return true;
        }
        return false;
    }
}
