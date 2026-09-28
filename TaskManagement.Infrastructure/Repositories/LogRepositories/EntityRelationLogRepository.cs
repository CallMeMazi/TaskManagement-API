using TaskManagement.Application.Interfaces.Repositories.Log;
using TaskManagement.Domain.Entities.LogEntities;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories.LogRepositories;

public class EntityRelationLogRepository
    : LogBaseRepository<EntityRelationLog>, IEntityRelationLogRepository
{
    public EntityRelationLogRepository(LogDbContext context)
        : base(context) { }
}
