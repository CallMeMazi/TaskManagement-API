using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using TaskManagement.Application.Utilities.Helpers;
using TaskManagement.Common.Helpers;

namespace TaskManagement.WebConfig.Logging;

public static class RequestTraceResolver
{
    public static string? Resolve(HttpContext? httpContext)
    {
        var activity = Activity.Current;
        if (!activity.IsNullParameter())
        {
            var activityTraceId = activity!.TraceId.ToString();
            if (activityTraceId != RequestTraceIdHelper.EmptyActivityTraceId)
                return RequestTraceIdHelper.Fit(activityTraceId);
        }

        return RequestTraceIdHelper.Fit(httpContext?.TraceIdentifier);
    }
}
