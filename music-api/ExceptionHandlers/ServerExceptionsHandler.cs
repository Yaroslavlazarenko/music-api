using Microsoft.AspNetCore.Diagnostics;

namespace music_api.ExceptionHandlers;

public class ServerExceptionsHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = 500;

        await httpContext.Response
            .WriteAsJsonAsync(new
                {
                    message = "Oups..."
                },
                cancellationToken);
        
        return true;
    }
}