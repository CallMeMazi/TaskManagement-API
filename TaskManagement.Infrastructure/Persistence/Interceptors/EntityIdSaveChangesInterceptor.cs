using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Domain.Entities.BaseEntities;
using TaskManagement.Infrastructure.IdGeneration;

namespace TaskManagement.Infrastructure.Persistence.Interceptors;

// Safety: assigns a snowflake id to Added aggregates that still have Id == 0
// Primary assignment happens in BaseRepository before SaveChanges
public sealed class EntityIdSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IIdGenerator _idGenerator;

    public EntityIdSaveChangesInterceptor(IIdGenerator idGenerator)
    {
        _idGenerator = idGenerator;
    }


    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        AssignMissingIds(eventData.Context);
        return base.SavingChanges(eventData, result);
    }
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        AssignMissingIds(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void AssignMissingIds(DbContext? context)
    {
        if (context is null)
            return;

        foreach (var entry in context.ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State != EntityState.Added)
                continue;

            EntityIdAssigner.EnsureId(entry.Entity, _idGenerator, entry.Metadata.ClrType);
        }
    }
}
