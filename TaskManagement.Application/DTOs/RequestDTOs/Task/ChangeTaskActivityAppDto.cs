namespace TaskManagement.Application.DTOs.ApplicationDTOs.Task;
public record ChangeTaskActivityAppDto(
    int UserId,
    int TaskId,
    bool Activity
);