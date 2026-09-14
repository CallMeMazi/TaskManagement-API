using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Domain.Entities.BaseEntities;

namespace TaskManagement.Infrastructure.IdGeneration;

internal static class EntityIdAssigner
{
    public static void EnsureId(BaseEntity entity, IIdGenerator idGenerator, Type? entityClrType = null)
    {
        if (entity.Id != 0)
            return;

        entity.AssignId(idGenerator.NextId(entityClrType ?? entity.GetType()));
    }
}
