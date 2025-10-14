using System.Linq.Expressions;
using TaskManagement.Application.DTOs.SharedDTOs.TaskInfo;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Application.Interfaces.Repositories;
public interface ITaskInfoRepository : IBaseRepository<TaskInfo>
{
    System.Threading.Tasks.Task AddRangeTaskInfosAsync(IEnumerable<TaskInfo> entities, CancellationToken cancellationToken = default);
    System.Threading.Tasks.Task AddTaskInfoAsync(TaskInfo entity, CancellationToken cancellationToken = default);
    void DeleteRangeTaskInfos(IEnumerable<TaskInfo> entities);
    void DeleteTaskInfo(TaskInfo entity);
    ValueTask<TaskInfo?> FindTaskInfosByIdsAsync(CancellationToken cancellationToken, params object[] ids);
    Task<List<TaskInfoDetailsDto>> GetAllTaskInfoDtosAsync(CancellationToken cancellationToken = default);
    Task<List<TaskInfo>> GetAllTaskInfosAsync(bool isTracking = false, CancellationToken cancellationToken = default);
    Task<TaskInfo?> GetTaskInfoByFilterAsync(Expression<Func<TaskInfo, bool>> filter, bool isTracking = false, CancellationToken cancellationToken = default);
    Task<TaskInfo?> GetTaskInfoByIdAsync(int taskInfoId, bool isTracking = false, CancellationToken cancellationToken = default);
    Task<TaskInfoDetailsDto?> GetTaskInfoDtoByFilterAsync(Expression<Func<TaskInfo, bool>> filter, CancellationToken cancellationToken = default);
    Task<TaskInfoDetailsDto?> GetTaskInfoDtoByIdAsync(int taskInfoId, CancellationToken cancellationToken = default);
    void UpdateRangeTaskInfos(IEnumerable<TaskInfo> entities);
    void UpdateTaskInfo(TaskInfo entity);
}
