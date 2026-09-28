using TaskManagement.Application.Interfaces.Repositories.Log;
using TaskManagement.Domain.Entities.LogEntities;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories.LogRepositories;

public class EntityLogRepository 
    : LogBaseRepository<EntityLog>, IEntityLogRepository
{
    public EntityLogRepository(LogDbContext context)
        : base(context) { }
}
