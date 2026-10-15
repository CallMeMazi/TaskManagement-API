using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManagement.Domain.Utilities.Exceptions;

namespace TaskManagement.WebConfig.ExceeptionHandlers.DomainExceptionHandlers;

public class DomainValidationFailureExceptionHandler(IProblemDetailsService _problemDetailsService)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken ct)
    {
        if (exception is not DomainValidationFailureException domainValidation)
            return false;

        // log this excepiton

        httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        var problemDetails = new ProblemDetails
        {
            Status = httpContext.Response.StatusCode,
            Title = "Access Denaid!",
            Detail = domainValidation.Message,
            Type = "https://www.rfc-editor.org/info/rfc9110/#section-15.5.1"
        };
        problemDetails.Extensions["Errors"] = domainValidation.ErrorMessages;

        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails
        });
    }
}
