namespace TaskManagement.Application.DTOs.RequestDTOs.TaskInfo;
public record CreateTaskInfoAppDto(
    int TaskId,
    int UserId,
    int TaskAssignmentId,
    DateTime StartedTaskAt,
    DateTime EndedTaskAt
);