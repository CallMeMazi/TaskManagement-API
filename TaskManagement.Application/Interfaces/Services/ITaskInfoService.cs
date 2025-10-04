using TaskManagement.Application.DTOs.ApplicationDTOs.Project;
using TaskManagement.Application.DTOs.ApplicationDTOs.TaskInfo;
using TaskManagement.Application.DTOs.SharedDTOs.TaskInfo;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Interfaces.Services;
public interface ITaskInfoService
{
    Task<GeneralResult<TaskInfoDetails>> GetTaskInfoByIdAsync(Guid taskInfoId, CancellationToken cancellationToken);
    Task<GeneralResult> CreateTaskInfoAsync(CreateTaskInfoAppDto command, CancellationToken cancellationToken);
    Task<GeneralResult> EndTaksInfoAsync(EndTaskInfoAppDto command, CancellationToken cancellationToken);
}
