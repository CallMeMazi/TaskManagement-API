using TaskManagement.Domain.Enums.Logs;

namespace TaskManagement.Application.Interfaces.Services.Halper;

// Decoded fields of a generated 64-bit entity id
// [1 sign=0][42 timestamp ms][5 entityType][3 worker][10 sequence][3 version]
public readonly record struct EntityIdParts(
    long TimestampMs,
    EntityType EntityType,
    byte WorkerId,
    int Sequence,
    byte Version
);
