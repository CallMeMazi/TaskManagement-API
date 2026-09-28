using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Domain.Entities.BaseEntities;
using TaskManagement.Domain.Interface.Repository;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories.ApplicationRepositories;
public class TaskInfoRepository 
    : BaseRepository<TaskInfo>, ITaskInfoRepository
{
    public TaskInfoRepository(ApplicationDbContext dbContext, ICommonService commonService)
        : base(dbContext, commonService) { }
}
