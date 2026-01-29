using TaskManagement.Application.DTOs.ApplicationDTOs.Project;
using TaskManagement.Application.DTOs.ApplicationDTOs.TaskInfo;
using TaskManagement.Application.DTOs.SharedDTOs.TaskInfo;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Interfaces.Services.Main;
public interface ITaskInfoService
{
    Task<GeneralResult<TaskInfoDetailsDto>> GetTaskInfoByIdAsync(int taskInfoId, CancellationToken ct);
    Task<GeneralResult> CreateTaskInfoAsync(CreateTaskInfoAppDto command, CancellationToken ct);
    Task<GeneralResult> EndTaksInfoAsync(EndTaskInfoAppDto command, CancellationToken ct);
}
