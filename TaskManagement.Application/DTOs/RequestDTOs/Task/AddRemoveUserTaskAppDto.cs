namespace TaskManagement.Application.DTOs.ApplicationDTOs.Task;
public record AddRemoveUserTaskAppDto(
    int OwnerId,
    int UserId,
    int TaskId,
    int ProjId
);