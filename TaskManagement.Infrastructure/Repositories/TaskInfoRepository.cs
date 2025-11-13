using AutoMapper;
using TaskManagement.Application.DTOs.SharedDTOs.TaskInfo;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domin.Entities.BaseEntities;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;
public class TaskInfoRepository 
    : BaseRepository<TaskInfo, TaskInfoDetailsDto>, ITaskInfoRepository
{
    public TaskInfoRepository(ApplicationDbContext dbContext, IMapper mapper)
        : base(dbContext, mapper) { }
}
