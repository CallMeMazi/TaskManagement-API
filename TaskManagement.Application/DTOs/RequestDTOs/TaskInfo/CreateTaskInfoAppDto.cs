namespace TaskManagement.Application.DTOs.RequestDTOs.TaskInfo;
public record CreateTaskInfoAppDto(
    long TaskId,
    long UserId,
    string TaskInfoDescription
);