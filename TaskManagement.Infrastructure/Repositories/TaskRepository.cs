using TaskManagement.Domin.Interface.Repository;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;
public class TaskRepository 
    : BaseRepository<Domin.Entities.BaseEntities.Task>, ITaskRepository
{
    public TaskRepository(ApplicationDbContext dbContext)
        : base(dbContext) { }
}
