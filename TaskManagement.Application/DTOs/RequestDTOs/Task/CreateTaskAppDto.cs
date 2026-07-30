using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs.ApplicationDTOs.Task;
public record CreateTaskAppDto(
    int ProjId,
    int UserId,
    string TaskName,
    string TaskDescription,
    TaskType TaskType,
    DateTime TaskDeadLine,
    List<int> UserIds
);