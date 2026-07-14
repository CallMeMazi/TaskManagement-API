namespace TaskManagement.Application.DTOs.ApplicationDTOs.Task;
public record ChangeTaskProgressAppDto(
    int UserId,
    int TaskId,
    byte TaskProgress
);