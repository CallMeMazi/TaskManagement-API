using TaskManagement.Application.DTOs.SharedDTOs.TaskInfo;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Application.Interfaces.Repositories;
public interface ITaskInfoRepository : IBaseRepository<TaskInfo, TaskInfoDetailsDto>
{
}
