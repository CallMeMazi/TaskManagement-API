namespace TaskManagement.Application.DTOs.ApplicationDTOs.Task;
public record UpdateTaskAppDto(
    int UserId,
    int TaskId,
    string TaskName,
    string TaskDescription,
    DateTime TaskDeadLine
);