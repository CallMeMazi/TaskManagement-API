using Microsoft.AspNetCore.Http;
using TaskManagement.Application.Interfaces.Services.Log;

namespace TaskManagement.WebConfig.Logging;

public class HttpRequestTrace(IHttpContextAccessor httpContextAccessor) 
    : IRequestTrace
{
    public string? TraceId => RequestTraceResolver.Resolve(httpContextAccessor.HttpContext);
}
