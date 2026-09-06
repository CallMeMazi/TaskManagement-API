namespace TaskManagement.Application.DTOs.RequestDTOs.Task;
public record UpdateTaskAppDto(
    long UserId,
    long TaskId,
    string TaskName,
    string TaskDescription,
    DateTime TaskDeadLine
);