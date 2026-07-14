using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Enums.Statuses;

namespace TaskManagement.Application.DTOs.SharedDTOs.Task;
public record TaskDetailsDto(
    string TaskName,
    string TaskDescription,
    bool IsActive,
    TaskType TaskType,
    TaskStatusType TaskStatusType,
    DateTime TaskDeadline,
    byte TaskProgress,
    DateTime CreateAt
);