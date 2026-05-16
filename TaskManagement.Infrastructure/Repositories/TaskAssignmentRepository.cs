using TaskManagement.Domain.Entities.BaseEntities;
using TaskManagement.Domain.Interface.Repository;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;
public class TaskAssignmentRepository 
    : BaseRepository<TaskAssignment>, ITaskAssignmentRepository
{
    public TaskAssignmentRepository(ApplicationDbContext context)
        : base(context) { }
}
