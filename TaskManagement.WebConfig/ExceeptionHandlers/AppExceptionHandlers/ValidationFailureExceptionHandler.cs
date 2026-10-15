using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManagement.Application.Utilities.Exceptions;

namespace TaskManagement.WebConfig.ExceeptionHandlers.AppExceptionHandlers;

public class ValidationFailureExceptionHandler(IProblemDetailsService _problemDetailsService)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken ct)
    {
        if (exception is not ValidationFailureException validation)
            return false;

        httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        var problemDetails = new ProblemDetails
        {
            Status = httpContext.Response.StatusCode,
            Title = "Invalide Request!",
            Detail = validation.Message,
            Type = "https://www.rfc-editor.org/info/rfc9110/#section-15.5.1"
        };
        problemDetails.Extensions["Errors"] = validation.ErrorMessages;

        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails
        });
    }
}
