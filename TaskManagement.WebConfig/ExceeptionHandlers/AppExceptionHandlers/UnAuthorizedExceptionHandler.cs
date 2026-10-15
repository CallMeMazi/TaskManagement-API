using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManagement.Application.Utilities.Exceptions;

namespace TaskManagement.WebConfig.ExceeptionHandlers.AppExceptionHandlers;

public class UnAuthorizedExceptionHandler(IProblemDetailsService _problemDetailsService)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken ct)
    {
        if (exception is not UnAuthorizedException unauthorized)
            return false;

        httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

        var problemDetails = new ProblemDetails
        {
            Status = httpContext.Response.StatusCode,
            Title = "Unauthorized!",
            Detail = unauthorized.Message,
            Type = "https://www.rfc-editor.org/info/rfc9110/#section-15.5.2"
        };

        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails
        });
    }
}
