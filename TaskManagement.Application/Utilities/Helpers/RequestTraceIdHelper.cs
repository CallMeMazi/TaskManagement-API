using TaskManagement.Common.Helpers;
using TaskManagement.Domain.Entities.LogEntities;

namespace TaskManagement.Application.Utilities.Helpers;

public static class RequestTraceIdHelper
{
    public const string EmptyActivityTraceId = "00000000000000000000000000000000";

    public static string? Fit(string? traceId)
    {
        if (traceId.IsNullParameter() || traceId == EmptyActivityTraceId)
            return null;

        return traceId!.Trim().Length <= AuditLog.TraceIdMaxLength
            ? traceId
            : traceId[..AuditLog.TraceIdMaxLength];
    }
}
