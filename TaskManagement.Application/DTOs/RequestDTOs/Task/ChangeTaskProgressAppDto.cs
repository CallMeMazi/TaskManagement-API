namespace TaskManagement.Application.DTOs.RequestDTOs.Task;
public record ChangeTaskProgressAppDto(
    int UserId,
    int TaskId,
    byte TaskProgress
);