namespace TaskManagement.Application.DTOs.RequestDTOs.Task;
public record ChangeTaskActivityAppDto(
    int UserId,
    int TaskId,
    bool Activity
);