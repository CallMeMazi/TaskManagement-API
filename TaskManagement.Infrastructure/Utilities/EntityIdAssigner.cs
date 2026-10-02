using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Domain.Entities.BaseEntities;

namespace TaskManagement.Infrastructure.Utilities;

internal static class EntityIdAssigner
{
    public static void EnsureId(BaseEntity entity, IIdGeneratorService idGenerator, Type? entityClrType = null)
    {
        if (entity.Id != 0)
            return;

        entity.AssignId(idGenerator.NextId(entityClrType ?? entity.GetType()).Result);
    }
}
