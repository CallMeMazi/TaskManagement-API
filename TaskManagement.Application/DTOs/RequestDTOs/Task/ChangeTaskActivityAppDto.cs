namespace TaskManagement.Application.DTOs.RequestDTOs.Task;
public record ChangeTaskActivityAppDto(
    long UserId,
    long TaskId,
    bool Activity
);