using TaskManagement.Application.Interfaces.Repositories.Log;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.UnitOfWorks;

public class LogUnitOfWork : ILogUnitOfWork
{
    private readonly LogDbContext _context;
    public IEntityLogRepository EntityLog { get; }
    public IEntityRelationLogRepository EntityRelationLog { get; }

    public LogUnitOfWork(LogDbContext context, IEntityLogRepository entityLog, IEntityRelationLogRepository entityRelationLog)
    {
        _context = context;
        EntityLog = entityLog;
        EntityRelationLog = entityRelationLog;
    }

    public void Save()
    {
        _context.SaveChanges();
    }
    public void Save(bool acceptAllChangesOnSuccess)
    {
        _context.SaveChanges(acceptAllChangesOnSuccess);
    }
    public async Task SaveAsync(CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);
    }
    public async Task SaveAsync(bool acceptAllChangesOnSuccess, CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(acceptAllChangesOnSuccess, ct);
    }
}
