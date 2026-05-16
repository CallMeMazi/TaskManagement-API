using TaskManagement.Domain.Entities.BaseEntities;
using TaskManagement.Domain.Interface.Repository;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;
public class TaskInfoRepository 
    : BaseRepository<TaskInfo>, ITaskInfoRepository
{
    public TaskInfoRepository(ApplicationDbContext dbContext)
        : base(dbContext) { }
}
