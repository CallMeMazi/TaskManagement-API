namespace TaskManagement.Application.DTOs.RequestDTOs.Task;
public record UpdateTaskAppDto(
    int UserId,
    int TaskId,
    string TaskName,
    string TaskDescription,
    DateTime TaskDeadLine
);