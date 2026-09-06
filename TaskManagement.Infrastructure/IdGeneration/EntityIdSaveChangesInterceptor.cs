using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Domain.Entities.BaseEntities;

namespace TaskManagement.Infrastructure.IdGeneration;

/// <summary>
/// Assigns snowflake ids to newly added aggregates right before SQL is generated.
/// Create flows do not need to call the generator themselves.
/// </summary>
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
        CancellationToken ct = default)
    {
        AssignMissingIds(eventData.Context);
        return base.SavingChangesAsync(eventData, result, ct);
    }

    private void AssignMissingIds(DbContext? context)
    {
        if (context is null)
            return;

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.State != EntityState.Added)
                continue;

            if (entry.Entity is BaseEntity baseEntity && baseEntity.Id == 0)
                baseEntity.AssignId(_idGenerator.NextId(entry.Entity.GetType()));
        }
    }
}
