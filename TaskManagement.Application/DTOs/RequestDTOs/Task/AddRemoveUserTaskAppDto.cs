namespace TaskManagement.Application.DTOs.RequestDTOs.Task;
public record AddRemoveUserTaskAppDto(
    long OwnerId,
    long UserId,
    long TaskId,
    long ProjId
);