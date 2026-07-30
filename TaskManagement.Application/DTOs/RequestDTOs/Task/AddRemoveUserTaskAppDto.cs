namespace TaskManagement.Application.DTOs.RequestDTOs.Task;
public record AddRemoveUserTaskAppDto(
    int OwnerId,
    int UserId,
    int TaskId,
    int ProjId
);