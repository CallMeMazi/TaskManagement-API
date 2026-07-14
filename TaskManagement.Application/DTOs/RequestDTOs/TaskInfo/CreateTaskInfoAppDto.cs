namespace TaskManagement.Application.DTOs.ApplicationDTOs.TaskInfo;
public record CreateTaskInfoAppDto(
    int TaskId,
    int UserId,
    int TaskAssignmentId,
    DateTime StartedTaskAt,
    DateTime EndedTaskAt
);