using TaskManagement.Domain.Enums.Types.Application;
using TaskManagement.Domain.Enums.Types.Logs;

namespace TaskManagement.Domain.Entities.LogEntities;

// Business audit row committed in the same application transaction as the change it describes.
// The table is not tamper-proof. Retention is an operational concern and is not enforced here.
public class AuditLog
{
    public const int ActionMaxLength = 100;
    public const int TraceIdMaxLength = 64;
    public const int MetadataMaxLength = 2000;

    public long Id { get; private set; }
    public DateTime OccurredAtUtc { get; private set; }
    public long? ActorUserId { get; private set; }
    public string Action { get; private set; } = null!;
    public EntityType EntityType { get; private set; }
    public long EntityId { get; private set; }
    public EntityType? RelatedEntityType { get; private set; }
    public long? RelatedEntityId { get; private set; }
    public AuditOutcome Outcome { get; private set; }
    public string? TraceId { get; private set; }
    public string? Metadata { get; private set; }

    private AuditLog()
    {
    }

    public static AuditLog CreateSucceeded(
        string action,
        EntityType entityType,
        long entityId,
        long? actorUserId,
        EntityType? relatedEntityType,
        long? relatedEntityId,
        string? traceId,
        string? metadata)
    {
        if (string.IsNullOrWhiteSpace(action) || action.Length > ActionMaxLength)
            throw new ArgumentException($"Audit action must be 1 to {ActionMaxLength} characters.", nameof(action));

        if (entityId <= 0)
            throw new ArgumentOutOfRangeException(nameof(entityId), "Audit entity id must be positive.");

        if (actorUserId is <= 0)
            throw new ArgumentOutOfRangeException(nameof(actorUserId), "Audit actor id must be positive when it is present.");

        if (relatedEntityType.HasValue != relatedEntityId.HasValue)
            throw new ArgumentException("Related entity type and id must both be set or both be empty.");

        if (relatedEntityId is <= 0)
            throw new ArgumentOutOfRangeException(nameof(relatedEntityId), "Related entity id must be positive when it is present.");

        if (traceId is not null && traceId.Length > TraceIdMaxLength)
            throw new ArgumentException($"Trace id must be at most {TraceIdMaxLength} characters.", nameof(traceId));

        if (metadata is not null && metadata.Length > MetadataMaxLength)
            throw new ArgumentException($"Audit metadata must be at most {MetadataMaxLength} characters.", nameof(metadata));

        return new AuditLog
        {
            OccurredAtUtc = DateTime.UtcNow,
            ActorUserId = actorUserId,
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            RelatedEntityType = relatedEntityType,
            RelatedEntityId = relatedEntityId,
            Outcome = AuditOutcome.Succeeded,
            TraceId = traceId,
            Metadata = metadata
        };
    }
}
