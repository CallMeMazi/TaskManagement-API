using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TaskManagement.WebConfig.ExceeptionHandlers;
using TaskManagement.WebConfig.ExceeptionHandlers.AppExceptionHandlers;
using TaskManagement.WebConfig.ExceeptionHandlers.DomainExceptionHandlers;
using TaskManagement.WebConfig.Logging;

namespace TaskManagement.WebConfig.Registeration;

public static class ExceptionHandlerRegisteration
{
    // Support in .net 10
    extension(IServiceCollection services)
    {
        public IServiceCollection RegisterExceptionHandlersAndServices()
            => services.RegisterProblemDetailsService()
            .RegisterAppExceptionHandlers()
            .RegisterDomainExceptionHandlers()
            .RegisterGlobalExceptionHandlers();
    }

    public static void UseApplicationExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(new ExceptionHandlerOptions
        {
            SuppressDiagnosticsCallback = static _ => true
        });
    }

    private static IServiceCollection RegisterProblemDetailsService(this IServiceCollection services)
    {
        services.AddProblemDetails(opt =>
        {
            opt.CustomizeProblemDetails = ctx =>
            {
                ctx.ProblemDetails.Extensions["isSuccess"] = false;
                ctx.ProblemDetails.Extensions["traceId"] = RequestTraceResolver.Resolve(ctx.HttpContext);
                ctx.ProblemDetails.Extensions["timeStamp"] = DateTime.Now;
                ctx.ProblemDetails.Instance = ctx.HttpContext.Request.Path;
            };
        });

        return services;
    }
    private static IServiceCollection RegisterAppExceptionHandlers(this IServiceCollection services)
    {
        services.AddExceptionHandler<ConflictExceptionHandler>()
            .AddExceptionHandler<ValidationFailureExceptionHandler>()
            .AddExceptionHandler<NotFoundExceptionHandler>()
            .AddExceptionHandler<ForbiddenExceptionHandler>()
            .AddExceptionHandler<UnAuthorizedExceptionHandler>();

        return services;
    }
    private static IServiceCollection RegisterDomainExceptionHandlers(this IServiceCollection services)
    {
        services.AddExceptionHandler<DomainLogicalExceptionHandler>()
            .AddExceptionHandler<DomainValidationFailureExceptionHandler>()
            .AddExceptionHandler<DomainAuthorizationExceptionHandler>();

        return services;
    }
    private static IServiceCollection RegisterGlobalExceptionHandlers(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();

        return services;
    }
}
