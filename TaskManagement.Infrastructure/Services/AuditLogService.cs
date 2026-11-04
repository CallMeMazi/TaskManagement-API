using Microsoft.Extensions.Logging;
using TaskManagement.Application.Auditing;
using TaskManagement.Application.DTOs.InternalDTOs.Common;
using TaskManagement.Application.Interfaces.Services.Log;
using TaskManagement.Application.Utilities.Helpers;
using TaskManagement.Common.Helpers;
using TaskManagement.Domain.Entities.LogEntities;

namespace TaskManagement.Infrastructure.Services;

public class AuditLogService(
    IAuditQueue queue,
    IRequestTrace requestTrace,
    ILogger<AuditLogService> logger) 
    : IAuditLogService
{
    public void Stage(AuditEntryInternalDto entry)
    {
        if (entry.IsNullParameter())
            throw new Exception($"The Log Entry Is Null, In {nameof(AuditLogService)}.{nameof(Stage)} Method!");

        var audit = AuditLog.CreateSucceeded(
            entry.Action,
            entry.EntityType,
            entry.EntityId,
            entry.ActorUserId,
            entry.RelatedEntityType,
            entry.RelatedEntityId,
            RequestTraceIdHelper.Fit(requestTrace.TraceId),
            AuditMetadata.Serialize(entry.Metadata));

        try
        {
            var res = queue.TryEnqueue(audit);
            if (!res)
                logger.LogError($"Can not add audit to queue, TraceId: {RequestTraceIdHelper.Fit(requestTrace.TraceId)}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Can not add audit to queue, TraceId: {RequestTraceIdHelper.Fit(requestTrace.TraceId)}");
        }
    }
}
