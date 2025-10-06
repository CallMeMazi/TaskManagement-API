using System.Linq.Expressions;
using TaskManagement.Application.DTOs.SharedDTOs.Task;

namespace TaskManagement.Application.Interfaces.Repositories;
public interface ITaskRepository : IBaseRepository<Domin.Entities.BaseEntities.Task>
{
    Task AddRangeTasksAsync(IEnumerable<Domin.Entities.BaseEntities.Task> entities, CancellationToken cancellationToken = default);
    Task AddTaskAsync(Domin.Entities.BaseEntities.Task entity, CancellationToken cancellationToken = default);
    void DeleteRangeTasks(IEnumerable<Domin.Entities.BaseEntities.Task> entities);
    void DeleteTask(Domin.Entities.BaseEntities.Task entity);
    ValueTask<Domin.Entities.BaseEntities.Task?> FindTasksByIdsAsync(CancellationToken cancellationToken, params object[] ids);
    Task<List<TaskDetailsDto>> GetAllTaskDtosAsync(CancellationToken cancellationToken = default);
    Task<List<Domin.Entities.BaseEntities.Task>> GetAllTasksAsync(bool isTracking = false, CancellationToken cancellationToken = default);
    Task<Domin.Entities.BaseEntities.Task?> GetTaskByFilterAsync(Expression<Func<Domin.Entities.BaseEntities.Task, bool>> filter, bool isTracking = false, CancellationToken cancellationToken = default);
    Task<Domin.Entities.BaseEntities.Task?> GetTaskByIdAsync(Guid taskId, bool isTracking = false, CancellationToken cancellationToken = default);
    Task<TaskDetailsDto?> GetTaskDtoByFilterAsync(Expression<Func<Domin.Entities.BaseEntities.Task, bool>> filter, CancellationToken cancellationToken = default);
    Task<TaskDetailsDto?> GetTaskDtoByIdAsync(Guid taskId, CancellationToken cancellationToken = default);
    void UpdateRangeTasks(IEnumerable<Domin.Entities.BaseEntities.Task> entities);
    void UpdateTask(Domin.Entities.BaseEntities.Task entity);
}
