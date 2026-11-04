using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace TaskManagement.WebConfig.Logging;

public class RequestTraceEnricher(IHttpContextAccessor httpContextAccessor) 
    : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        if (logEvent.Properties.ContainsKey("TraceId"))
            return;

        var traceId = RequestTraceResolver.Resolve(httpContextAccessor.HttpContext);
        if (traceId is null)
            return;

        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("TraceId", traceId));
    }
}
