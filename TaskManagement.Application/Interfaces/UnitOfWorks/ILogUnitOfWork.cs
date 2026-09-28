using TaskManagement.Application.Interfaces.Repositories.Log;

namespace TaskManagement.Application.Interfaces.UnitOfWorks;

public interface ILogUnitOfWork
{
    IEntityLogRepository EntityLog { get; }
    IEntityRelationLogRepository EntityRelationLog { get; }

    void Save();
    void Save(bool acceptAllChangesOnSuccess);
    Task SaveAsync(CancellationToken ct = default);
    Task SaveAsync(bool acceptAllChangesOnSuccess, CancellationToken ct = default);
}
