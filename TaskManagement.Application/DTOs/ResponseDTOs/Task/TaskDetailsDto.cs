using TaskManagement.Domain.Enums.Statuses;
using TaskManagement.Domain.Enums.Types.Application;

namespace TaskManagement.Application.DTOs.ResponseDTOs.Task;
public record TaskDetailsDto(
    string TaskName,
    string TaskDescription,
    bool IsActive,
    TaskType TaskType,
    TaskStatusType TaskStatus,
    DateTime TaskDeadline,
    byte TaskProgress,
    DateTime CreatedAt
);