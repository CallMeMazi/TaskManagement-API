using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace TaskManagement.WebConfig.ExceeptionHandlers;

public class GlobalExceptionHandler(
    IProblemDetailsService _problemDetailsService,
    ILogger<GlobalExceptionHandler> _logger) 
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken ct)
    {
        _logger.LogError(exception, "UnhandledException {ExceptionType}", exception.GetType().FullName);

        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var problemDetails = new ProblemDetails
        {
            Title = "Internal Server Error!",
            Status = httpContext.Response.StatusCode,
            Type = "https://www.rfc-editor.org/info/rfc9110/#section-15.6.1",
            Detail = "Sorry, An Unhandled Internal Server Error, Try Again Later!"
        };

        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails
        });
    }
}
