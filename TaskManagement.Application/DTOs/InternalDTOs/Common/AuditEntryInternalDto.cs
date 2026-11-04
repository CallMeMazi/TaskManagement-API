using TaskManagement.Domain.Enums.Types.Application;

namespace TaskManagement.Application.DTOs.InternalDTOs.Common;

public sealed record AuditEntryInternalDto(
    string Action,
    EntityType EntityType,
    long EntityId,
    long? ActorUserId,
    EntityType? RelatedEntityType = null,
    long? RelatedEntityId = null,
    IReadOnlyDictionary<string, string>? Metadata = null
);
