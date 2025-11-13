using AutoMapper;
using TaskManagement.Application.DTOs.SharedDTOs.Task;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;
public class TaskRepository 
    : BaseRepository<Domin.Entities.BaseEntities.Task, TaskDetailsDto>, ITaskRepository
{
    public TaskRepository(ApplicationDbContext dbContext, IMapper mapper)
        : base(dbContext, mapper) { }
}
