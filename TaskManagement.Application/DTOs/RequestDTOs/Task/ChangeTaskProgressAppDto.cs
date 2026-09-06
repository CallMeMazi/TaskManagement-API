namespace TaskManagement.Application.DTOs.RequestDTOs.Task;
public record ChangeTaskProgressAppDto(
    long UserId,
    long TaskId,
    byte TaskProgress
);