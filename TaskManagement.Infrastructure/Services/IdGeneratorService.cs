using TaskManagement.Application.DTOs.InternalDTOs.Common;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Common.Classes;
using TaskManagement.Common.Settings;
using TaskManagement.Domain.Entities.BaseEntities;
using TaskManagement.Domain.Enums.Types.Application;
using DomainTask = TaskManagement.Domain.Entities.BaseEntities.Task;

namespace TaskManagement.Infrastructure.Services;

// Custom snowflake generator.
public class IdGeneratorService : IIdGeneratorService
{
    // [1 sign][42 timestamp][5 entityType][3 worker][10 sequence][3 version]
    private const byte VersionBits = 3;
    private const byte SequenceBits = 10;
    private const byte WorkerBits = 3;
    private const byte EntityBits = 5;
    private const byte TimestampBits = 42;

    private const byte VersionShift = 0;
    private const byte SequenceShift = VersionBits;
    private const byte WorkerShift = SequenceShift + SequenceBits;
    private const byte EntityShift = WorkerShift + WorkerBits;
    private const byte TimestampShift = EntityShift + EntityBits;

    private const long SequenceMask = (1L << SequenceBits) - 1;
    private const long WorkerMask = (1L << WorkerBits) - 1;
    private const long EntityMask = (1L << EntityBits) - 1;
    private const long TimestampMask = (1L << TimestampBits) - 1;
    private const long VersionMask = (1L << VersionBits) - 1;

    // Current id format. Bump this when the bit layout changes and decode by version.
    private const byte CurrentVersion = 1;

    // Custom epoch. Timestamp field is milliseconds since this instant.
    private static readonly DateTime EpochUtc = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Local);

    private static readonly Dictionary<Type, EntityType> EntityTypeMap = new()
    {
        [typeof(User)] = EntityType.User,
        [typeof(Organization)] = EntityType.Organization,
        [typeof(OrganizationMemberShip)] = EntityType.OrganizationMemberShip,
        [typeof(Project)] = EntityType.Project,
        [typeof(ProjectMemberShip)] = EntityType.ProjectMemberShip,
        [typeof(DomainTask)] = EntityType.Task,
        [typeof(TaskAssignment)] = EntityType.TaskAssignment,
        [typeof(TaskInfo)] = EntityType.TaskInfo,
        [typeof(UserToken)] = EntityType.UserToken,
        [typeof(OrganizationInvitation)] = EntityType.OrganizationInvitation
    };

    private readonly byte _workerId;
    private readonly object _sync = new();
    private long _lastTimestamp;
    private int _sequence;

    public IdGeneratorService(AppSettings appSettings)
    {
        var workerId = appSettings.IdGeneratorSetting?.WorkerId ?? 0;
        if (workerId > WorkerMask)
            throw new ArgumentOutOfRangeException(nameof(appSettings), $"WorkerId must be between 0 and {WorkerMask}. In {nameof(IdGeneratorService)} Constractor!");

        _workerId = workerId;
    }


    public GeneralResult<long> NextId(Type entityClrType)
    {
        if (entityClrType is null)
            return GeneralResult<long>.Failure($"The entity type is null. In {nameof(NextId)} method!");

        if (!EntityTypeMap.TryGetValue(entityClrType, out var entityType))
            return GeneralResult<long>.Failure($"No id mapping is registered for '{entityClrType.Name}'. In {nameof(NextId)} method!");

        var idResult = NextId(entityType);
        if (!idResult.IsSuccess)
            return GeneralResult<long>.Failure(idResult.Message);

        return GeneralResult<long>.Success(idResult.Result);
    }
    public GeneralResult<long> NextId(EntityType entityType)
    {
        var entityValue = (long)entityType;
        if (entityValue < 0 || entityValue > EntityMask)
            return GeneralResult<long>.Failure($"Entity type does not fit in the 5-bit tag. In {nameof(NextId)} method!");

        lock (_sync)
        {
            var timestamp = GetCurrentTimestamp();
            if (timestamp < _lastTimestamp)
                timestamp = WaitUntil(_lastTimestamp);

            if (timestamp == _lastTimestamp)
            {
                _sequence = (int)((_sequence + 1) & SequenceMask);
                if (_sequence == 0)
                    timestamp = WaitUntil(_lastTimestamp + 1);
            }
            else
            {
                _sequence = 0;
            }

            _lastTimestamp = timestamp;
            var id = Pack(timestamp, entityValue, _workerId, _sequence, CurrentVersion);

            return GeneralResult<long>.Success(id);
        }
    }
    public GeneralResult<EntityType> GetEntityType(long id)
    {
        var idPartsResult = Decode(id);
        if (!idPartsResult.IsSuccess)
            return GeneralResult<EntityType>.Failure(idPartsResult.Message);

        return GeneralResult<EntityType>.Success(idPartsResult.Result.EntityType);
    }
    public GeneralResult<EntityIdPartsInternalDto> Decode(long id)
    {
        if (id <= 0)
            return GeneralResult<EntityIdPartsInternalDto>.Failure($"Id must be a positive generated value. In {nameof(Decode)} method!");

        var version = (byte)((id >> VersionShift) & VersionMask);
        if (version != CurrentVersion)
            return GeneralResult<EntityIdPartsInternalDto>.Failure($"Unsupported id version '{version}'. In {nameof(Decode)} method!");

        var sequence = (int)((id >> SequenceShift) & SequenceMask);
        var workerId = (byte)((id >> WorkerShift) & WorkerMask);
        var entityType = (EntityType)((id >> EntityShift) & EntityMask);
        var timestampMs = (id >> TimestampShift) & TimestampMask;

        var idParts = new EntityIdPartsInternalDto(timestampMs, entityType, workerId, sequence, version);

        return GeneralResult<EntityIdPartsInternalDto>.Success(idParts);
    }
    public GeneralResult ValidateTypeId(long id, EntityType entityType)
    {
        var idPartsResult = Decode(id);
        if (!idPartsResult.IsSuccess)
            return GeneralResult.Failure(idPartsResult.Message);

        return idPartsResult.Result.EntityType == entityType 
            ? GeneralResult.Success()
            : GeneralResult.Failure();
    }

    private static long Pack(long timestamp, long entityType, byte workerId, int sequence, byte version)
    {
        if (timestamp > TimestampMask)
            throw new InvalidOperationException("Timestamp overflowed the 42-bit field. Choose a later epoch or a new id version.");

        return (timestamp << TimestampShift)
            | (entityType << EntityShift)
            | ((workerId & WorkerMask) << WorkerShift)
            | ((sequence & SequenceMask) << SequenceShift)
            | (version & VersionMask);
    }
    private static long GetCurrentTimestamp()
    {
        var timestamp = (long)(DateTime.Now - EpochUtc).TotalMilliseconds;
        if (timestamp < 0)
            throw new InvalidOperationException("System clock is before the id-generator epoch.");

        return timestamp;
    }
    private static long WaitUntil(long targetTimestamp)
    {
        var timestamp = GetCurrentTimestamp();
        while (timestamp < targetTimestamp)
        {
            Thread.Sleep(1);
            timestamp = GetCurrentTimestamp();
        }

        return timestamp;
    }
}
