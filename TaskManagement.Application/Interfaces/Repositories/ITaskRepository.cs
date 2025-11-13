using TaskManagement.Application.DTOs.SharedDTOs.Task;

namespace TaskManagement.Application.Interfaces.Repositories;
public interface ITaskRepository : IBaseRepository<Domin.Entities.BaseEntities.Task, TaskDetailsDto>
{
}
