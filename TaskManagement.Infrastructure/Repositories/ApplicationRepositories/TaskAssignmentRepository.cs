using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Domain.Entities.BaseEntities;
using TaskManagement.Domain.Interface.Repository;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories.ApplicationRepositories;
public class TaskAssignmentRepository 
    : BaseRepository<TaskAssignment>, ITaskAssignmentRepository
{
    public TaskAssignmentRepository(ApplicationDbContext context, ICommonService commonService)
        : base(context, commonService) { }
}
