using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Domain.Entities.BaseEntities;
using TaskManagement.Domain.Interface.Repository;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;
public class TaskInfoRepository 
    : BaseRepository<TaskInfo>, ITaskInfoRepository
{
    public TaskInfoRepository(ApplicationDbContext dbContext, IIdGenerator idGenerator)
        : base(dbContext, idGenerator) { }
}
