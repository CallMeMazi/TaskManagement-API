using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TaskManagement.Common.Helpers;

namespace TaskManagement.Infrastructure.Persistence.Interceptors;

// Normalizes characters on mapped string columns before SQL is generated
// Registered only on the application DbContext
public sealed class NormalizeStringInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        NormalizeStrings(eventData.Context);
        return base.SavingChanges(eventData, result);
    }


    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        NormalizeStrings(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void NormalizeStrings(DbContext? context)
    {
        if (context is null)
            return;

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.State is not (EntityState.Added or EntityState.Modified))
                continue;

            foreach (var property in entry.Metadata.GetProperties())
            {
                if (property.ClrType != typeof(string))
                    continue;

                var propertyEntry = entry.Property(property.Name);
                if (entry.State == EntityState.Modified && !propertyEntry.IsModified)
                    continue;

                if (propertyEntry.CurrentValue is not string value || value.IsNullParameter())
                    continue;

                var cleaned = value.FixPersianCharsFull();
                if (cleaned is null || cleaned == value)
                    continue;

                propertyEntry.CurrentValue = cleaned;
            }
        }
    }
}
