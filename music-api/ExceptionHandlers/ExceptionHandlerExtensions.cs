namespace music_api.ExceptionHandlers;

public static class ExceptionHandlerExtensions
{
    public static IServiceCollection AddGlobalExceptionHandlers(this IServiceCollection services)
    {
        services.AddExceptionHandler<NotFoundExceptionHandler>();
        services.AddExceptionHandler<ValidationExceptionHandler>();
        services.AddExceptionHandler<ConflictExceptionHandler>();
        services.AddExceptionHandler<ForbiddenExceptionHandler>();
        services.AddExceptionHandler<ServerExceptionHandler>();
        services.AddProblemDetails();
        
        return services;
    }
}