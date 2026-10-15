using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManagement.Domain.Utilities.Exceptions;
using static System.Net.WebRequestMethods;

namespace TaskManagement.WebConfig.ExceeptionHandlers.DomainExceptionHandlers;

public class DomainAuthorizationExceptionHandler(IProblemDetailsService _problemDetailsService)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken ct)
    {
        if (exception is not DomainAuthorizationException domainAuthorization)
            return false;

        // log this exception

        httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;

        var problemDetails = new ProblemDetails
        {
            Status = httpContext.Response.StatusCode,
            Title = "Access Denaid!",
            Detail = domainAuthorization.Message,
            Type = "https://www.rfc-editor.org/info/rfc9110/#section-15.5.4"
        };

        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails
        });
    }
}
