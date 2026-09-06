using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs.RequestDTOs.Task;
public record CreateTaskAppDto(
    long ProjId,
    long UserId,
    string TaskName,
    string TaskDescription,
    TaskType TaskType,
    DateTime TaskDeadLine,
    List<long> UserIds
);