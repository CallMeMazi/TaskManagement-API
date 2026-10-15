using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManagement.Domain.Utilities.Exceptions;

namespace TaskManagement.WebConfig.ExceeptionHandlers.DomainExceptionHandlers;

public class DomainLogicalExceptionHandler(IProblemDetailsService _problemDetailsService)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken ct)
    {
        if (exception is not DomainLogicalException domainLogical)
            return false;

        // log this exception

        httpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;

        var problemDetails = new ProblemDetails
        {
            Status = httpContext.Response.StatusCode,
            Title = "Conflict Error!",
            Detail = domainLogical.Message,
            Type = "https://www.rfc-editor.org/info/rfc9110/#section-15.5.10"
        };

        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails
        });
    }
}
