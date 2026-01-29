using TaskManagement.Domin.Entities.BaseEntities;
using TaskManagement.Domin.Interface.Repository;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;
public class TaskInfoRepository 
    : BaseRepository<TaskInfo>, ITaskInfoRepository
{
    public TaskInfoRepository(ApplicationDbContext dbContext)
        : base(dbContext) { }
}
